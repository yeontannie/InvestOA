using InvestOA.Core;
using InvestOA.Core.Data;
using Microsoft.AspNetCore.Identity;

namespace InvestOA.Repositories
{
    public class HistoryRepository : IHistoryRepository
    {
        private readonly AppDbContext context;
        private readonly SignInManager<User> signInManager;

        public HistoryRepository(AppDbContext ctx, SignInManager<User> signIn)
        {
            context = ctx;
            signInManager = signIn;
        }

        public async Task AddToHistory(History history)
        {
            context.Add(history);
            await context.SaveChangesAsync();
        }

        public IEnumerable<History> GetHistory()
        {
            var username = signInManager.Context.User.Identity.Name;
            return context.Histories.Where(x => x.Username == username);
        }

        public IEnumerable<History> GetPaginated(int page, int pageSize = 7)
        {
            var historyByUser = GetHistory().OrderBy(x => x.TimeOfTransaction).Reverse();
            return historyByUser.Skip(page * pageSize).Take(pageSize).ToList();
        }
    }
}
