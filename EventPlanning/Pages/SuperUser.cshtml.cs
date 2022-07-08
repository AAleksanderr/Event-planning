using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EventPlanning.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EventPlanning.Pages
{
    public class SuperUserModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        [BindProperty]
        public List<EditedUser> EditedUsers { get; set; }
        public class EditedUser
        {
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Display(Name = "Email confirmed")]
            public bool EmailConfirmed { get; set; }

            [Display(Name = "Is admin")]
            public bool IsAdmin { get; set; }
        }

        public SuperUserModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task OnGetAsync()
        {
            var appUsers = await _userManager.Users.ToListAsync();
            EditedUsers = appUsers
                          .Select(u => new EditedUser()
                                       {
                                           Name = u.UserName, EmailConfirmed = u.EmailConfirmed, IsAdmin = u.IsAdmin
                                       })
                          .ToList();
        }
        public async Task OnPostAsync()
        {
            foreach (var editedUser in EditedUsers)
            {
                var appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == editedUser.Name);
                if (appUser != null)
                {
                    appUser.EmailConfirmed = editedUser.EmailConfirmed;
                    appUser.IsAdmin = editedUser.IsAdmin;
                    await _userManager.UpdateAsync(appUser);
                }
            }
        }
    }
}
