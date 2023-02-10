using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WorksheetGenerator.Data
{
    [Table("Class")]
    public partial class Class
    {
        
        [Key]
        public int Id { get; set; }
        [StringLength(256)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        public int ClassLevel { get; set; }

    }
}
