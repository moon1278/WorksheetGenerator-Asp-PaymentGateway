using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorksheetGenerator.Data
{
    [Table("HTML_SpecificationType")]
    public partial class HTML_SpecificationType
    {
     
        [Key]
        public int Id { get; set; }
        [StringLength(256)]
        [Unicode(false)]
        public string Type { get; set; } = null!;

    }
}
