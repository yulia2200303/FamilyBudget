using System;
using DAL.Model;
using DAL.Repository.Common;

namespace DAL.Repository
{
    internal class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(Microsoft.Data.Entity.DbContext dataContext) : base(dataContext)
        {
        }

        public bool IsUserExist(string login)
        {
            throw new NotImplementedException();
        }

        public User GetByLogin(string login)
        {
            throw new NotImplementedException();
        }
    }
}