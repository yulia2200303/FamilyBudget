using System;
using System.Linq;
using DAL.Model;
using DAL.Repository.Common;
using Shared.Constant;
using Shared.Enum;

namespace DAL.Repository
{
    internal class AssetRepository : Repository<Asset>, IAssetRepository
    {
        public AssetRepository(Microsoft.Data.Entity.DbContext dataContext) : base(dataContext)
        {
        }

        public bool IsAssetExists(string name)
        {
            return DbContext.Assets.Any(a => a.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }

        public double GetSummary(int assetId)
        {
           double result = 0;

            var transactions = DbContext.Transactions.Where(t => t.AssetId == assetId);

            var curencies = DbContext.Currencies.ToList();

            foreach (var transaction in transactions)
            {
                double cost;

                var cussrency = curencies.First(c => c.Id == transaction.CurrencyId);
                if (cussrency.Code.Equals(CurrencyCode.BelarussianRub))
                {
                    cost = transaction.Cost;
                }
                else
                {
                    cost = transaction.Cost*cussrency.Converter;
                }

                if (transaction.Type == (int) OperationType.Debit)
                {
                    result -= cost;
                }
                else
                {
                    result += cost;
                }
            }

            return result;
        }
    }
}