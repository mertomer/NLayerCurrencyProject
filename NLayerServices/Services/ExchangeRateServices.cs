using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
