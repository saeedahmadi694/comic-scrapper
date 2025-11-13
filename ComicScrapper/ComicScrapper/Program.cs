using ComicScrapper.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text.Json.Serialization;

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class MangaResponse
{
    [JsonProperty("result")]
    public string Result { get; set; }

    [JsonProperty("response")]
    public string Response { get; set; }

    [JsonProperty("data")]
    public MangaData Data { get; set; }
}

public class MangaData
{
    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("attributes")]
    public MangaAttributes Attributes { get; set; }

    [JsonProperty("relationships")]
    public List<MangaRelationship> Relationships { get; set; }
}

public class MangaAttributes
{
    // "title": { "en": "Shy" }
    [JsonProperty("title")]
    public Dictionary<string, string> Title { get; set; }

    // IMPORTANT: this is an ARRAY of OBJECTS, each object is { "langCode": "value" }
    // so we use List<Dictionary<string,string>>
    //
    // "altTitles": [
    //   { "ja": "SHY" },
    //   { "en": "SHY" },
    //   ...
    // ]
    [JsonProperty("altTitles")]
    public List<Dictionary<string, string>> AltTitles { get; set; }

    // "description": { "en": "...", "fr": "...", ... }
    [JsonProperty("description")]
    public Dictionary<string, string> Description { get; set; }

    [JsonProperty("isLocked")]
    public bool IsLocked { get; set; }

    // "links": { "al": "110909", "ap": "shy", ... }
    [JsonProperty("links")]
    public Dictionary<string, string> Links { get; set; }

    [JsonProperty("officialLinks")]
    public object OfficialLinks { get; set; }

    [JsonProperty("originalLanguage")]
    public string OriginalLanguage { get; set; }

    [JsonProperty("lastVolume")]
    public string LastVolume { get; set; }

    [JsonProperty("lastChapter")]
    public string LastChapter { get; set; }

    [JsonProperty("publicationDemographic")]
    public string PublicationDemographic { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("year")]
    public int? Year { get; set; }

    [JsonProperty("contentRating")]
    public string ContentRating { get; set; }

    [JsonProperty("tags")]
    public List<MangaTag> Tags { get; set; }

    [JsonProperty("state")]
    public string State { get; set; }

    [JsonProperty("chapterNumbersResetOnNewVolume")]
    public bool ChapterNumbersResetOnNewVolume { get; set; }

    [JsonProperty("createdAt")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("updatedAt")]
    public DateTime UpdatedAt { get; set; }

    [JsonProperty("version")]
    public int Version { get; set; }

    [JsonProperty("availableTranslatedLanguages")]
    public List<string> AvailableTranslatedLanguages { get; set; }

    [JsonProperty("latestUploadedChapter")]
    public string LatestUploadedChapter { get; set; }
}

public class MangaTag
{
    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("attributes")]
    public MangaTagAttributes Attributes { get; set; }

    [JsonProperty("relationships")]
    public List<object> Relationships { get; set; }
}

public class MangaTagAttributes
{
    // "name": { "en": "Sci-Fi" }
    [JsonProperty("name")]
    public Dictionary<string, string> Name { get; set; }

    // "description": {} (empty object)
    [JsonProperty("description")]
    public Dictionary<string, string> Description { get; set; }

    [JsonProperty("group")]
    public string Group { get; set; }

    [JsonProperty("version")]
    public int Version { get; set; }
}

public class MangaRelationship
{
    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    // Attributes shape depends on "type" (author / artist / cover_art),
    // so we make one combined class with nullable properties.
    [JsonProperty("attributes")]
    public RelationshipAttributes Attributes { get; set; }
}

public class RelationshipAttributes
{
    // For author / artist
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("imageUrl")]
    public string ImageUrl { get; set; }

    [JsonProperty("biography")]
    public Dictionary<string, string> Biography { get; set; }

    [JsonProperty("twitter")]
    public string Twitter { get; set; }

    [JsonProperty("pixiv")]
    public string Pixiv { get; set; }

    [JsonProperty("melonBook")]
    public string MelonBook { get; set; }

    [JsonProperty("fanBox")]
    public string FanBox { get; set; }

    [JsonProperty("booth")]
    public string Booth { get; set; }

    [JsonProperty("namicomi")]
    public string Namicomi { get; set; }

    [JsonProperty("nicoVideo")]
    public string NicoVideo { get; set; }

    [JsonProperty("skeb")]
    public string Skeb { get; set; }

    [JsonProperty("fantia")]
    public string Fantia { get; set; }

    [JsonProperty("tumblr")]
    public string Tumblr { get; set; }

    [JsonProperty("youtube")]
    public string Youtube { get; set; }

    [JsonProperty("weibo")]
    public string Weibo { get; set; }

    [JsonProperty("naver")]
    public string Naver { get; set; }

    [JsonProperty("website")]
    public string Website { get; set; }

    // For cover_art
    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("volume")]
    public string Volume { get; set; }

    [JsonProperty("fileName")]
    public string FileName { get; set; }

    [JsonProperty("locale")]
    public string Locale { get; set; }

    [JsonProperty("createdAt")]
    public DateTime? CreatedAt { get; set; }

    [JsonProperty("updatedAt")]
    public DateTime? UpdatedAt { get; set; }

    [JsonProperty("version")]
    public int? Version { get; set; }
}


public class ChapterCollectionResponse
{
    [JsonProperty("result")]
    public string Result { get; set; }

    [JsonProperty("response")]
    public string Response { get; set; }

    [JsonProperty("data")]
    public List<ChapterData> Data { get; set; }

    [JsonProperty("limit")]
    public int Limit { get; set; }

    [JsonProperty("offset")]
    public int Offset { get; set; }

    [JsonProperty("total")]
    public int Total { get; set; }
}

public class ChapterData
{
    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("attributes")]
    public ChapterAttributes Attributes { get; set; }

    [JsonProperty("relationships")]
    public List<ChapterRelationship> Relationships { get; set; }
}

public class ChapterAttributes
{
    [JsonProperty("volume")]
    public string Volume { get; set; }

    [JsonProperty("chapter")]
    public string Chapter { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("translatedLanguage")]
    public string TranslatedLanguage { get; set; }

    [JsonProperty("externalUrl")]
    public string ExternalUrl { get; set; }

    [JsonProperty("isUnavailable")]
    public bool IsUnavailable { get; set; }

    [JsonProperty("publishAt")]
    public DateTime PublishAt { get; set; }

    [JsonProperty("readableAt")]
    public DateTime ReadableAt { get; set; }

    [JsonProperty("createdAt")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("updatedAt")]
    public DateTime UpdatedAt { get; set; }

    [JsonProperty("version")]
    public int Version { get; set; }

    [JsonProperty("pages")]
    public int Pages { get; set; }
}

public class ChapterRelationship
{
    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }
}


class Program
{
    private const string TargetUrl = "4809bb6c-1af2-41c5-9291-983027602d7f";
    public static IHttpClientFactory _httpClientFactory = new DefaultHttpClientFactory();
    public static IHttpRequest _httpRequest = new HttpRequest(_httpClientFactory);
    static async Task Main()
    {
        var headers = new Dictionary<string, string>()
        {
            {"User-Agent","PostmanRuntime/7.50.0" }
        };
        Console.WriteLine($"scanning item ${TargetUrl}");
        var requestUrl = $"https://api.mangadex.org/manga/{TargetUrl}?includes[]=artist&includes[]=author&includes[]=cover_art";
        var responseDto = await _httpRequest.GetAsync<MangaResponse>(requestUrl, headers);
        Console.WriteLine("responseDto");

        Console.WriteLine($"scanning chapter of  ${responseDto.Data.Attributes.Title.Values}");

        var allChapters = new List<ChapterData>();
        int limit = 10;
        int offset = 0;

        while (true)
        {
            string chapterRequestUrl =
                $"https://api.mangadex.org/manga/{TargetUrl}/feed" +
                $"?limit={limit}" +
                $"&offset={offset}" +
                $"&includeUnavailable=0" +
                $"&excludeExternalUrl=blinktoon.com" +
                $"&translatedLanguage[]=en" +
                $"&translatedLanguage[]=fa";

            var pageResponse = await _httpRequest.GetAsync<ChapterCollectionResponse>(chapterRequestUrl, headers);

            if (pageResponse?.Data == null)
            {
                Console.WriteLine("Failed to fetch chapters.");
                break;
            }

            // Add page data
            allChapters.AddRange(pageResponse.Data);

            Console.WriteLine($"Fetched {pageResponse.Data.Count} chapters (offset {offset})");

            // Stop if all results are fetched
            offset += limit;
            if (offset >= pageResponse.Total)
                break;
        }

    }
}

public class DefaultHttpClientFactory : IHttpClientFactory
{
    public HttpClient CreateClient(string name = "")
    {
        var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromMinutes(5);
        return httpClient;
    }
}