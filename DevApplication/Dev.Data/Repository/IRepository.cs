using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Dev.Data.Repository
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        T GetById(int id);
        T GetById(long id);
        T GetById(Guid id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(int id);
        bool Delete(Guid id);
        IList<T> GetList();
        IQueryable<T> GetBy(Expression<Func<T, bool>> predicate);
        object ExecWithStoreProcedure(string query, params object[] parameters);
        IEnumerable<T> GetRecordsByPage<TKey>(int pageNumber, int pageSize, Expression<Func<T, TKey>> orderBy);
    }
}
