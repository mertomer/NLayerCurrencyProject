using NLayerCore.Entities;
using NLayerCore.Interfaces;
using System.Xml;

namespace NLayerInfrastructure.MessageBroker
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly IRabbitMQPublisher _rabbitMQPublisher;
        private readonly IRabbitMQConsumer _rabbitMQConsumer;

        public ExchangeRateService(IRabbitMQPublisher rabbitMQPublisher, IRabbitMQConsumer rabbitMQConsumer)
        {
            _rabbitMQPublisher = rabbitMQPublisher;
            _rabbitMQConsumer = rabbitMQConsumer;
        }

        public async Task<ExchangeRate> GetExchangeRateAsync(string currencyCode)
        {
            string url = "https://www.tcmb.gov.tr/kurlar/today.xml";
            var xmldoc = new XmlDocument();
            xmldoc.Load(url);

            var exchangeRate = new ExchangeRate
            {
                CurrencyCode = currencyCode,
                BanknoteBuying = Convert.ToDecimal(xmldoc.SelectSingleNode($"Tarih_Date/Currency[@Kod='{currencyCode}']/BanknoteBuying")?.InnerXml),
                BanknoteSelling = Convert.ToDecimal(xmldoc.SelectSingleNode($"Tarih_Date/Currency[@Kod='{currencyCode}']/BanknoteSelling")?.InnerXml)
            };

            var message = $"{currencyCode}:{exchangeRate.BanknoteBuying}:{exchangeRate.BanknoteSelling}";
            _rabbitMQPublisher.Publish(message);

            return exchangeRate;
        }

        public string GetExchangeRateFromQueue()
        {
            return _rabbitMQConsumer.Consume();
        }
    }
}
