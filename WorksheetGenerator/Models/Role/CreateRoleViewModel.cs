using Microsoft.Build.Framework;

namespace WorksheetGenerator.Models.Role
{
    public class CreateRoleViewModel
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

    }
}
