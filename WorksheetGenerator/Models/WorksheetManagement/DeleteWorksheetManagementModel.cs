using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using WorksheetGenerator.Models.SubTaskType.Helper_DetailsSubTaskTypeModel;

namespace WorksheetGenerator.Models.WorksheetManagement
{
    public class DeleteWorksheetManagementModel
    {

        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string SubTaskType { get; set; }
        public string? Query { get; set; }
        public bool Activated { get; set; }



    }
}
