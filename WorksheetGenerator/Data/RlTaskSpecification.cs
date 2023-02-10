using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WorksheetGenerator.Data
{
    [Table("RL_TaskSpecification")]
    public partial class RlTaskSpecification
    {
        [Key]
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int SpecificationId { get; set; }
    }
}
