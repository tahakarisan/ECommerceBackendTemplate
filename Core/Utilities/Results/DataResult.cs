namespace Core.Utilities.Results
{
    public class DataResult<T> : Result, IDataResult<T>
    {
        public T Data { get; }

        /// <summary>
        /// IDataResult'ın temel implementasyonu.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="success"></param>
        /// <param name="message"></param>
        public DataResult(T data, bool success, string message) : base(success, message)
        {
            Data = data;
        }

        public DataResult(T data, bool success) : base(success)
        {
            Data = data;
        }
    }
}
