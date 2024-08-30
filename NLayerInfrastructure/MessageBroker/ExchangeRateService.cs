using System.Xml;
using NLayerCore.Entities;
using NLayerCore.Interfaces;

namespace NLayerInfrastructure.MessageBroker
{
    public class ExchangeRateService : IExchangeRateService
    {
        public async Task<ExchangeRate> GetExchangeRateAsync(string currencyCode)
        {
            string url = "https://www.tcmb.gov.tr/kurlar/today.xml";
            var xmldoc = new XmlDocument();
            xmldoc.Load(url);

            var banknoteBuyingNode = xmldoc.SelectSingleNode($"Tarih_Date/Currency[@Kod='{currencyCode}']/BanknoteBuying");
            var banknoteSellingNode = xmldoc.SelectSingleNode($"Tarih_Date/Currency[@Kod='{currencyCode}']/BanknoteSelling");

            decimal banknoteBuying = banknoteBuyingNode != null ? Convert.ToDecimal(banknoteBuyingNode.InnerXml) : 0;
            decimal banknoteSelling = banknoteSellingNode != null ? Convert.ToDecimal(banknoteSellingNode.InnerXml) : 0;

            var exchangeRate = new ExchangeRate
            {
                CurrencyCode = currencyCode,
                BanknoteBuying = banknoteBuying,
                BanknoteSelling = banknoteSelling
            };

            return exchangeRate;
        }
    }
}
