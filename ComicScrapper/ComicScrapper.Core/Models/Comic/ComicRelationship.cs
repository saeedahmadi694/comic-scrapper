using Newtonsoft.Json;
using System;

namespace ComicScrapper.Core.Models.Comic
{
    public class ComicRelationship
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

}
