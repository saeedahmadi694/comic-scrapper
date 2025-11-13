using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ComicScrapper.Core.Models.Comic
{
    public class ComicData
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("attributes")]
        public ComicAttributes Attributes { get; set; }

        [JsonProperty("relationships")]
        public List<ComicRelationship> Relationships { get; set; }
    }

}
