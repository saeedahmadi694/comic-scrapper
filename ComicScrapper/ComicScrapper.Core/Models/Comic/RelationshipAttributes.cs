using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ComicScrapper.Core.Models.Comic
{
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

}
