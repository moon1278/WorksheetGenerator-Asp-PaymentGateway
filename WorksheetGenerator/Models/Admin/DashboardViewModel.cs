using Microsoft.AspNetCore.Identity;
using WorksheetGenerator.Data;

namespace WorksheetGenerator.Models.Admin
{
    public class DashboardViewModel
    {
        public int TaskCount { get; set; }
        public int WorksheetCount { get; set; }
        public int UserCount { get; set; }
        public List<Worksheet> Worksheets { get; set; }
        public List<IdentityUser> Users { get; set; }

    }
}
