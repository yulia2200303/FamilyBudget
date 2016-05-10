using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Common;
using DAL.Model;

namespace DAL.Migrations
{
    public static class FamilyBudgetInitializer
    {
        private static void InitializeCurrency()
        {
            using (var uow = new UnitOfWork())
            {
                var date = new DateTime(2016, 05, 09);

                var currencies = new List<Currency>
                {
                    new Currency
                    {
                        Name = "American Dollar",
                        Code = "USD",
                        Converter = 19340,
                        UpadeDate = date
                    },
                    new Currency
                    {
                        Name = "Russian Rouble",
                        Code = "RUB",
                        Converter = 290,
                        UpadeDate = date
                    },
                    new Currency
                    {
                        Name = "Belarusian Ruble",
                        Code = "BYR",
                        Converter = 1,
                        UpadeDate = date
                    },
                    new Currency
                    {
                        Name = "Ukraine Hryvnia",
                        Code = "UAH",
                        Converter = 760,
                        UpadeDate = date
                    },
                    new Currency
                    {
                        Name = "Euro",
                        Code = "EUR",
                        Converter = 21970,
                        UpadeDate = date
                    }
                };

                var dbCurrencies = uow.CurrencyRepository.GetAll();

                foreach (var currency in currencies)
                {
                    var dbCurrency =
                        dbCurrencies.FirstOrDefault(
                            c => string.Equals(c.Code, currency.Code, StringComparison.CurrentCultureIgnoreCase));
                    if (dbCurrency == null)
                    {
                        uow.CurrencyRepository.Insert(currency);
                    }
                }
                uow.Commit();
            }
        }

        public static void Initialize()
        {
            InitializeCurrency();
        }
    }
}