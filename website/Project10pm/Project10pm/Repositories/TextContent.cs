namespace Project10pm.Repositories
{
    public class TextContent
    {
        public string RawText { get; set; } = string.Empty;

        public List<Event> Events { get; set; } = new List<Event>();
    }

    public class Event
    {
        public DateTimeOffset? EventDateTimeOffset { get; set; }
        public string EventName { get; set; } = string.Empty;
        public string EventDescription { get; set; } = string.Empty;
    }
}
