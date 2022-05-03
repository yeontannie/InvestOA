using InvestOA.Core;

namespace InvestOA.Repositories
{
    public interface IPortfolioRepository
    {
        public Task AddToPortfolio(Portfolio portfolio);
        public Task UpdatePortfolio(Portfolio portfolio);
        public IQueryable<Portfolio> FindStocks(string symbol, string username);
        public Portfolio FindStock(string symbol, string username);
        public Task RemoveStock(Portfolio portfolio);
        public IQueryable<Portfolio> PortfolioByUser(string username);
    }
}
