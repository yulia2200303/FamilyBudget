using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DAL.DbContext;
using DAL.Model.Common;
using Microsoft.Data.Entity;

namespace DAL.Repository.Common
{
    public class Repository<T> : IRepository<T> where T : class, IEntity
    {
        protected FamilyBudgetContext DbContext;
        protected DbSet<T> DbSet;

        public Repository(Microsoft.Data.Entity.DbContext dataContext)
        {
            DbContext = (FamilyBudgetContext) dataContext;
            DbSet = dataContext.Set<T>();
        }

        public void Delete(T entity)
        {
            if (DbContext.Entry(entity).State == EntityState.Detached)
                DbSet.Attach(entity);

            DbSet.Remove(entity);
        }


        public void DeleteById(int id)
        {
            var entity = DbSet.First(e => e.Id == id);
            DbSet.Remove(entity);
        }

        public IEnumerable<T> GetAll()
        {
            return DbSet.AsEnumerable();
        }

        object IRepository<T>.Insert(T entity)
        {
            return Insert(entity);
        }

        public virtual T GetById(int id)
        {
            return DbSet.FirstOrDefault(e => e.Id == id);
        }

        public IEnumerable<T> GetByQuery(Expression<Func<T, bool>> query = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Expression<Func<T, object>> navigationPropertyPath = null)
        {
            IQueryable<T> queryResult = DbSet;

            if (query != null)
            {
                queryResult = queryResult.Where(query);
            }

            if (navigationPropertyPath != null)
            {
                queryResult = queryResult.Include(navigationPropertyPath); //!!!! check it
            }

            if (orderBy != null)
            {
                return orderBy(queryResult).ToList();
            }

            return queryResult.ToList();
        }

        public void Update(T entity)
        {
            DbSet.Attach(entity);
            DbContext.Entry(entity).State = EntityState.Modified;
        }

        public T Insert(T entity)
        {
            return DbSet.Add(entity).Entity; //!!! check it
        }
    }
}