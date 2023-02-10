using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WorksheetGenerator.Data
{
    [Table("RL_WorksheetClass")]
    public partial class RlWorksheetClass
    {
        [Key]
        public int Id { get; set; }
        public int ClassId { get; set; }
        public int WorksheetId { get; set; }

       
      
    }
}
