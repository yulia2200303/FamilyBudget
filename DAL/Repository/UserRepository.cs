using System;
using System.Linq;
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
            return GetByLogin(login) != null;
        }

        public User GetByLogin(string login)
        {
            return DbContext.Users.FirstOrDefault(u => u.Name.Equals(login, StringComparison.CurrentCultureIgnoreCase));
        }

        //public override User GetById(int id)
        //{
        //    return DbContext.AttachRange()
        //}
    }
}