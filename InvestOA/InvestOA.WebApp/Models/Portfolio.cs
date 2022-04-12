using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvestOA.WebApp.Models
{
    public class Portfolio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Ticker { get; set; }
        public int Shares { get; set; }
        public double Price { get; set; }
        public DateTime TimeOfBuying { get; set; }
    }
}
