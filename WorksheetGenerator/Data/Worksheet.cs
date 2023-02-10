using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WorksheetGenerator.Data
{
    [Table("Worksheet")]
    public partial class Worksheet
    {
      

        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        [StringLength(450)]
        public string UserId { get; set; } = null!;

        public DateTime Created { get; set; }

     
    }
}
