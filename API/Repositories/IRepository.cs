using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace P1.Data.Repositories
{
    public interface IRepository<T>
    {
        T Get(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Query(Expression<Func<T, bool>> whereExpression);
        T Add(T entity);
        void Delete(int id);
        void AddRange(IEnumerable<T> entities);
        void DeleteRange(IEnumerable<T> entities);
    }
}
