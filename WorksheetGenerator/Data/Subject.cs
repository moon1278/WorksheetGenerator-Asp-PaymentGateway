using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WorksheetGenerator.Data
{
    [Table("Subject")]
    public partial class Subject
    {
        public Subject()
        {
            TaskTypes = new HashSet<TaskType>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(256)]
        [Unicode(false)]
        public string Name { get; set; } = null!;

        [InverseProperty("Subject")]
        public virtual ICollection<TaskType> TaskTypes { get; set; }
    }
}
