namespace Core.Utilities.Results
{
    /// <summary>
    /// Temel bir arayüz, işlem sonucunun başarılı olup olmadığını ve mesajını tanımlar.
    /// </summary>
    public interface IResult
    {
        bool Success { get; }
        string Message { get; }
    }
}
