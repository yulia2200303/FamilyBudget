using System;
using System.Collections.Generic;
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

        public List<Asset> GetByUserId(int userId)
        {
            return DbContext.Assets.Where(a => a.UserId == userId).ToList();
        }

        public void RemoveByUserId(int userId)
        {
            var assets = DbContext.Assets.Where(a => a.UserId == userId);
            foreach (var asset in assets)
            {
                Delete(asset);
            }
        }

        public void Insert(int userId, IEnumerable<string> assets)
        {
            var dbAssets = GetByUserId(userId);

            foreach (var asset in assets)
            {
                if (!dbAssets.Any(a => a.Name.Equals(asset, StringComparison.CurrentCultureIgnoreCase)))
                {
                    Insert(new Asset
                    {
                        Name = asset,
                        UserId = userId
                    });
                }
            }

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