using Newtonsoft.Json;
using System;

namespace ComicScrapper.Core.Models.Episode
{
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

}
