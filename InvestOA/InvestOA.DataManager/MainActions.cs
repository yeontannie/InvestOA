using IEXSharp;
using InvestOA.Core;
using InvestOA.Core.Requests;
using InvestOA.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace InvestOA.DataManager
{
    public class MainActions
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IConfiguration configuration;
        private readonly IPortfolioRepository portfolioRepository;
        private readonly IHistoryRepository historyRepository;

        public MainActions(UserManager<User> userM, SignInManager<User> signInM,
            IConfiguration config, PortfolioRepository portfolioRepo, HistoryRepository historyRepo)
        {
            userManager = userM;
            signInManager = signInM;
            configuration = config;
            portfolioRepository = portfolioRepo;
            historyRepository = historyRepo;
        }

        public async Task<Dictionary<string, string>> LookUp(string symbol)
        {
            var publishableToken = configuration["publishableToken"];
            var secretToken = configuration["secretToken"];

            var data = new Dictionary<string, string>();
            using (var iexCloudClient =
                new IEXCloudClient(publishableToken, secretToken, signRequest: false, useSandBox: false))
            {
                var response = await iexCloudClient.StockPrices.QuoteAsync(symbol);
                data.Add("companyName", response.Data.companyName);
                data.Add("price", response.Data.latestPrice.ToString());
                data.Add("symbol", response.Data.symbol);
            }
            return data;
        }


        public async Task<string> Buy(string symbol, int shares)
        {
            var response = LookUp(symbol);
            var data = response.Result;
            if (data == null)
            {
                return "data not found";
            }

            var price = Convert.ToDouble(data["price"]);
            var value = price * shares;

            var username = signInManager.Context.User.Identity.Name;
            var user = await userManager.FindByNameAsync(username);

            if (user.Cash < value)
            {
                return "you have not enough money";
            }
            else
            {
                var stock = portfolioRepository.FindStocks(symbol, username);
                if (stock.Count() == 0)
                {
                    var portfolio = new Portfolio
                    {
                        CompanyName = data["companyName"],
                        Ticker = symbol,
                        Shares = shares,
                        Price = price,
                        TimeOfBuying = DateTime.UtcNow,
                        Username = username
                    };
                    await portfolioRepository.AddToPortfolio(portfolio);
                }
                else if (stock.Count() > 0)
                {
                    var s = stock.First();
                    s.Shares += shares;
                    await portfolioRepository.UpdatePortfolio(s);
                }

                var history = new History
                {
                    Ticker = symbol,
                    Shares = $"+{shares}",
                    Price = price,
                    TimeOfTransaction = DateTime.UtcNow,
                    Username = username
                };
                await historyRepository.AddToHistory(history);

                user.Cash -= value;
                await userManager.UpdateAsync(user);
            }
            return "OK";
        }

        public async Task<string> Sell(string symbol, int shares)
        {
            var username = signInManager.Context.User.Identity.Name;
            var user = await userManager.FindByNameAsync(username);

            var response = LookUp(symbol);
            var data = response.Result;
            var price = Convert.ToDouble(data["price"]);
            var stock = portfolioRepository.FindStock(symbol, username);

            if (stock.Shares < shares)
            {
                return "you don't have that many shares";
            }
            stock.Shares -= shares;

            if (stock.Shares == 0)
            {
                await portfolioRepository.RemoveStock(stock);
            }

            user.Cash += price * shares;
            var history = new History
            {
                Ticker = symbol,
                Shares = $"-{shares}",
                Price = price,
                TimeOfTransaction = DateTime.UtcNow,
                Username = username
            };

            await historyRepository.AddToHistory(history);
            await userManager.UpdateAsync(user);
            return "OK";
        }

        public async Task<IndexRequest> Index()
        {
            var username = signInManager.Context.User.Identity.Name;
            var user = await userManager.FindByNameAsync(username);

            var info = portfolioRepository.PortfolioByUser(username);
            var total = user.Cash;

            var stock = new List<Dictionary<string, string>>();

            if (info.Count() != 0)
            {
                foreach (var i in info)
                {
                    var temp = new Dictionary<string, string>();
                    if (i.Shares == 0)
                    {
                        await portfolioRepository.RemoveStock(i);
                    }
                    else
                    {
                        var response = LookUp(i.Ticker);
                        var data = response.Result;
                        var totalByStock = Convert.ToDouble(data["price"]) * i.Shares;

                        total += totalByStock;
                        temp.Add("symbol", data["symbol"]);
                        temp.Add("shares", i.Shares.ToString());
                        temp.Add("companyName", data["companyName"]);
                        temp.Add("price", data["price"]);
                        temp.Add("total", totalByStock.ToString("F"));
                    }
                    stock.Add(temp);
                }
            }
            return new IndexRequest { Cash = user.Cash.ToString("F"), Total = total.ToString("F"), Stocks = stock };
        }
    }
}
