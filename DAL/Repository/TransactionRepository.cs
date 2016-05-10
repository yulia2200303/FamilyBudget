using DAL.Model;
using DAL.Repository.Common;

namespace DAL.Repository
{
    internal class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(Microsoft.Data.Entity.DbContext dataContext) : base(dataContext)
        {
        }
    }
}