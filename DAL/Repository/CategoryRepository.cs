using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Model;
using DAL.Repository.Common;
using Microsoft.Data.Entity;

namespace DAL.Repository
{
    internal class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(Microsoft.Data.Entity.DbContext dataContext) : base(dataContext)
        {
            
        }

        public List<Category> GetCategories()
        {
            return DbContext.Categories.Include(c => c.SubCategories).Where(c => c.ParentId == null).ToList();
        }

        public List<Category> GetSubCategories(string category)
        {
            return
                DbContext.Categories.
                    Where(
                        c =>
                            c.ParentId != null &&
                            c.Parent.Name.Equals(category, StringComparison.CurrentCultureIgnoreCase))
                    .ToList();
        }
    }
}