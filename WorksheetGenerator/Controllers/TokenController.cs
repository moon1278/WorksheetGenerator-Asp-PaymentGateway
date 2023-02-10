using Microsoft.AspNetCore.Mvc;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc.Rendering;
using WorksheetGenerator.Data;
using WorksheetGenerator.Helper;
using WorksheetGenerator.Models.WorksheetModels;
using WorksheetGenerator.Models.WorksheetModels.TaskModels;
using System.Web;
using WorksheetGenerator.Helper.Task;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using WorksheetGenerator.Helper.Logging;
using System.Dynamic;
using WorksheetGenerator.Helper.Word;
using WorksheetGenerator.Models.SpecificationModels;
using WorksheetGenerator.Helper.Worksheet;
using WorksheetGenerator.Helper.Specification;
using WorksheetGenerator.Models.WorksheetModels.TaskModels.General;
using Microsoft.AspNetCore.Identity;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WorksheetGenerator.Models.Task;
using System.Text;
using WorksheetGenerator.Models.Token;
using WorksheetGenerator.Models.Account;
using WorksheetGenerator.Models.Task.Helper_DetailsTaskModel;

namespace WorksheetGenerator.Controllers
{
    public class TokenController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        public TokenController(ApplicationDbContext context)
        {
            _context = context;
        }

        //public IActionResult Index()
        //{

        //    var tokens = _context.Tokens.ToList();
        //    return View(tokens);

        //}

        [Authorize(Roles = "Administrator")]
        public IActionResult List()
        {
            List<TokenViewModel> tokens = new List<TokenViewModel>();
            var query = from token in _context.Tokens
                        select token;

            var tokentemp = query.ToList();
            foreach (var data in tokentemp)
            {
                tokens.Add(new TokenViewModel()
                {
                    Id = data.Id,
                    UserId = data.UserId,
                    CreditAmount = data.CreditAmount,
                    OrderId = data.OrderId,
                    PaymentId = data.PaymentId,
                    PaymentToken = data.PaymentToken,
                    PayAmount = data.PayAmount
                });
            }

            return View(tokens);
        }

        public IActionResult User()
        {
            List<TokenViewModel> tokens = new List<TokenViewModel>();
            var query = from token in _context.Tokens
                        select token;

            var tokentemp = query.ToList();

            foreach (var data in tokentemp)
            {
                tokens.Add(new TokenViewModel()
                {
                    Id = data.Id,
                    UserId = data.UserId,
                    CreditAmount = data.CreditAmount,
                    OrderId = data.OrderId,
                    PaymentId = data.PaymentId,
                    PaymentToken = data.PaymentToken,
                    PayAmount = data.PayAmount
                });
            }

            return View(tokens);
        }

        [HttpPost]
        public async Task<IActionResult> Buy(Token token)
        {
            await _context.Tokens.AddAsync(token);
            await _context.SaveChangesAsync();

            return RedirectToAction("User", "Token", new { javaScriptToRun = "ShowSuccessMessage('Buy tickets successfully.')" });

        }
    }
}
