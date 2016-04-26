using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Model;

namespace DAL.Common
{
    public interface IUserRepository:IRepository<User>
    {
        bool IsUserExist(string login);
        User GetByLogin(string login);
    }
}
