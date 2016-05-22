using System.Collections.Generic;
using DAL.Model;
using DAL.Repository.Common;

namespace DAL.Repository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        List<Category> GetCategories();
        List<Category> GetSubCategories(string category);
    }
}