using DAL.Model;
using DAL.Repository.Common;

namespace DAL.Repository
{
    internal class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(Microsoft.Data.Entity.DbContext dataContext) : base(dataContext)
        {
        }
    }
}