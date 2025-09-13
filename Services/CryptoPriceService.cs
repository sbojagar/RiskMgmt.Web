namespace RiskMgmt.Web.Services;
//{
//    public class CryptoPriceService
//    {
//    }
//}
// Services/CryptoPriceService.cs
using Binance.Net.Clients;
using Binance.Net.Interfaces.Clients;
using Microsoft.AspNetCore.SignalR;
using RiskMgmt.Web.Hubs;
using RiskMgmt.Web.Models;
using System;
using System.Drawing;

public interface ICryptoPriceService
{
    Task<CryptoPriceModel> GetCurrentPriceAsync(string symbol);
    Task StartRealTimePriceUpdates();
}

public class CryptoPriceService : ICryptoPriceService, IHostedService
{
    private readonly IBinanceSocketClient _binanceSocketClient;
    private readonly IHubContext<PriceHub> _hubContext;
    private readonly ILogger<CryptoPriceService> _logger;
    private readonly Timer _timer;
    private readonly List<string> _trackedSymbols = new() { "BTCUSDT", "ETHUSDT", "ADAUSDT", "SOLUSDT", "XRPUSDT" };

    public CryptoPriceService(IBinanceSocketClient binanceSocketClient,
                            IHubContext<PriceHub> hubContext,
                            ILogger<CryptoPriceService> logger)
    {
        _binanceSocketClient = binanceSocketClient;
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task<CryptoPriceModel> GetCurrentPriceAsync(string symbol)
    {
        try
        {
            using var restClient = new BinanceRestClient();
            var tickerResult = await restClient.SpotApi.ExchangeData.GetTickerAsync(symbol);

            if (tickerResult.Success)
            {
                return new CryptoPriceModel
                {
                    Symbol = symbol,
                    Price = tickerResult.Data.LastPrice,
                    //Change24h = tickerResult.Data.PriceChangePercent ?? 0,
                    Change24h = tickerResult.Data.PriceChangePercent,
                    LastUpdate = DateTime.UtcNow
                };
            }

            throw new Exception($"Failed to get price for {symbol}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting price for {Symbol}", symbol);
            throw;
        }
    }

    public async Task StartRealTimePriceUpdates()
    {
        try
        {
            await _binanceSocketClient.SpotApi.ExchangeData.SubscribeToTickerUpdatesAsync(
                _trackedSymbols,
                data =>
                {
                    var priceModel = new CryptoPriceModel
                    {
                        Symbol = data.Data.Symbol,
                        Price = data.Data.LastPrice,
                        Change24h = data.Data.PriceChangePercent,
                        LastUpdate = DateTime.UtcNow
                    };

                    // Send real-time updates to connected clients
                    _hubContext.Clients.All.SendAsync("ReceivePriceUpdate", priceModel);
                }
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error starting real-time price updates");
        }
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _ = StartRealTimePriceUpdates();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
