using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common;
using DAL.Model;
using Shared.Constant;

namespace UI.Logic
{
    public sealed class CurrencyConverter
    {
        private readonly string _defaultCurrencyCode;
        private readonly Currency _defaultCurrency;
        private readonly List<Currency> Currencies;

        public CurrencyConverter()
        {
            _defaultCurrencyCode = CurrencyCode.BelarussianRub;

            using (var uow = new UnitOfWork())
            {
                Currencies = uow.CurrencyRepository.GetAll().ToList();
                _defaultCurrency = Currencies.First(c => c.Code == _defaultCurrencyCode);
            }
        }

        public double Convert(string codeFrom, string codeTo)
        {
            var currencyFrom = Currencies.First(c => c.Code == codeFrom);
            var currencyTo = Currencies.First(c => c.Code == codeTo);
            return (currencyFrom.Converter/currencyTo.Converter);
        }
    }
}
