namespace EventPlanning.Models
{
    public class EventUserRegistration
    {
        public int EventUserRegistrationId { get; set; }
        public string RegisteredUserName { get; set; }
        public int EventModelId { get; set; }
        public EventModel EventModel { get; set; }
    }
}
