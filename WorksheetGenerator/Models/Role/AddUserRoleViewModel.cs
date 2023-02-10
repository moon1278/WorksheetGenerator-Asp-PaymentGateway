using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;

namespace WorksheetGenerator.Models.Role
{
    public class AddUserToRoleViewModel
    {

        public string RoleId { get; set; }

        public string RoleName { get; set; }

        public List<IdentityUser> UsersNotInRole { get; set; }


    }
}
