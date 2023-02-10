using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WorksheetGenerator.Data
{
    [Table("Specification")]
    public partial class Specification
    {
       
        [Key]
        public int Id { get; set; }

        [StringLength(256)]
        [Unicode(false)]
        public string Name { get; set; } = null!;

        public int TaskId { get; set; }

        [StringLength(256)]
        [Unicode(false)]
        public string? Description { get; set; }

        public int HTML_SpecificationTypeId { get; set; }
        public string? Dynamic_Replace_Text { get; set; }
        public int Position { get; set; }
   
        
    }
}
