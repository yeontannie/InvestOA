using InvestOA.Core;
using InvestOA.Core.Requests;
using InvestOA.DataManager;
using InvestOA.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvestOA.WebApp.Controllers
{
    [Authorize]
    public class PortfolioController : Controller
    {
        private readonly MainActions mainActions;
        private readonly IHistoryRepository historyRepository;

        public PortfolioController(MainActions main, HistoryRepository historyRepo)
        {
            mainActions = main;
            historyRepository = historyRepo;
        }

        [HttpGet]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            var response = await mainActions.Index();
            return View(response);
        }

        [HttpGet]
        [Route("quote")]
        public IActionResult Quote()
        {
            return View();
        }

        [HttpPost]
        [Route("quoted")]
        public IActionResult QuoteTicker([Bind("Symbol")] QuoteRequest model)
        {
            if (string.IsNullOrEmpty(model.Symbol))
            {
                return View("Apology", new Apology { StatusCode = 400, Description = "Must enter symbol" });
            }

            try
            {
                var response = mainActions.LookUp(model.Symbol.ToUpper());
                var data = response.Result;
                return View("Quoted", new QuoteRequest { CompanyName = data["companyName"], Symbol = data["symbol"], Price = data["price"] });
            }
            catch (Exception e)
            {
                return View("Apology", new Apology { StatusCode = 400, Description = e.Message });
            }
            return View("Quote");
        }

        [HttpGet]
        [Route("buy")]
        public IActionResult Buy()
        {
            return View();
        }

        [HttpPost]
        [Route("buy")]
        public async Task<IActionResult> BuyStock([Bind("Symbol,Shares")] TransactionRequest model)
        {
            if (ModelState.IsValid)
            {
                var response = await mainActions.Buy(model.Symbol.ToUpper(), model.Shares);
                switch (response.ToString())
                {
                    case "data not found":
                        return View("Apology", new Apology { StatusCode = 400, Description = response.ToString() });
                        break;

                    case "you have not enough money":
                        return View("Apology", new Apology { StatusCode = 400, Description = response.ToString() });
                        break;

                    case "OK":
                        TempData["success"] = "Bought successfully";
                        return RedirectToAction("Index");
                        break;

                    default:
                        break;
                }
            }
            return View("Apology", new Apology { StatusCode = 400, Description = "must enter symbol and shares quontity" });
        }

        [HttpGet]
        [Route("sell")]
        public IActionResult Sell()
        {
            return View();
        }

        [HttpPost]
        [Route("sell")]
        public async Task<IActionResult> SellStock([Bind("Symbol,Shares")] TransactionRequest model)
        {
            if (ModelState.IsValid)
            {
                var response = await mainActions.Sell(model.Symbol.ToUpper(), model.Shares);
                switch (response.ToString())
                {
                    case "OK":
                        TempData["success"] = "Sold successfully";
                        return RedirectToAction("Index");
                        break;

                    case "you don't have that many stock":
                        return View("Apology", new Apology { StatusCode = 400, Description = "you don't have that many shares" });
                        break;

                    default:
                        break;
                }
            }
            return View("Apology", new Apology { StatusCode = 400, Description = "must enter symbol and share quontity" });
        }

        [HttpGet]
        [Route("history")]
        public async Task<IActionResult> History()
        {
            var response = historyRepository.GetHistory();
            return View(response);
        }
    }
}
