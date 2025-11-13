using Newtonsoft.Json;

namespace ComicScrapper.Core.Models
{
    public class BaseResponse<T>
    {
        [JsonProperty("result")]
        public string Result { get; set; }

        [JsonProperty("response")]
        public string Response { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("offset")]
        public int Offset { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }
    }
}
