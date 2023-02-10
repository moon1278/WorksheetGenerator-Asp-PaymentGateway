using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace WorksheetGenerator.Data
{
    [Table("SubTaskType")]
    public partial class SubTaskType
    {
        //public SubTaskType()
        //{
        //    Tasks = new HashSet<Task>();
        //}

        [Key]
        public int Id { get; set; }
        [StringLength(256)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        public int TaskTypeId { get; set; }
        [Unicode(false)]
        public string? Description { get; set; }

        [ForeignKey("TaskTypeId")]
        [JsonIgnore]
        [InverseProperty("SubTaskTypes")]
        public virtual TaskType TaskType { get; set; } = null!;
        [JsonIgnore]
        [InverseProperty("SubTaskType")]
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
