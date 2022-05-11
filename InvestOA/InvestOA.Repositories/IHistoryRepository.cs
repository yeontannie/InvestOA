using InvestOA.Core;

namespace InvestOA.Repositories
{
    public interface IHistoryRepository
    {
        public Task AddToHistory(History history);
        public IEnumerable<History> GetHistory();
        public IEnumerable<History> GetPaginated(int page, int pageSize);
    }
}
