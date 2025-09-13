using Microsoft.AspNetCore.Mvc;
using RiskMgmt.Web.Models;
using RiskMgmt.Web.Services;

namespace RiskMgmt.Web.Controllers;
//{
//    public class CryptoController
//    {
//    }
//}

// Controllers/CryptoController.cs
[ApiController]
[Route("api/[controller]")]
public class CryptoController : ControllerBase
{
    private readonly ICryptoPriceService _priceService;
    private readonly IRiskCalculatorService _riskCalculator;

    public CryptoController(ICryptoPriceService priceService, IRiskCalculatorService riskCalculator)
    {
        _priceService = priceService;
        _riskCalculator = riskCalculator;
    }

    [HttpGet("price/{symbol}")]
    public async Task<ActionResult<CryptoPriceModel>> GetPrice(string symbol)
    {
        try
        {
            var price = await _priceService.GetCurrentPriceAsync(symbol);
            return Ok(price);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error getting price: {ex.Message}");
        }
    }

    [HttpPost("calculate-risk")]
    public ActionResult<RiskCalculationModel> CalculateRisk([FromBody] RiskCalculationRequest request)
    {
        try
        {
            var result = _riskCalculator.CalculatePositionSize(
                request.AccountBalance,
                request.RiskPercentage,
                request.EntryPrice,
                request.StopLossPrice,
                request.Leverage,
                request.IsShort
            );
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error calculating risk: {ex.Message}");
        }
    }

    public class RiskCalculationRequest
    {
        public decimal AccountBalance { get; set; }
        public decimal RiskPercentage { get; set; }
        public decimal EntryPrice { get; set; }
        public decimal StopLossPrice { get; set; }
        public decimal Leverage { get; set; } = 1;
        public bool IsShort { get; set; }
    }

}
