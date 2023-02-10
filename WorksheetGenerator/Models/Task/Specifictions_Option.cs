using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace WorksheetGenerator.Models.Task
{
    public class Specifictions_Option
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public int TaskId { get; set; }
        public string? Description { get; set; }
        public int HTML_SpecificationTypeId { get; set; }
        public string? Dynamic_Replace_Text { get; set; }
        public int Selected_HTML_SpecificationTypeId { get; set; }
        public int Position { get; set; }

    }
}
