using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ComicScrapper.Core.Models.Episode
{
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

}
