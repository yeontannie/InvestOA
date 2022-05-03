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

        public IQueryable<History> GetHistory()
        {
            var username = signInManager.Context.User.Identity.Name;
            return context.Histories.Where(x => x.Username == username);
        }
    }
}
