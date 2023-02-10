using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WorksheetGenerator.Data
{
    [Table("RL_WorksheetTask")]
    public partial class RlWorksheetTask
    {
        [Key]
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int WorksheetId { get; set; }
        [Unicode(false)]
        public string Result { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime Created { get; set; }

       
    }
}
