using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DAL.Common;
using DAL.Model;
using Shared.Constant;
using UI.ExchangeRatesService;
using UI.Logic.Extension;

namespace UI.Logic
{
    public class CurrencyWrapper
    {
        private readonly string[] _availablyCurrencies;
        private readonly ExRatesSoapClient _client;


        public CurrencyWrapper()
        {
            _client = new ExRatesSoapClient();
            _availablyCurrencies = new[]
            {
                CurrencyCode.AmericanDoll,
                CurrencyCode.Euro,
                CurrencyCode.RussianRub,
                CurrencyCode.UkraineHry
            };
        }

        public async Task<List<Currency>> GetCurrenciesFromServer()
        {
            var date = DateTime.Now;
            var xmlData = await _client.ExRatesDailyAsync(date);
            var root = xmlData.Nodes[1]; //skip xml scheme

            var currencies = root.Element("NewDataSet").Elements("DailyExRatesOnDate")
                .Where(el => _availablyCurrencies.Contains((string) el.Element("Cur_Abbreviation")))
                .Select(el => new Currency
                {
                    Name = ConvertCurrencyName((string) el.Element("Cur_QuotName")),
                    Code = (string) el.Element("Cur_Abbreviation"),
                    Converter = (double) el.Element("Cur_OfficialRate"),
                    UpadeDate = date
                }).ToList();

            return currencies;
        }

        public async Task RefreshCurrencies()
        {
            try
            {
                using (var uow = new UnitOfWork())
                {
                    var currencies = await GetCurrenciesFromServer();

                    uow.CurrencyRepository.Refresh(currencies);
                    uow.Commit();
                }
            }
            catch (Exception)
            {
                throw new Exception("Произошла ошибка при обновлении данных");
            }
        }

        /// <summary>
        ///     convert currency name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string ConvertCurrencyName(string name)
        {
            var pattern = @"^\d+\s"; //delete first symbol and space ex. 1 австралийский доллар 
            return Regex.Replace(name, pattern, "", RegexOptions.Compiled).FirstCharToUpper();
        }
    }
}