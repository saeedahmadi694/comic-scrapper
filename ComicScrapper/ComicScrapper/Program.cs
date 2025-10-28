using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Text;
using System.Text.Json;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

class ChapterInfo
{
    public string Title { get; set; } = "";
    public string Url { get; set; } = "";
    public string? Lang { get; set; }
    public string? Group { get; set; }
    public string? Volume { get; set; }
    public string? Chapter { get; set; }
}

class Program
{
    // <<<<<<<<<<<<<<<< EDIT THIS URL IF YOU NEED >>>>>>>>>>>>>>>>
    private const string TargetUrl =
        "https://mangadex.org/title/2a55e420-b5c6-47cb-b189-fb9a1a7d6ab5/335km";

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

            // Handle cookie banner if present (MangaDex uses a consent modal sometimes)
            TryAcceptCookies(driver, wait);

            // Click the "Chapters" tab if the page shows tabs
            TryOpenChaptersTab(driver, wait);

            // Ensure chapters container is visible
            wait.Until(d =>
            {
                return d.FindElements(By.XPath("//a[contains(@href, '/chapter/')]")).Count > 0
                       || d.PageSource.Contains("/chapter/");
            });

            // Scroll to trigger lazy loading until it stops adding new /chapter/ links
            ScrollToLoadAll(driver);

            // Extract chapter rows
            var chapters = ExtractChapters(driver);

            Console.WriteLine($"Found {chapters.Count} chapters.");

            // Save to files
            File.WriteAllText("chapters.json",
                JsonSerializer.Serialize(chapters, new JsonSerializerOptions { WriteIndented = true }),
                Encoding.UTF8);

            WriteCsv("chapters.csv", chapters);

            Console.WriteLine("Saved: chapters.json, chapters.csv");
        }
        finally
        {
            driver.Quit();
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

    static void TryOpenChaptersTab(IWebDriver driver, WebDriverWait wait)
    {
        try
        {
            // Look for a tab/button with text "Chapters"
            var chaptersTab = driver.FindElements(By.XPath("//a[normalize-space()='Chapters' or contains(., 'Chapters')] | //button[normalize-space()='Chapters' or contains(., 'Chapters')]"));
            if (chaptersTab.Count > 0)
            {
                chaptersTab[0].Click();
                Thread.Sleep(700);
            }
        }
        catch { /* ignore */ }
    }

    static void ScrollToLoadAll(IWebDriver driver)
    {
        var js = (IJavaScriptExecutor)driver;

        int sameCount = 0;
        int lastLinkCount = 0;

        for (int i = 0; i < 80; i++) // hard stop to avoid infinite loops
        {
            // Scroll to bottom
            js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
            Thread.Sleep(700);

            // Sometimes additional content loads when scrolling up a bit
            js.ExecuteScript("window.scrollBy(0, -200);");
            Thread.Sleep(300);

            var currentLinkCount = driver.FindElements(By.CssSelector("a[href*='/chapter/']")).Count;
            if (currentLinkCount == lastLinkCount)
                sameCount++;
            else
                sameCount = 0;

            lastLinkCount = currentLinkCount;

            // If we haven't gained any links after a few scrolls, assume done
            if (sameCount >= 3)
                break;
        }
    }

    static List<ChapterInfo> ExtractChapters(IWebDriver driver)
    {
        var results = new List<ChapterInfo>();

        // General strategy:
        // - Find all <a> whose href contains "/chapter/"
        // - Use the closest text around the link as the chapter title
        // - Try to fetch badges like language, group, chapter number if present
        var chapterLinks = driver.FindElements(By.CssSelector("a[href*='/chapter/']"))
                                 .GroupBy(a => a.GetAttribute("href"))
                                 .Select(g => g.First())
                                 .ToList();

        foreach (var a in chapterLinks)
        {
            try
            {
                var url = a.GetAttribute("href") ?? "";
                if (string.IsNullOrWhiteSpace(url)) continue;

                // Grab a readable label: the link text or nearby text
                var title = a.Text?.Trim();
                if (string.IsNullOrWhiteSpace(title))
                {
                    // try parent text
                    try
                    {
                        var parent = a.FindElement(By.XPath(".."));
                        title = parent.Text?.Trim();
                    }
                    catch { /* ignore */ }
                }
                if (string.IsNullOrWhiteSpace(title))
                    title = System.IO.Path.GetFileName(url.TrimEnd('/'));

                // Attempt to parse extra bits commonly shown next to the link
                string? lang = null, group = null, volume = null, chapter = null;

                // Small heuristics based on siblings
                try
                {
                    var row = a.FindElement(By.XPath("./ancestor-or-self::*[self::tr or self::*[@role='row'] or self::div[contains(@class,'chapter')]][1]"));
                    var rowText = row.Text;

                    // Very light parsing heuristics (safe if structure varies)
                    // e.g., "[EN] GroupName • Vol.1 Ch.5: Title"
                    lang = FindMatch(rowText, @"\[(?<v>[A-Za-z\-]+)\]");
                    group = FindMatch(rowText, @"\]\s*(?<v>[^•\n\r]+)\s*•");
                    volume = FindMatch(rowText, @"Vol\.?\s*(?<v>[0-9]+)");
                    chapter = FindMatch(rowText, @"Ch\.?\s*(?<v>[0-9\.]+)");
                    if (string.IsNullOrWhiteSpace(title))
                        title = FindMatch(rowText, @"Ch\.?\s*[0-9\.]+:? (?<v>.+)$") ?? title;
                }
                catch { /* best-effort only */ }

                results.Add(new ChapterInfo
                {
                    Title = title ?? "",
                    Url = url,
                    Lang = lang,
                    Group = group,
                    Volume = volume,
                    Chapter = chapter
                });
            }
            catch { /* skip broken node */ }
        }

        // Optional: de-dup & sort by URL or chapter number if parsed
        results = results
            .GroupBy(x => x.Url)
            .Select(g => g.First())
            .OrderBy(x => x.Chapter, StringComparer.OrdinalIgnoreCase)
            .ToList();

        return results;
    }

    static string? FindMatch(string text, string regex)
    {
        var m = System.Text.RegularExpressions.Regex.Match(text, regex);
        if (m.Success)
        {
            var g = m.Groups["v"];
            if (g != null && g.Success)
                return g.Value.Trim();
        }
        return null;
    }

    static void WriteCsv(string path, List<ChapterInfo> items)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Title,Url,Lang,Group,Volume,Chapter");
        foreach (var c in items)
        {
            sb.AppendLine(string.Join(",",
                CsvEscape(c.Title),
                CsvEscape(c.Url),
                CsvEscape(c.Lang),
                CsvEscape(c.Group),
                CsvEscape(c.Volume),
                CsvEscape(c.Chapter)));
        }
        File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
    }

    static string CsvEscape(string? v)
    {
        v ??= "";
        if (v.Contains('"') || v.Contains(',') || v.Contains('\n') || v.Contains('\r'))
        {
            v = v.Replace("\"", "\"\"");
            return $"\"{v}\"";
        }
        return v;
    }
}
