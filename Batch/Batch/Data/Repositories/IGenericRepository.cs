using System;
using System.Collections.Generic;
using Data.Model;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> FindAllBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate);
        IEnumerable<T> List();
        IEnumerable<T> Edit(IEnumerable<T> entityList);
        TemplateEntities1 Context();

        bool ExecuteStoredProcedure(string ProcedureName, List<Tuple<string, object>> Parameters);

        void setLazyLoading(bool LazyLoadingEnabled);
        T Get(int id);
        T Get(Guid id);
        T Add(T entity);
        T Delete(T entity);
        T Delete(int id);
        T Delete(Guid id);
        T Edit(T entity);

        bool Save();
        void Reload(T entity, string property);
        bool HasModifications();

        int Count();


    }
}