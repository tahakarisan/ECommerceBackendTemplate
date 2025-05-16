using Serilog;
using Serilog.Events;

namespace Core.CrossCuttingConcerns.Logging.SeriLog
{
    public class LoggerServiceBase
    {
        private readonly ILogger _log;

        // Serilog yapılandırması için Constructor
        public LoggerServiceBase(string name)
        {
            _log = new LoggerConfiguration()
                .WriteTo.Console()  // Konsola log yazma
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)  // Dosyaya log yazma
                .CreateLogger();
        }

        // Log seviyeleri için özellikler
        public bool IsInfoEnabled => _log.IsEnabled(LogEventLevel.Information);
        public bool IsDebugEnabled => _log.IsEnabled(LogEventLevel.Debug);
        public bool IsWarnEnabled => _log.IsEnabled(LogEventLevel.Warning);
        public bool IsFatalEnabled => _log.IsEnabled(LogEventLevel.Fatal);
        public bool IsErrorEnabled => _log.IsEnabled(LogEventLevel.Error);

        // Log metotları
        public void Info(object logMessage)
        {
            if (IsInfoEnabled)
                _log.Information(logMessage.ToString());
        }

        public void Debug(object logMessage)
        {
            if (IsDebugEnabled)
                _log.Debug(logMessage.ToString());
        }

        public void Warn(object logMessage)
        {
            if (IsWarnEnabled)
                _log.Warning(logMessage.ToString());
        }

        public void Fatal(object logMessage)
        {
            if (IsFatalEnabled)
                _log.Fatal(logMessage.ToString());
        }

        public void Error(object logMessage)
        {
            if (IsErrorEnabled)
                _log.Error(logMessage.ToString());
        }
    }
}
