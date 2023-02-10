using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WorksheetGenerator.Data
{
    [Table("Option")]
    public partial class Option
    {
        [Key]
        public int Id { get; set; }
        public int SpecificationId { get; set; }
        [StringLength(256)]
        [Unicode(false)]
        public string Name { get; set; } = null!;

      
    }
}
