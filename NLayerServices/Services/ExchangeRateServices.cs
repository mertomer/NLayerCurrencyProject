using NLayerCore.Entities;
using NLayerCore.Interfaces;

namespace NLayerService.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly IExchangeRateService _exchangeRateService;

        public ExchangeRateService(IExchangeRateService exchangeRateService)
        {
            _exchangeRateService = exchangeRateService;
        }

        public async Task<ExchangeRate> GetExchangeRateAsync(string currencyCode)
        {
            return await _exchangeRateService.GetExchangeRateAsync(currencyCode);
        }

        public string GetExchangeRateFromQueue()
        {
            return _exchangeRateService.GetExchangeRateFromQueue();
        }
    }
}
