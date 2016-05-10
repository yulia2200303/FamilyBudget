using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Model;
using DAL.Repository.Common;

namespace DAL.Repository
{
    internal class CurrencyRepository : Repository<Currency>, ICurrencyRepository
    {
        public CurrencyRepository(Microsoft.Data.Entity.DbContext dataContext) : base(dataContext)
        {
        }

        public void Refresh(List<Currency> currencies)
        {
            var dbCurrencies = DbContext.Currencies;

            foreach (var currency in currencies)
            {
                var dbCurrency = dbCurrencies
                    .FirstOrDefault(c => c.Code.Equals(currency.Code, StringComparison.CurrentCultureIgnoreCase));

                if (dbCurrency == null)
                {
                    DbContext.Currencies.Add(currency);
                }
                else
                {
                    dbCurrency.Converter = currency.Converter;
                    dbCurrency.Name = currency.Name;
                    dbCurrency.UpadeDate = currency.UpadeDate;
                    DbContext.Currencies.Update(dbCurrency);
                }
            }
        }

        public List<Currency> GetCurrenciesByCode(string[] codes = null, string[] excludedCodes = null)
        {
            var currencies = GetByQuery(c => (codes == null || codes.Length == 0 || codes.Contains(c.Code))
                                             && (excludedCodes == null || excludedCodes.Length == 0 || !excludedCodes.Contains(c.Code)),
                queryable => queryable.OrderBy(c => c.Name));
            return currencies.ToList();
        }
    }
}