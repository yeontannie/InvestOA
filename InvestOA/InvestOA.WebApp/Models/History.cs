using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvestOA.WebApp.Models
{
    public class History
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Ticker { get; set; }
        public string Shares { get; set; }
        public double Price { get; set; }
        public DateTime TimeOfSelling { get; set; }
    }
}
