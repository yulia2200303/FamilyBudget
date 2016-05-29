using System.Collections.Generic;
using System.Linq;
using DAL.Model;
using DAL.Repository.Common;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Query;

namespace DAL.Repository
{
    internal class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(Microsoft.Data.Entity.DbContext dataContext) : base(dataContext)
        {
        }

        public List<Transaction> GetByAssetId(int assetId)
        {
            return
                DbContext.Transactions.Include(t => t.Currency)
                    .Include(t => t.Product)
                    .Include(t => t.Product.Parent)
                    .Where(t => t.AssetId == assetId)
                    .ToList();
        }
    }
}