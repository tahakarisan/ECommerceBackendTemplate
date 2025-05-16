using Serilog.Events;

namespace Core.CrossCuttingConcerns.Logging.SeriLog
{
    [Serializable]
    public class SerializableLogEvent
    {
        private LogEvent _logEvent;

        public SerializableLogEvent(LogEvent logEvent)
        {
            _logEvent = logEvent;
        }

        public string Message => _logEvent.MessageTemplate.Text;
        public DateTime Timestamp => _logEvent.Timestamp.DateTime;
        public string Level => _logEvent.Level.ToString();
        public string Exception => _logEvent.Exception?.ToString();
        public IReadOnlyDictionary<string, LogEventPropertyValue> Properties => _logEvent.Properties;
    }
}
