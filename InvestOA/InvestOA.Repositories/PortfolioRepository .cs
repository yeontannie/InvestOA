using InvestOA.Core;
using InvestOA.Core.Data;

namespace InvestOA.Repositories
{
    public class PortfolioRepository: IPortfolioRepository
    {
        private readonly AppDbContext context;

        public PortfolioRepository(AppDbContext ctx)
        {
            context = ctx;
        }

        public async Task AddToPortfolio(Portfolio portfolio)
        {
            context.Add(portfolio);
            await context.SaveChangesAsync();
        }

        public Portfolio FindStock(string symbol, string username)
        {
            return context.Portfolios.Where(x => x.Ticker == symbol && x.Username == username).First();
        }

        public IQueryable<Portfolio> FindStocks(string symbol, string username)
        {
            return context.Portfolios.Where(x => x.Ticker == symbol && x.Username == username);
        }

        public async Task RemoveStock(Portfolio portfolio)
        {
            context.Remove(portfolio);
            await context.SaveChangesAsync();
        }

        public IQueryable<Portfolio> PortfolioByUser(string username)
        {
            return context.Portfolios.Where(x => x.Username == username).OrderBy(i => i.Ticker);
        }

        public async Task UpdatePortfolio(Portfolio portfolio)
        {
            context.Update(portfolio);
            await context.SaveChangesAsync();
        }
    }
}
