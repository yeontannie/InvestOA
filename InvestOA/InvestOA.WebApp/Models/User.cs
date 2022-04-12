using Microsoft.AspNetCore.Identity;

namespace InvestOA.WebApp.Models
{
    public class User : IdentityUser
    {
        public double Cash { get; set; }
    }
}
