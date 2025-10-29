using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V140.CacheStorage;
using OpenQA.Selenium.Support.UI;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

public class Catalog
{
    public string Title { get; set; } = "";
    public string Url { get; set; } = "";
    public string Description { get; set; } = "";
    public string BannerImageAddress { get; set; }
    public string CoverImageAddress { get; set; }
    public List<string> Categories { get; set; }
    public DateTime? PublishDate { get; set; }
    public string Status { get; set; }
    public Dictionary<string, string> Authors { get; set; }
    public List<string> Titles { get; set; }
}

class Program
{
    // <<<<<<<<<<<<<<<< EDIT THIS URL IF YOU NEED >>>>>>>>>>>>>>>>
    private const string TargetUrl =
        "https://mangadex.org/title/4809bb6c-1af2-41c5-9291-983027602d7f/shy";

    static void Main()
    {
        new DriverManager().SetUpDriver(new ChromeConfig());

        var options = new ChromeOptions();
        // comment this out if you want to see the browser
        options.AddArgument("--headless=new");
        options.AddArgument("--window-size=1280,2000");
        options.AddArgument("--disable-gpu");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");
        options.AddArgument("--lang=en-US");
        options.AddArgument("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120 Safari/537.36");

        using var driver = new ChromeDriver(options);
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

        try
        {
            driver.Navigate().GoToUrl(TargetUrl);
            var catalog = new Catalog();
            catalog.Url = TargetUrl;
            // Handle cookie banner if present (MangaDex uses a consent modal sometimes)
            TryAcceptCookies(driver, wait);

            ExtractBannerAndCover(driver, catalog);
            ExtractCategories(driver, catalog);
            ExtractDescriptionAndTitle(driver, catalog);
            ExtractPublishAndStatus(driver, catalog);
            ExtractTitles(driver, catalog);
            ExtractAuthors(driver, catalog);
            //var chapters = ExtractChapters(driver);

            Console.WriteLine("Saved: chapters.json, chapters.csv");
        }
        finally
        {
            driver.Quit();
        }
    }

    private static void ExtractAuthors(ChromeDriver driver, Catalog catalog)
    {
        var result = new Dictionary<string, string>();

        var infoBlocks = driver.FindElements(By.CssSelector("div.mb-2"));

        foreach (var block in infoBlocks)
        {
            try
            {
                // Label: "Author", "Artist", etc.
                var labelElement = block.FindElements(By.CssSelector(".font-bold"))
                                        .FirstOrDefault();
                if (labelElement == null) continue;

                var key = labelElement.Text.Trim();
                if (string.IsNullOrWhiteSpace(key)) continue;

                // Values: usually <a><span>NAME</span></a>
                var valueElements = block.FindElements(By.CssSelector("a span"));
                var values = valueElements
                    .Select(v => v.Text?.Trim())
                    .Where(t => !string.IsNullOrWhiteSpace(t))
                    .Distinct()
                    .ToList();

                if (values.Count == 0) continue;

                //if (result.ContainsKey(key))
                //    result[key].AddRange(values);
                //else
                //    result[key] = values;
            }
            catch
            {
                // Ignore parsing errors for malformed sections
            }
        }
        catalog.Authors = result;
    }

    private static void ExtractTitles(ChromeDriver driver, Catalog catalog)
    {
        var results = new List<AltTitle>();

        // Each alt title row looks like: <div class="alt-title"> [flag imgs] <span>Title</span> </div>
        var rows = driver.FindElements(By.CssSelector(".alt-title"));

        foreach (var row in rows)
        {
            try
            {
                // Title text
                var textEl = row.FindElements(By.CssSelector("span")).FirstOrDefault();
                var text = textEl?.Text?.Trim();
                if (string.IsNullOrWhiteSpace(text)) continue;

                // Language/script is stored in img[title], e.g. "Japanese (Romanized)"
                // Prefer the last img[title] in case there are multiple (flag + script icon)
                var langImg = row.FindElements(By.CssSelector("img[title]")).LastOrDefault();
                string? language = null;
                string? script = null;

                if (langImg != null)
                {
                    var raw = langImg.GetAttribute("title")?.Trim();
                    if (!string.IsNullOrWhiteSpace(raw))
                    {
                        // Split "Japanese (Romanized)" → language="Japanese", script="Romanized"
                        var open = raw.IndexOf('(');
                        var close = raw.LastIndexOf(')');
                        if (open >= 0 && close > open)
                        {
                            language = raw.Substring(0, open).Trim();
                            script = raw.Substring(open + 1, close - open - 1).Trim();
                        }
                        else
                        {
                            language = raw;
                        }
                    }
                }

                results.Add(new AltTitle(text, language, script));
            }
            catch
            {
                // ignore this row and continue
            }
        }


    }

    private static void ExtractPublishAndStatus(ChromeDriver driver, Catalog catalog)
    {
        try
        {
            // Target the <span> that contains "Publication:" text
            var span = driver.FindElements(By.CssSelector("span.tag"))
                             .FirstOrDefault(e => e.Text.Contains("Publication:", StringComparison.OrdinalIgnoreCase));

            if (span == null)
                return;

            // Example text: "Publication: 2025, Ongoing"
            var text = span.Text.Trim();

            // Remove prefix
            text = Regex.Replace(text, @"^Publication:\s*", "", RegexOptions.IgnoreCase);

            // Split by comma → e.g. ["2025", " Ongoing"]
            var parts = text.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            DateTime? publishDate = null;
            string? status = null;

            if (parts.Length > 0)
            {
                // Try to parse first part as year or full date
                var yearPart = parts[0];
                if (int.TryParse(yearPart, out int year))
                {
                    publishDate = new DateTime(year, 1, 1);
                }
                else if (DateTime.TryParse(yearPart, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
                {
                    publishDate = dt;
                }
            }

            if (parts.Length > 1)
            {
                status = parts[1];
            }
            catalog.PublishDate = publishDate ?? DateTime.MinValue;
            catalog.Status = status ?? "";
        }
        catch
        {
            return;
        }
    }

    private static void ExtractDescriptionAndTitle(ChromeDriver driver, Catalog catalog)
    {

        // The title text is inside <div class="title"><p>...</p></div>
        var titleElement = driver.FindElement(By.CssSelector(".title p"));
        var title = titleElement.Text?.Trim();
        catalog.Title = title ?? "";

        IWebElement? container = null;

        // 1️⃣ Prefer main synopsis block (grid-area: synopsis)
        var candidates = driver.FindElements(By.CssSelector("[style*='grid-area: synopsis']"));
        container = candidates.FirstOrDefault();

        // 2️⃣ Fallbacks for mobile/other layouts
        if (container == null)
        {
            var alt = driver.FindElements(By.CssSelector(".md-md-container, .synopsis, .description"));
            container = alt.FirstOrDefault();
        }

        if (container == null) return;

        // Collect paragraph texts
        var paragraphs = container.FindElements(By.CssSelector("p"))
                                  .Select(p => p.Text?.Trim())
                                  .Where(t => !string.IsNullOrWhiteSpace(t))
                                  .ToList();

        // If no <p> found, take container text
        string raw = paragraphs.Count > 0
            ? string.Join("\n\n", paragraphs)
            : container.Text?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(raw)) return;

        // Normalize whitespace
        raw = Regex.Replace(raw, @"[ \t]+", " ");      // collapse spaces
        raw = Regex.Replace(raw, @"\n{3,}", "\n\n");   // limit blank lines

        catalog.Description = raw.Trim();

    }

    private static void ExtractCategories(ChromeDriver driver, Catalog catalog)
    {
        var categories = new List<string>();

        // Find both <a> and <span> elements inside the tags container
        var tagElements = driver.FindElements(
            By.CssSelector(".tags-row a, .tags-row span")
        );

        foreach (var elem in tagElements)
        {
            var text = elem.Text?.Trim();
            if (!string.IsNullOrWhiteSpace(text))
            {
                categories.Add(text);
            }
        }

        catalog.Categories = categories;
    }
    private static void ExtractBannerAndCover(ChromeDriver driver, Catalog catalog)
    {
        var urls = new List<string>();

        // 1. Extract <a href=".../covers/...jpg">
        var coverElement = driver.FindElements(By.CssSelector("a[href*='/covers/']")).FirstOrDefault();
        var href = coverElement?.GetAttribute("href");
        if (!string.IsNullOrWhiteSpace(href))
        {
            href = href.Replace(".512.jpg", ".jpg").Replace(".256.jpg", ".jpg");
            catalog.CoverImageAddress = href;
        }


        // 2. Extract background-image URLs from inline styles
        var bannerElement = driver.FindElements(By.CssSelector(".banner-image[style*='background-image']")).FirstOrDefault();
        var style = bannerElement?.GetAttribute("style");
        if (!string.IsNullOrWhiteSpace(style))
        {
            var match = Regex.Match(style, @"url\(['""]?(?<url>https.*?\.jpg(?:\.\d+)?)[^'""]*['""]?\)");
            if (match.Success)
            {
                var bgUrl = match.Groups["url"].Value
                    .Replace(".512.jpg", ".jpg")
                    .Replace(".256.jpg", ".jpg");
                catalog.BannerImageAddress = bgUrl;
            }
        }
    }
    static void TryAcceptCookies(IWebDriver driver, WebDriverWait wait)
    {
        try
        {
            // Try common selectors/text for consent buttons
            var possibleButtons = new[]
            {
                "//button[.//text()[contains(.,'Accept') or contains(.,'I agree')]]",
                "//button[contains(@data-testid,'consent') or contains(.,'OK')]",
                "//button[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'),'accept')]"
            };

            foreach (var xp in possibleButtons)
            {
                var buttons = driver.FindElements(By.XPath(xp));
                if (buttons.Count > 0)
                {
                    buttons[0].Click();
                    Thread.Sleep(500);
                    break;
                }
            }
        }
        catch { /* ignore */ }
    }

}

public class AltTitle
{
    public string Text;
    public string? Language;
    public string? Script;

    public AltTitle(string text, string? language, string? script)
    {
        this.Text = text;
        this.Language = language;
        this.Script = script;
    }
}