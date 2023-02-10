using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WorksheetGenerator.Data
{
    [Table("Token")]
    public class Token
    {
        [Key]
        public int Id { get; set; }
        public float CreditAmount { get; set; }
        public float PayAmount { get; set; }
        public string OrderId { get; set; } = "";
        public string PaymentId { get; set; } = "";
        public string PaymentToken { get; set; } = "";
        public string UserId { get; set; } = "";
        public DateTime CreateAt { get; set; }
    }
}
