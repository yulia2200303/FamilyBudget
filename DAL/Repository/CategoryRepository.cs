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

        public void InsertCategories(IEnumerable<string> categories)
        {
            var dbcategories = GetCategories();

            foreach (var category in categories)
            {
                if (!dbcategories.Any(c => c.Name.Equals(category, StringComparison.CurrentCultureIgnoreCase)))
                {
                    Insert(new Category
                    {
                        Name = category
                    });
                }
                
            }
        }

        public void InsertSubcategories(string category, IEnumerable<string> subcategories)
        {
            var dbCategory =
                DbContext.Categories.FirstOrDefault(
                    c => c.Parent == null && c.Name.Equals(category, StringComparison.CurrentCultureIgnoreCase));

            if(dbCategory == null) return;

            var dbSubcategories = GetSubCategories(dbCategory.Name);
            foreach (var subcategory in subcategories)
            {
                if (!dbSubcategories.Any(s => s.Name.Equals(subcategory, StringComparison.CurrentCultureIgnoreCase)))
                {
                    Insert(new Category()
                    {
                        Name = subcategory,
                        ParentId = dbCategory.Id
                    });
                }
            }

        }
    }
}