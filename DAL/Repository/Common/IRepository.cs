using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DAL.Model.Common;

namespace DAL.Repository.Common
{
    public interface IRepository<T> where T : IEntity
    {
        object Insert(T entity); //добавление нового элемента
        T GetById(int id); //поиск элемента по ключу
        IEnumerable<T> GetAll(); //получение всех элементов
        void Update(T entity); //обновление полей элемента
        void Delete(T entity); //удаление элемента
        void DeleteById(int id); //удаление элемента по ключу

        IEnumerable<T> GetByQuery(Expression<Func<T, bool>> query = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Expression<Func<T, object>> navigationPropertyPath = null);

        //сортировка + оптимизация выборки с помощью загрузки свящанных данных
        //поиск элементов по заданному выражению, 
    }
}