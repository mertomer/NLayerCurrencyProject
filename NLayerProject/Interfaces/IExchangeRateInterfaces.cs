using NLayerCore.Entities;
using System.Threading.Tasks;

namespace NLayerCore.Interfaces
{
    public interface IExchangeRateService
    {
        Task<ExchangeRate> GetExchangeRateAsync(string currencyCode);
        string GetExchangeRateFromQueue(); // RabbitMQ kuyruğundan veriyi bu methodla çektim
    }
}
