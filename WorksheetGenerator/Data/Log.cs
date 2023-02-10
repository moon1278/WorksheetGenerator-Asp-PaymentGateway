using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorksheetGenerator.Data
{
    [Table("Log")]
    public class Log
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string Application { get; set; } = null!;
        public DateTime Logged { get; set; } 
        [StringLength(50)]
        public string Level { get; set; } = null!;
        [StringLength(4000)]
        public string Message { get; set; } = null!;
        [StringLength(250)]
        public string Logger { get; set; } = null!;
        [StringLength(4000)]
        public string Callsite { get; set; } = null!;
        [StringLength(2000)]
        public string Exception { get; set; } = null!;
       

    }
}
