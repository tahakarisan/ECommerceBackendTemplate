namespace Core.Utilities.Results
{
    /// <summary>
    /// Hatalı işlemleri temsil eder.
    /// </summary>
    public class ErrorResult : Result
    {
        public ErrorResult(string message) : base(false, message)
        {
        }

        public ErrorResult() : base(false)
        {
        }
    }
}
