using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace P1.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected DBModelContainer _context { get; set; }
        private readonly DbSet<T> dbSet;

        public Repository(DBModelContainer context)
        {
            _context = context;
            dbSet = _context.Set<T>();
        }

        public T Get(int id)
        {
            return dbSet.Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }

        public IEnumerable<T> Query(Expression<Func<T, bool>> whereExpression)
        {
            return dbSet.Where(whereExpression);
        }

        public T Add(T entity)
        {
            var result = dbSet.Add(entity);
            _context.SaveChanges();
            return result;
        }

        public void Delete(int id)
        {
            try
            {
                T entity = Get(id);
                dbSet.Remove(entity);
                _context.SaveChanges();
            }
            catch(InvalidOperationException e)
            {

            }
        }

        public void AddRange(IEnumerable<T> entities)
        {
            dbSet.AddRange(entities);
            _context.SaveChanges();
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
            _context.SaveChanges();
        }
    }
}
