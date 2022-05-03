using InvestOA.Core;

namespace InvestOA.Repositories
{
    public interface IHistoryRepository
    {
        public Task AddToHistory(History history);
        public IQueryable<History> GetHistory();
    }
}
