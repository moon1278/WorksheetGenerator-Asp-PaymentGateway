using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WorksheetGenerator.Models.WorksheetManagement
{
    public class EditWorksheetManagementModel
    {

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime Created { get; set; }
        public string Created_User { get; set; }

        
    }
}
