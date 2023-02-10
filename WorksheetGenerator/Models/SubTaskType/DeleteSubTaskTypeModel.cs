using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using WorksheetGenerator.Models.SubTaskType.Helper_DetailsSubTaskTypeModel;

namespace WorksheetGenerator.Models.SubTaskType
{
    public class DeleteSubTaskTypeModel
    {

        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? TaskType { get; set; }

        public List<WorksheetGenerator.Data.Task> Tasks { get; set; }

    }
}
