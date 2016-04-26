using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Common
{
    public interface IUnitOfWork:IDisposable
    {
        IUserRepository UserRepository { get; }
        IAssetRepository AssetRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        ICurrencyRepository CurrencyRepository { get; }
        ITransactionRepository TransactionRepository { get; }

        void Commit();
    }
}
