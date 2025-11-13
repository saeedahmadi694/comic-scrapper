using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ComicScrapper.Core.Models.Comic
{
    public class ComicTag
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("attributes")]
        public ComicTagAttributes Attributes { get; set; }

        [JsonProperty("relationships")]
        public List<object> Relationships { get; set; }
    }

}
