using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WorksheetGenerator.Models.TaskType
{
    public class EditTaskTypeModel
    {

        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string? Description { get; set; }

        [RegularExpression("([a-zA-Z0-9 .&'-]+)", ErrorMessage = "Nur Buchstaben und Zahlen erlaubt.")]
        [Required]
        public string? HexColor { get; set; }

        #region SelectList TaskTypeId

        [Display(Name = "TaskTypeId")]
        public IList<SelectListItem>? TaskTypes { get; set; }
        [Required(ErrorMessage = "Bitte wähle eine TaskType aus.")]
        public int? SelectedTaskType { get; set; }

        #endregion

    }
}
