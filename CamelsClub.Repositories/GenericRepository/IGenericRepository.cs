using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Repositories
{
    public interface IGenericRepository<T>
    {
        IQueryable<T> GetAll();
        void RemoveRange(IEnumerable<int> ids);
        T Add(T entity);
        IEnumerable<T> EditRange(IEnumerable<T> entityList);
        IEnumerable<T> AddRange(IEnumerable<T> entityList);
        T Edit(T entity);
        void Remove(int id);
        T SaveIncluded(T entity, params string[] includedProperties);
        T SaveExcluded(T entity, params string[] excludedProperties);
        void RemoveMany(Expression<Func<T, bool>> where);
        void RemoveByIncluded(T entity);
        T GetById(int id);

    }
}
