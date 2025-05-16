namespace Core.Utilities.Results
{
    /// <summary>
    /// Veri dönen işlemler için kullanılan genişletilmiş bir arayüz.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDataResult<T> : IResult
    {
        T Data { get; }
    }
}
