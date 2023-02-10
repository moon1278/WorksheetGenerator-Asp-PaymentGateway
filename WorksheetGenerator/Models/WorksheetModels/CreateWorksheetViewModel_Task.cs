using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WorksheetGenerator.Models.WorksheetModels
{
    public class CreateWorksheetViewModel_Task
    {

        public string WorksheetName { get; set; }
        public int ClassId { get; set; }
        
        #region SelectList TaskTypes

        [Display(Name = "Aufgaben Typ")]
        public IList<SelectListItem>? TaskTypes { get; set; }
        [Required(ErrorMessage = "Bitte wähle einen Aufgaben Typ.")]
        public int? SelectedTaskType { get; set; }

        #endregion

        #region SelectList Task

        [Display(Name = "Sub Aufgaben Typ")]
        public IEnumerable<SelectListItem>? SubTaskTypes { get; set; }
        [Required(ErrorMessage = "Bitte wähle eine Sub Aufgaben Typ.")]
        public IEnumerable<string> SelectedSubTaskTypes { get; set; }

        #endregion
    }
}
