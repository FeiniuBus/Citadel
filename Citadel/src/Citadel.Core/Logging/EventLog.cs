namespace Citadel.Logging
{
    public sealed class EventLog
    {
        public EventLog() { }
        public EventLog(string source, string target, string eventName, object infomations)
        {
            Source = source;
            Target = target;
            Event = eventName;
            Infomations = infomations;
        }
        public string ApplciationName = "Citadel";
        public string Source { get; set; }
        public string Target { get; set; }
        public string Event { get; set; }
        public object Infomations { get; set; }
    }
}
