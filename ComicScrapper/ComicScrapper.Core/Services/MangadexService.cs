using ComicScrapper.Core.Models;
using ComicScrapper.Core.Models.Comic;
using ComicScrapper.Core.Models.Episode;
using ComicScrapper.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace ComicScrapper.Core.Services
{

    public class MangadexService : IMangadexService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _baseUrl = "https://api.mangadex.org";
        private readonly IHttpRequest _httpRequest;
        public MangadexService()
        {
            _httpClientFactory = new DefaultHttpClientFactory();
            _httpRequest = new HttpRequest(_httpClientFactory);
        }

        public async Task<List<ChapterData>> GetComicChaptersAsync(string id, CancellationToken cancellationToken)
        {
            var headers = new Dictionary<string, string>()
            {
                {"User-Agent","PostmanRuntime/7.50.0" }
            };

            var allChapters = new List<ChapterData>();
            int limit = 100;
            int offset = 0;

            while (true)
            {
                string chapterRequestUrl =
                    $"https://api.mangadex.org/manga/{id}/feed" +
                    $"?limit={limit}" +
                    $"&offset={offset}" +
                    $"&includeUnavailable=0" +
                    $"&excludeExternalUrl=blinktoon.com" +
                    $"&translatedLanguage[]=en" +
                    $"&translatedLanguage[]=fa";

                var pageResponse = await _httpRequest.GetAsync<ChapterCollectionResponse>(chapterRequestUrl, headers);

                if (pageResponse?.Data == null)
                {
                    Console.WriteLine("Failed to fetch chapters.");
                    break;
                }

                allChapters.AddRange(pageResponse.Data);

                Console.WriteLine($"Fetched {pageResponse.Data.Count} chapters (offset {offset})");

                offset += limit;
                if (offset >= pageResponse.Total)
                    break;
            }
            return allChapters;
        }

        public async Task<ComicData> GetComicDataAsync(string id, CancellationToken cancellationToken)
        {
            var headers = new Dictionary<string, string>()
            {
                {"User-Agent","PostmanRuntime/7.50.0" }
            };
            Console.WriteLine($"scanning item ${id}");

            var requestUrl = $"{_baseUrl}/manga/{id}?includes[]=artist&includes[]=author&includes[]=cover_art";
            var responseDto = await _httpRequest.GetAsync<BaseResponse<ComicData>>(requestUrl, headers);
            return responseDto.Data;
        }
    }
    public class DefaultHttpClientFactory : IHttpClientFactory
    {
        public HttpClient CreateClient(string name = "")
        {
            var httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            return httpClient;
        }
    }
}
