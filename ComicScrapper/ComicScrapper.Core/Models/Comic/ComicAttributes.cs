using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ComicScrapper.Core.Models.Comic
{
    public class ComicAttributes
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
        public List<ComicTag> Tags { get; set; }

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

}
