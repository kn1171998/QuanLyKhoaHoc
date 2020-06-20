using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebData.Models;

namespace PayCompute.Service
{
    public abstract class RepositoryBaseService<T> : IBaseService<T> where T : class
    {
        protected readonly quanlykhoahocContext _context;
        private readonly DbSet<T> dbSet;

        protected quanlykhoahocContext DbContext
        {
            get { return _context; }
        }

        protected RepositoryBaseService(quanlykhoahocContext context)
        {
            this._context = context;
            dbSet = _context.Set<T>();
        }

        public async Task CreateAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var entity = GetById(id);
            dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll()
        => dbSet.AsQueryable();

        public T GetById(int id)
        => dbSet.Find(id);

        public async Task UpdateAsync(T entity)
        {
            dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id)
        {
            var entity = GetById(id);
            dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public T GetBySingle(string code)
        => dbSet.Find(code);

        public IEnumerable<T> GetPaging<TProperty>(Expression<Func<T, bool>> predicate, out int total, int page, int pageSize, Expression<Func<T, TProperty>> orderbyDes = null)
        {
            int skipCount = (page - 1) * pageSize;
            IQueryable<T> _resetSet;
            _resetSet = predicate != null ? dbSet.Where<T>(predicate).AsQueryable() : dbSet.AsQueryable();
            _resetSet = orderbyDes != null ? _resetSet.OrderByDescending(orderbyDes).AsQueryable() : _resetSet;
            total = _resetSet.Count();
            _resetSet = skipCount == 0 ? _resetSet.Take(pageSize) : _resetSet.Skip(skipCount).Take(pageSize);
            return _resetSet.AsQueryable();
        }

        public int CountEntity()
        => dbSet.Count();

        public IEnumerable<T> GetPaging(out int total, int page, int pageSize)
        {
            var _resetSet = dbSet.Skip((page - 1) * pageSize).Take(pageSize);
            total = _resetSet.Count();
            return _resetSet;
        }

        public int CountCondition(Expression<Func<T, bool>> where)
        {
            return dbSet.Count(where);
        }

        public IEnumerable<T> GetCondition(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> _resetSet;
            _resetSet = predicate != null ? dbSet.Where<T>(predicate).AsQueryable() : dbSet.AsQueryable();
            return _resetSet.AsQueryable();
        }
    }
}