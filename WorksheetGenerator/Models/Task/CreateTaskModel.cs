using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WorksheetGenerator.Models.Task
{
    public class CreateTaskModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string? Description { get; set; }
        [Required]
        public string? Query { get; set; }
        [Required]
        public bool Activated { get; set; }

        #region SelectList SubTaskType

        [Display(Name = "SubTaskType")]
        public IList<SelectListItem>? SubTaskTypes { get; set; }
        [Required(ErrorMessage = "Bitte wähle eine SubTaskType aus.")]
        public int? SelectedSubTaskType { get; set; }

        #endregion


    }
}
