using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Tracking.Data.General;
using Tracking.Models;

namespace Tracking.Controllers
{
    public class HomeController : Controller
    {
        DataContext context;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, DataContext _context)
        {
            _logger = logger;
            context = _context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<ActionResult<Home>> HomePage(Login login)
        {
            var home = new Home();

            var user = await context.Users
                .SingleOrDefaultAsync(x => x.Username.ToLower() == login.Username.ToLower());

            if (user == null)
            {
                home.Error = "User does not exist";
                return View(home);
            }
            var result = user.Password == login.Password;

            if (!result)
            {
                home.Error = "Wrong password";
                return View(home);
            }

            home.UserID = user.UserID;
            home.Username = user.Username;
            home.Name = user.Name;
            home.Surname = user.Surname;
            
            return View(home);
        }

        [HttpGet]
        public IActionResult Credit(int ide)
        {
            Credit credit = new Credit();
            credit.ide = ide;

            return PartialView(credit);
        }

        [HttpPost]
        public async Task<ActionResult<Credit>> Credit(Credit credit)
        {
            var user = await context.Users.ToListAsync(); 
            var transactions = await context.Transactions
                .Where(x => x.SenderID == credit.SenderID || x.ReceiverID == credit.SenderID || x.SenderID == credit.ReceiverID || x.ReceiverID == credit.ReceiverID).FirstOrDefaultAsync();

            var userBudget = user.Where(e => e.UserID == credit.SenderID).Select(e => e.Budget).FirstOrDefault();
           
            if (transactions != null)
            {
                credit.Error = "There is only one transaction for account allowed in the system!";

                return Json(new ErrorViewModel { Title = "Failed", ErrorDescription = credit.Error });
            }
            if (userBudget - credit.Price < 0)
            {
                credit.Error = "No transaction can be made when the Budget is lower or equal to 0 after it! You got only " + userBudget + "$ in your account!";

                return Json(new ErrorViewModel { Title = "Failed", ErrorDescription = credit.Error });
            }
            
            context.Transactions.Add(new Transactions
            {
                SenderID = credit.SenderID,
                ReceiverID = credit.ReceiverID,
                Price = credit.Price,
                Reason = credit.Reason,
                CreatedAt = DateTime.Now
            });
            var updateSenderBudget = user.Where(e => e.UserID == credit.SenderID).FirstOrDefault();
            updateSenderBudget.Budget = (long)(updateSenderBudget.Budget - credit.Price);
            var updateReceiverBudget = user.Where(e => e.UserID == credit.ReceiverID).FirstOrDefault();
            updateReceiverBudget.Budget = (long)(updateReceiverBudget.Budget + credit.Price);

            await context.SaveChangesAsync();

            return Json(new ErrorViewModel { Title = "Success", ErrorDescription = "The transaction is finished!" });

        }


        [HttpGet]
        public IActionResult Debit(int ide)
        {
            Debit debit = new Debit();
            debit.ide = ide;

            return PartialView(debit);
        }

        [HttpPost]
        public async Task<ActionResult<Debit>> Debit(Debit debit)
        {

            var user = await context.Users.ToListAsync();
            var transactions = await context.Transactions
                .Where(x => x.SenderID == debit.SenderID || x.ReceiverID == debit.SenderID || x.SenderID == debit.ReceiverID || x.ReceiverID == debit.ReceiverID).FirstOrDefaultAsync();

            var userBudget = user.Where(e => e.UserID == debit.SenderID).Select(e => e.Budget).FirstOrDefault();

            if (transactions != null)
            {
                debit.Error = "There is only one transaction for account allowed in the system!";

                return Json(new ErrorViewModel { Title = "Failed", ErrorDescription = debit.Error });
            }
            if (userBudget - debit.Price < 0)
            {
                debit.Error = "No transaction can be made when the Budget is lower or equal to 0 after it! You got only " + userBudget + "$ in your account!";

                return Json(new ErrorViewModel { Title = "Failed", ErrorDescription = debit.Error });
            }

            context.Transactions.Add(new Transactions
            {
                SenderID = debit.SenderID,
                ReceiverID = debit.ReceiverID,
                Price = debit.Price,
                Reason = debit.Reason,
                CreatedAt = DateTime.Now
            });
            var updateSenderBudget = user.Where(e => e.UserID == debit.SenderID).FirstOrDefault();
            updateSenderBudget.Budget = (long)(updateSenderBudget.Budget - debit.Price);
            var updateReceiverBudget = user.Where(e => e.UserID == debit.ReceiverID).FirstOrDefault();
            updateReceiverBudget.Budget = (long)(updateReceiverBudget.Budget + debit.Price);

            await context.SaveChangesAsync();

            return Json(new ErrorViewModel { Title = "Success", ErrorDescription = "The transaction is finished!" });
        }

        public async Task<IActionResult> StatsAsync(int ide)
        {
            Stats stats = new Stats();
            stats.UserID = ide;
            if (ide != 3)
            {
                var senderPrices = await context.Transactions.Where(e => e.SenderID == ide).Select(e => e.Price).ToListAsync();
                stats.CountSenderMoney = senderPrices.Sum(x => Convert.ToDecimal(x));
                var senderReceives = await context.Transactions.Where(e => e.ReceiverID == ide).Select(e => e.Price).ToListAsync();
                stats.CountReceiverMoney = senderReceives.Sum(x => Convert.ToDecimal(x));
                stats.Count = stats.CountReceiverMoney - stats.CountSenderMoney;
            }
            else
            {
                // sum of price for every sender
                var querySender = context.Transactions
                   .GroupBy(e => e.SenderID)
                   .Select(g => new ListKey { key = g.Key, count = g.Sum(e => e.Price), type = 1 });

                stats.CountSenderMoney = querySender.Sum(q => q.count);

                // sum of price for every receiver
                var queryReceiver = context.Transactions
               .GroupBy(e => e.ReceiverID)
               .Select(g => new ListKey { key = g.Key, count = g.Sum(e => e.Price), type = 2 });

                stats.CountReceiverMoney = queryReceiver.Sum(q => q.count);
                stats.Count = stats.CountReceiverMoney - stats.CountSenderMoney;

            }

            return View(stats);
        }
    }
}
