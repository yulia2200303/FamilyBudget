using System;
using DAL.DbContext;
using DAL.Repository;

namespace DAL.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        private AssetRepository _assetRepository;
        private CategoryRepository _categoryRepository;
        private CurrencyRepository _currencyRepository;
        private FamilyBudgetContext _dbContext;
#pragma warning disable CS0414 // The field 'UnitOfWork._disposed' is assigned but its value is never used
        private bool _disposed;
#pragma warning restore CS0414 // The field 'UnitOfWork._disposed' is assigned but its value is never used
        private TransactionRepository _transactionRepository;

        private UserRepository _userRepository;

        public UnitOfWork()
        {
            _dbContext = new FamilyBudgetContext();
            _disposed = false;
        }

        public IUserRepository UserRepository
        {
            get { return _userRepository ?? (_userRepository = new UserRepository(_dbContext)); }
        }

        public IAssetRepository AssetRepository
        {
            get { return _assetRepository ?? (_assetRepository = new AssetRepository(_dbContext)); }
        }

        public ICategoryRepository CategoryRepository
        {
            get { return _categoryRepository ?? (_categoryRepository = new CategoryRepository(_dbContext)); }
        }

        public ICurrencyRepository CurrencyRepository
        {
            get { return _currencyRepository ?? (_currencyRepository = new CurrencyRepository(_dbContext)); }
        }

        public ITransactionRepository TransactionRepository
        {
            get { return _transactionRepository ?? (_transactionRepository = new TransactionRepository(_dbContext)); }
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_dbContext != null)
                {
                    _dbContext.Dispose();
                    _dbContext = null;
                }
            }
        }
    }
}