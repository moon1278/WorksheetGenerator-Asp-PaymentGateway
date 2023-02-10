using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace WorksheetGenerator.Data
{
    [Table("Task")]
    public partial class Task
    {
       

        [Key]
        public int Id { get; set; }
        [StringLength(256)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        [Column(TypeName = "text")]
        public string? Description { get; set; }
        public int? SubTaskTypeId { get; set; }
        [Unicode(false)]
        public string? Query { get; set; }
        public bool Activated { get; set; }
        public byte[] PreviewImage { get; set; }


        [ForeignKey("SubTaskTypeId")]
        [InverseProperty("Tasks")]
        [JsonIgnore]
        public virtual SubTaskType? SubTaskType { get; set; }
 
   
    }
}
