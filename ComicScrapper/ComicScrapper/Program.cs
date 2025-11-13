using ComicScrapper.Core.Services;

class Program
{

    static async Task Main()
    {

        var scrapper = new MangadexService();
        var detrail = await scrapper.GetComicDataAsync("4809bb6c-1af2-41c5-9291-983027602d7f", new CancellationToken());
        var chapters = await scrapper.GetComicChaptersAsync("4809bb6c-1af2-41c5-9291-983027602d7f", new CancellationToken());
        Console.WriteLine("ss");



    }
}
