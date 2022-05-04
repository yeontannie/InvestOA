namespace InvestOA.Core.Requests
{
    public class IndexRequest
    {
        public string Cash { get; set; }
        public string Total { get; set; } = "0";
        public List<Dictionary<string, string>>? Stocks { get; set; }
    }
}
