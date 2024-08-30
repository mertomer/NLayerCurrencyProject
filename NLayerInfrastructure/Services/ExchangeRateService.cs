using NLayerCore.Entities;
using NLayerCore.Interfaces;
using NLayerInfrastructure.Services;
using StackExchange.Redis;
using System.Threading.Tasks;
using System.Xml;

namespace NLayerInfrastructure.MessageBroker
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly IRabbitMQPublisher _rabbitMQPublisher;
        private readonly IRabbitMQConsumer _rabbitMQConsumer;
        private readonly RedisService _redisService;

        public ExchangeRateService(IRabbitMQPublisher rabbitMQPublisher, IRabbitMQConsumer rabbitMQConsumer, RedisService redisService)
        {
            _rabbitMQPublisher = rabbitMQPublisher;
            _rabbitMQConsumer = rabbitMQConsumer;
            _redisService = redisService;
        }

        public async Task<ExchangeRate> GetExchangeRateAsync(string currencyCode)
        {
            // Önce Redis'te döviz verisi var mı kontrol et
            var cachedRate = await _redisService.GetExchangeRateAsync(currencyCode);
            if (!string.IsNullOrEmpty(cachedRate))
            {
                // Redis'ten döviz verisini çek ve döndür
                var parts = cachedRate.Split(':');
                return new ExchangeRate
                {
                    CurrencyCode = currencyCode,
                    BanknoteBuying = decimal.Parse(parts[0]),
                    BanknoteSelling = decimal.Parse(parts[1])
                };
            }

            // Eğer Redis'te yoksa, TCMB'den veriyi çek
            string url = "https://www.tcmb.gov.tr/kurlar/today.xml";
            var xmldoc = new XmlDocument();
            xmldoc.Load(url);

            var exchangeRate = new ExchangeRate
            {
                CurrencyCode = currencyCode,
                BanknoteBuying = Convert.ToDecimal(xmldoc.SelectSingleNode($"Tarih_Date/Currency[@Kod='{currencyCode}']/BanknoteBuying")?.InnerXml),
                BanknoteSelling = Convert.ToDecimal(xmldoc.SelectSingleNode($"Tarih_Date/Currency[@Kod='{currencyCode}']/BanknoteSelling")?.InnerXml)
            };

            var message = $"{exchangeRate.BanknoteBuying}:{exchangeRate.BanknoteSelling}";
            await _redisService.SetExchangeRateAsync(currencyCode, message); // Redis'e kaydet
            _rabbitMQPublisher.Publish(message);

            return exchangeRate;
        }

        public string GetExchangeRateFromQueue()
        {
            return _rabbitMQConsumer.Consume();
        }
    }
}
