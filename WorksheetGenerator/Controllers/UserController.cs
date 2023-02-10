using DocumentFormat.OpenXml.Office2010.Drawing.Charts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WorksheetGenerator.Data;
using WorksheetGenerator.Models.Account;
using WorksheetGenerator.Models.User;

namespace WorksheetGenerator.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;

        }

        public IActionResult Index()
        {
            List<UserViewModel> users = new List<UserViewModel>();

            foreach(var user in _userManager.Users)
            {
                UserViewModel u = new UserViewModel();
                u.LockoutEnabled = user.LockoutEnabled;
                u.LockoutEnd = user.LockoutEnd;
                u.NormalizedEmail = user.NormalizedEmail;
                u.TwoFactorEnabled = user.TwoFactorEnabled;
                u.AccessFailedCount = user.AccessFailedCount;
                u.EmailConfirmed = user.EmailConfirmed;
                u.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
                u.PhoneNumber = user.PhoneNumber;
                u.ConcurrencyStamp = user.ConcurrencyStamp;
                u.Id = user.Id;
                u.SecurityStamp = user.SecurityStamp;
                u.UserName = user.UserName;
                u.Email = user.Email;

                users.Add(u);
            }

            return View(users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    //await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "User");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                ModelState.AddModelError(string.Empty, "User Erstellung fehlgeschlagen");

            }
            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
   
            
            UserViewModel u = new UserViewModel();
            u.LockoutEnabled = user.LockoutEnabled;
            u.LockoutEnd = user.LockoutEnd;
            u.NormalizedEmail = user.NormalizedEmail;
            u.TwoFactorEnabled = user.TwoFactorEnabled;
            u.AccessFailedCount = user.AccessFailedCount;
            u.EmailConfirmed = user.EmailConfirmed;
            u.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
            u.PhoneNumber = user.PhoneNumber;
            u.ConcurrencyStamp = user.ConcurrencyStamp;
            u.Id = user.Id;
            u.SecurityStamp = user.SecurityStamp;
            u.UserName = user.UserName;
            u.Email = user.Email;

            
            return View(u);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
           
            if (id == null)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByIdAsync(id);
            var logins = await _userManager.GetLoginsAsync(user);
            var rolesForUser = await _userManager.GetRolesAsync(user);

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                foreach (var login in logins)
                {
                    await _userManager.RemoveLoginAsync(user, login.LoginProvider, login.ProviderKey);
                }

                if (rolesForUser.Count() > 0)
                {
                    foreach (var role in rolesForUser.ToList())
                    {
                        // item should be the name of the role
                        var result = await _userManager.RemoveFromRoleAsync(user, role);
                    }
                }

                await _userManager.DeleteAsync(user);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            UserDetailsViewModel u = new UserDetailsViewModel();
            u.LockoutEnabled = user.LockoutEnabled;
            u.LockoutEnd = user.LockoutEnd;
            u.NormalizedEmail = user.NormalizedEmail;
            u.TwoFactorEnabled = user.TwoFactorEnabled;
            u.AccessFailedCount = user.AccessFailedCount;
            u.EmailConfirmed = user.EmailConfirmed;
            u.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
            u.PhoneNumber = user.PhoneNumber;
            u.ConcurrencyStamp = user.ConcurrencyStamp;
            u.Id = user.Id;
            u.SecurityStamp = user.SecurityStamp;
            u.UserName = user.UserName;
            u.Email = user.Email;


            var userRoles = await _userManager.GetRolesAsync(user);
            List<IdentityRole> roles = new List<IdentityRole>();

            foreach (var role in userRoles)
            {
                IdentityRole r = _roleManager.Roles.Where(x => x.Name == role).FirstOrDefault();

                if(r != null)
                {
                    roles.Add(r);
                }
            }

            u.Roles = roles;

            return View(u);
        }


    }
}
