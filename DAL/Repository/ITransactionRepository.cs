using System.Collections.Generic;
using DAL.Model;
using DAL.Repository.Common;

namespace DAL.Repository
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        List<Transaction> GetByAssetId(int assetId);
    }
}