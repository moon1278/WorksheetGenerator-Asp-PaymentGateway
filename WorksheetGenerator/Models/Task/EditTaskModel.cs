using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using WorksheetGenerator.Data;

namespace WorksheetGenerator.Models.Task
{
    public class EditTaskModel
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

        public byte[] PreviewImage { get; set; }

        public List<HTML_SpecificationType> HTML_SpecificationTypes { get; set; }

        public List<Specifictions_Option> Specifications { get; set; }

        #region SelectList SubTaskType

        [Display(Name = "SubTaskType")]
        public IList<SelectListItem>? SubTaskTypes { get; set; }
        [Required(ErrorMessage = "Bitte wähle eine SubTaskType aus.")]
        public int? SelectedSubTaskType { get; set; }

        #endregion


    }
}
