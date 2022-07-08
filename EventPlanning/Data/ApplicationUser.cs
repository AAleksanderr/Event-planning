using System.ComponentModel.DataAnnotations.Schema;
using EventPlanning.Models;
using Microsoft.AspNetCore.Identity;

namespace EventPlanning.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string NickName { get; set; }
        public string City { get; set; }
        public bool IsAdmin { get; set; }
    }
}
