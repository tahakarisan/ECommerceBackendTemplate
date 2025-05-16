namespace Core.CrossCuttingConcerns.Logging.SeriLog.Loggers
{
    public class FileLogger : LoggerServiceBase
    {
        public FileLogger() : base("JsonFileLogger")
        {
        }
    }
}
