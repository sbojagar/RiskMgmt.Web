using RiskMgmt.Web.Models;

// Services/RiskCalculatorService.cs
public interface IRiskCalculatorService
{
    RiskCalculationModel CalculatePositionSize(decimal accountBalance, decimal riskPercentage,
                                             decimal entryPrice, decimal stopLossPrice,
                                             decimal leverage, bool isShort);
}

public class RiskCalculatorService : IRiskCalculatorService
{
    public RiskCalculationModel CalculatePositionSize(decimal accountBalance, decimal riskPercentage,
                                                    decimal entryPrice, decimal stopLossPrice,
                                                    decimal leverage, bool isShort)
    {
        // Risk amount = Account Balance × Risk Percentage
        var riskAmount = accountBalance * (riskPercentage / 100);

        // Calculate price difference (risk per coin/contract)
        decimal priceDifference;
        if (isShort)
        {
            // For shorts: risk when price goes UP (stop loss > entry price)
            priceDifference = Math.Abs(stopLossPrice - entryPrice);
        }
        else
        {
            // For longs: risk when price goes DOWN (stop loss < entry price)
            priceDifference = Math.Abs(entryPrice - stopLossPrice);
        }

        // Position size in coins/contracts = Risk Amount ÷ Price Difference
        var positionSize = riskAmount / priceDifference;

        // USDT margin required = (Position Size × Entry Price) ÷ Leverage
        var marginRequired = (positionSize * entryPrice) / leverage;

        // Total position value (notional) = Position Size × Entry Price
        var totalPositionValue = positionSize * entryPrice;

        return new RiskCalculationModel
        {
            AccountBalance = accountBalance,
            RiskPercentage = riskPercentage,
            EntryPrice = entryPrice,
            StopLossPrice = stopLossPrice,
            PositionSize = positionSize,
            RiskAmount = riskAmount,
            Leverage = leverage,
            MarginRequired = marginRequired,
            TotalPositionValue = totalPositionValue,
            IsShort = isShort
        };
    }
}

