using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WorksheetGenerator.Models.WorksheetModels
{
    public class CreateWorksheetViewModel_General
    {
        [Required(ErrorMessage = "Bitte definieren einen Namen für das Arbeitsblatt.")]
        [Display(Name = "Arbeitsblatt Name")] 
        public string WorksheetName { get; set; }

        #region SelectList Class

        [Display(Name = "Klasse")]
        public IList<SelectListItem>? Classes { get; set; }
        [Required(ErrorMessage = "Bitte wähle eine Schulklasse.")]
        public int? SelectedClass { get; set; }

        #endregion

        #region SelectList Subject

        [Display(Name = "Schulfach")]
        public IList<SelectListItem>? Subjects { get; set; }
        [Required(ErrorMessage = "Bitte wähle einen Schulfach.")]
        public int? SelectedSubject { get; set; }

        #endregion

    }
}
