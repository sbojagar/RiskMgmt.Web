namespace RiskMgmt.Web.Models;

// Models/RiskCalculationModel.cs
public class RiskCalculationModel
{
    public decimal AccountBalance { get; set; }
    public decimal RiskPercentage { get; set; }
    public decimal EntryPrice { get; set; }
    public decimal StopLossPrice { get; set; }
    public decimal PositionSize { get; set; }
    public decimal RiskAmount { get; set; }
    public decimal Leverage { get; set; } = 1;
    public decimal MarginRequired { get; set; }
    public decimal TotalPositionValue { get; set; }
    public bool IsShort { get; set; }

    // Calculated properties
    public decimal LiquidationPrice => CalculateLiquidationPrice();

    private decimal CalculateLiquidationPrice()
    {
        if (IsShort)
            return EntryPrice + (EntryPrice / Leverage); // Short liquidation
        else
            return EntryPrice - (EntryPrice / Leverage); // Long liquidation
    }
}
