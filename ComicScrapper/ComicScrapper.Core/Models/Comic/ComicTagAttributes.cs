using Newtonsoft.Json;
using System.Collections.Generic;

namespace ComicScrapper.Core.Models.Comic
{
    public class ComicTagAttributes
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

}
