using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WorksheetGenerator.Data
{
    [Table("TaskType")]
    public partial class TaskType
    {
        public TaskType()
        {
            SubTaskTypes = new HashSet<SubTaskType>();
        }

        [Key]
        public int Id { get; set; }
        public int SubjectId { get; set; }
        [StringLength(256)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        [StringLength(256)]
        [Unicode(false)]
        public string? Description { get; set; }
        [StringLength(12)]
        [Unicode(false)]
        public string HexColor { get; set; } = null!;

        [ForeignKey("SubjectId")]
        [InverseProperty("TaskTypes")]
        public virtual Subject Subject { get; set; } = null!;
        [InverseProperty("TaskType")]
        public virtual ICollection<SubTaskType> SubTaskTypes { get; set; }
    }
}
