using Newtonsoft.Json;
using System.Collections.Generic;

namespace ComicScrapper.Core.Models.Episode
{
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

}
