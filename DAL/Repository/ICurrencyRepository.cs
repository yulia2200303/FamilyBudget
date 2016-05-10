using System.Collections.Generic;
using DAL.Model;
using DAL.Repository.Common;

namespace DAL.Repository
{
    public interface ICurrencyRepository : IRepository<Currency>
    {
        /// <summary>
        ///     обновляет записи в бд
        /// </summary>
        /// <param name="currencies"></param>
        /// <returns></returns>
        void Refresh(List<Currency> currencies);

        List<Currency> GetCurrenciesByCode(string[] codes = null, string[] excludedCodes = null);
    }
}