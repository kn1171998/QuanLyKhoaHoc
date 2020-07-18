using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PayCompute.Service
{
    public interface IBaseService<T> where T : class
    {
        Task CreateAsync(T entity);

        T GetById(int id);
        T GetBySingle(string code);
        //IEnumerable<T> GetPaging(Expression<Func<T, bool>> filter, out int total, int page, int pageSize);
        IEnumerable<T> GetPaging<TProperty>(Expression<Func<T, bool>> predicate, out int total, int page, int pageSize, Expression<Func<T, TProperty>> orderbyDes = null, string[] includes = null);
        IEnumerable<T> GetPaging(out int total, int page, int pageSize);
        int CountCondition(Expression<Func<T, bool>> where);
        IEnumerable<T> GetCondition(Expression<Func<T, bool>> predicate);
        Task UpdateAsync(T entity);
        Task UpdateAsync(int id);
        Task Delete(int id);
        int CountEntity();
        IEnumerable<T> GetAll();
        Task Delete(T entity);
    }
}