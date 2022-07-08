namespace EventPlanning.Models
{
    public class EventDetail
    {
        public int EventDetailId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int EventModelId { get; set; }
        public EventModel EventModel { get; set; }
    }
}
