using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WorksheetGenerator.Models.Token
{
    public class TokenViewModel
    {
        public int Id { get; set; }
        public float CreditAmount { get; set; }
        public float PayAmount { get; set; }
        public string OrderId { get; set; } = "";
        public string PaymentId { get; set; } = "";
        public string PaymentToken { get; set; } = "";
        public string UserId { get; set; } = "";
        public DateTime CreateAt { get; set; }

        public List<TokenViewModel> Tokens { get; set; } = null!;
    }
}
