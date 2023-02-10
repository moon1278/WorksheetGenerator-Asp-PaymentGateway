using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using WorksheetGenerator.Models.SubTaskType.Helper_DetailsSubTaskTypeModel;

namespace WorksheetGenerator.Models.TaskType
{
    public class DeleteTaskTypeModel
    {

        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? HexColor { get; set; }
        public string Subject { get; set; }

        public List<WorksheetGenerator.Data.SubTaskType> SubTaskTypes { get; set; }
        public List<WorksheetGenerator.Data.Task> Tasks { get; set; }


    }
}
