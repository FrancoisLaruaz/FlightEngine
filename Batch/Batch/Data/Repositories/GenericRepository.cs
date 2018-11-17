using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Data.Model;
using System.Data.Entity.Validation;
using Data.Helpers;

namespace Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected TemplateEntities1 _context;
        protected readonly IDbSet<T> _dbset;

        public GenericRepository(TemplateEntities1 context)
        {
            _context = context;
            _dbset = _context.Set<T>();
        }

        public TemplateEntities1 Context()
        {
            return this._context;
        }

        public void setLazyLoading(bool LazyLoadingEnabled)
        {
            this._context.Configuration.LazyLoadingEnabled = LazyLoadingEnabled;
        }

        public virtual IEnumerable<T> List()
        {
            return _dbset.AsEnumerable<T>();
        }

        public IEnumerable<T> FindAllBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> query = _dbset.Where(predicate).AsEnumerable();
            return query;
        }

        public virtual T Add(T entity)
        {
            return _dbset.Add(entity);
        }

        public virtual T Delete(T entity)
        {
            return _dbset.Remove(entity);
        }

        public virtual T Delete(int id)
        {
            return _dbset.Remove(_dbset.Find(id));
        }

        public virtual T Delete(Guid id)
        {
            return _dbset.Remove(_dbset.Find(id));
        }

        public virtual T Edit(T entity)
        {
            //_context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            _context.Entry(entity).CurrentValues.SetValues(entity);
            return entity;
        }

        public virtual IEnumerable<T> Edit(IEnumerable<T> entityList)
        {
            foreach (T entity in entityList)
            {
                _context.Entry(entity).CurrentValues.SetValues(entity);
            }
            return entityList;
        }

        public virtual bool Save()
        {
            bool result = true;
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                result = false;
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                        Logger.GenerateError(e, typeof(GenericRepository<Object>), "State=" + eve.Entry.State + " | Type=" + eve.Entry.Entity.GetType().Name + "| PropertyName = " + ve.PropertyName + " | ErrorMessage =" + ve.ErrorMessage);
                    }
                }
            }
            catch (Exception e)
            {
                result = false;
                Logger.GenerateError(e,  typeof(GenericRepository<Object>));
            }
            return result;
        }


        public virtual bool ExecuteStoredProcedure(string ProcedureName, List<Tuple<string, object>> Parameters)
        {
            bool result = true;
            try
            {
                result = _context.ExecuteStoredProcedure(ProcedureName, Parameters);
            }
            catch (Exception e)
            {
                result = false;
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "ProcedureName : " + ProcedureName + " and Parameters :" + String.Join(", ", Parameters));
            }
            return result;
        }

        public virtual T Get(int id)
        {
            T m = null;
            try
            {
                m = _dbset.Find(id);
            }
            catch (Exception e)
            {
                Logger.GenerateError(e,  System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,"Parameter = " + id);
            }
            return m;
        }



        public virtual int Count()
        {
            int result = 0;
            try
            {
                result = _dbset.Count();
            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return result;
        }

        public virtual T Get(Guid id)
        {
            T m = null;
            try
            {
                m = _dbset.Find(id);
            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Parameter = " + id);
            }
            return m;
        }

        public virtual void Reload(T entity, string property)
        {
            try
            {
                _context.Entry(entity).Reference(property).Load();
            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Reload error, entity type = " + typeof(T).ToString() + " and property = " + property);
            }
        }

        public virtual bool HasModifications()
        {
            return _context.ChangeTracker.Entries<T>().Where(x => x.State == EntityState.Modified).Any();
        }
    }
}