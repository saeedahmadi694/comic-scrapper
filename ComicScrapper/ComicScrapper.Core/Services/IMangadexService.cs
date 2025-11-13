using ComicScrapper.Core.Models.Comic;
using ComicScrapper.Core.Models.Episode;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ComicScrapper.Core.Services
{
    public interface IArcaptchaService
    {
        Task<ComicData> GetComicDataAsync(string id, CancellationToken cancellationToken);
        Task<List<ChapterData>> GetComicChaptersAsync(string id, CancellationToken cancellationToken);
    }
}
