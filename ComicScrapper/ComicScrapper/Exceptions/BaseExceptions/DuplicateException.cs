namespace ComicScrapper.Exceptions.BaseExceptions;

public class DuplicateException: BaseException
{
    public DuplicateException(string message) : base(message)
    {
    }

    public DuplicateException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
