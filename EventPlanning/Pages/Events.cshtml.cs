using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EventPlanning.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EventPlanning.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EventPlanning.Pages
{
    [Authorize]
    public class EventsModel : PageModel
    {
        private readonly EventPlanning.Data.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EventsModel(EventPlanning.Data.ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        public IList<EventModel> EventModel { get;set; }
        public bool IsAdmin { get; set; }

        [BindProperty]
        public InputEventModel Input { get; set; }

        [BindProperty]
        public List<InputEventDetail> Details { get; set; }
        public InputEventModel InputEvent { get; set; }

        public class InputEventModel
        {
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
            [Display(Name = "Event name")]
            public string EventName { get; set; } = "New event name";

            [Required]
            [DataType(DataType.DateTime)]
            [DisplayFormat(DataFormatString = "yyyy-MM-ddTHH:mm")]
            [Display(Name = "Event date")]
            public DateTime EventDate { get; set; } = DateTime.Now;

            [Display(Name = "Maximum persons")]
            public int MaxPersons { get; set; } = 100;
        }

        public class InputEventDetail
        {
            [Display(Name = "Description name")]
            public string Name { get; set; }

            [Display(Name = "Description")]
            public string Description { get; set; }
        }

        public async Task OnGetAsync()
        {
            EventModel = await _context.EventModels
                                       .Include(e => e.EventDetails)
                                       .Include(e => e.RegisteredUsers)
                                       .ToListAsync();

            var currentUser = await _userManager.GetUserAsync(User);
            IsAdmin = currentUser.IsAdmin;
            Details ??= new List<InputEventDetail>();
            Input ??= new InputEventModel();
        }

        public async Task OnPostAddEvent(string description, string addEvent, string deleteDescription)
        {
            if (addEvent != null)
            {
                var newEvent = new EventModel()
                {
                    EventName = Input.EventName,
                    EventDate = Input.EventDate,
                    MaxPersons = Input.MaxPersons
                };

                await _context.EventModels.AddAsync(newEvent);
                var newEventDetails = Details.Select(detail => new EventDetail()
                {
                    Name = detail.Name,
                    Description = detail.Description,
                    EventDetailId = newEvent.EventModelId,
                    EventModel = newEvent
                }).ToList();
                newEvent.EventDetails = newEventDetails;
                await _context.SaveChangesAsync();
            }
            else if (description != null)
            {
                Details ??= new List<InputEventDetail>();
                Details.Add(new InputEventDetail());
            }
            else if (deleteDescription != null)
            {
                if (Details.Count > 0) Details.RemoveAt(Details.Count - 1);
            }
            await OnGetAsync();
        }
        public async Task OnPostAddDescription()
        {
            Details ??= new List<InputEventDetail>();

            Details.Add(new InputEventDetail());
            Details.Add(new InputEventDetail());
            await OnGetAsync();
        }

        public async Task OnPostSignUp(int eventModelId)
        {
            try
            {
                var selectedEvent = _context.EventModels
                                            .Include(e => e.RegisteredUsers)
                                            .FirstOrDefault(e => e.EventModelId == eventModelId);
                if (selectedEvent != null && User != null)
                {
                    selectedEvent.RegisteredUsers.Add(new EventUserRegistration()
                                                      {
                                                          EventModelId = eventModelId,
                                                          EventModel = selectedEvent,
                                                          RegisteredUserName = User.Identity.Name
                                                      });
                }
                await _context.SaveChangesAsync();
                await OnGetAsync();
            }
            catch
            {
                throw new Exception("Can't sign in.");
            }
        }
        public async Task OnPostSignOut(int eventModelId)
        {
            try
            {
                var selectedEvent = _context.EventModels
                                            .Include(e => e.RegisteredUsers)
                                            .FirstOrDefault(e => e.EventModelId == eventModelId);
                if (selectedEvent != null && User != null)
                {
                    var person =
                        selectedEvent.RegisteredUsers.FirstOrDefault(p => p.RegisteredUserName == User.Identity.Name);
                    selectedEvent.RegisteredUsers.Remove(person);
                }

                await _context.SaveChangesAsync();
                await OnGetAsync();
            }
            catch
            {
                throw new Exception("Can't sign out.");
            }
        }
    }
}
