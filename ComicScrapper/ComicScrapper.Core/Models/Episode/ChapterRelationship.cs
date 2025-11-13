using Newtonsoft.Json;
using System;
using System.Text;

namespace ComicScrapper.Core.Models.Episode
{

    public class ChapterRelationship
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

}
