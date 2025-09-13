namespace RiskMgmt.Web.Models
{
    public class CryptoPriceModel
    {
        public string Symbol { get; set; }
        public decimal Price { get; set; }
        public DateTime LastUpdate { get; set; }
        public decimal Change24h { get; set; }
    }
}
