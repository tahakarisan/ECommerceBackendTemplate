namespace Core.Utilities.Results
{
    /// <summary>
    /// Başarılı işlemleri temsil eder.
    /// </summary>
    public class SuccessResult : Result
    {
        public SuccessResult(string message) : base(true, message)
        {
        }

        public SuccessResult() : base(true)
        {
        }
    }
}
