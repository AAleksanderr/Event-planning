using System;
using System.Collections.Generic;
using EventPlanning.Data;

namespace EventPlanning.Models
{
    public class EventModel
    {
        public int EventModelId { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public int MaxPersons { get; set; }
        public ICollection<EventDetail> EventDetails { get; set; }
        public ICollection<EventUserRegistration> RegisteredUsers { get; set; }
    }
}
