using Microsoft.AspNetCore.Identity;

namespace InvestOA.Core
{
    public class User : IdentityUser
    {
        public double Cash { get; set; }
    }
}
