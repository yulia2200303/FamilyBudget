using System;
using DAL.Repository;

namespace DAL.Common
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        IAssetRepository AssetRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        ICurrencyRepository CurrencyRepository { get; }
        ITransactionRepository TransactionRepository { get; }

        void Commit();
    }
}