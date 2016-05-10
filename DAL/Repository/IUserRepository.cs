using DAL.Model;
using DAL.Repository.Common;

namespace DAL.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        bool IsUserExist(string login);
        User GetByLogin(string login);
    }
}