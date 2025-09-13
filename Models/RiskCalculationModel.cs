namespace RiskMgmt.Web.Models
{
    public class RiskCalculationModel
    {
        public decimal AccountBalance { get; set; }
        public decimal RiskPercentage { get; set; }
        public decimal EntryPrice { get; set; }
        public decimal StopLossPrice { get; set; }
        public decimal PositionSize { get; set; }
        public decimal RiskAmount { get; set; }
    }
}
