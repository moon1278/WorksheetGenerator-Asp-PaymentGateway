using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WorksheetGenerator.Models.TaskType    
{
    public class CreateTaskTypeModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string? Description { get; set; }

        [RegularExpression("([a-zA-Z0-9 .&'-]+)", ErrorMessage = "Nur Buchstaben und Zahlen erlaubt.")]
        [Required]
        public string? HexColor { get; set; }

        #region SelectList Subject

        [Display(Name = "Fach")]
        public IList<SelectListItem>? Subjects { get; set; }
        [Required(ErrorMessage = "Bitte wähle eine TaskType aus.")]
        public int? SelectedSubject { get; set; }

        #endregion


    }
}
