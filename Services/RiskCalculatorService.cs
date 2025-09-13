using RiskMgmt.Web.Models;

namespace RiskMgmt.Web.Services;
//{
//    public class RiskCalculatorService
//    {
//    }
//}
// Services/RiskCalculatorService.cs
public interface IRiskCalculatorService
{
    RiskCalculationModel CalculatePositionSize(decimal accountBalance, decimal riskPercentage,
                                             decimal entryPrice, decimal stopLossPrice);
}

public class RiskCalculatorService : IRiskCalculatorService
{
    public RiskCalculationModel CalculatePositionSize(decimal accountBalance, decimal riskPercentage,
                                                    decimal entryPrice, decimal stopLossPrice)
    {
        // Risk amount = Account Balance × Risk Percentage
        var riskAmount = accountBalance * (riskPercentage / 100);

        // Price difference between entry and stop loss
        var priceDifference = Math.Abs(entryPrice - stopLossPrice);

        // Position size = Risk Amount / Price Difference
        var positionSize = riskAmount / priceDifference;

        // USDT value = Position Size × Entry Price
        var usdtSize = positionSize * entryPrice;

        return new RiskCalculationModel
        {
            AccountBalance = accountBalance,
            RiskPercentage = riskPercentage,
            EntryPrice = entryPrice,
            StopLossPrice = stopLossPrice,
            PositionSize = positionSize,
            RiskAmount = riskAmount
        };
    }
}

