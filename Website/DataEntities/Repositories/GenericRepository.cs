using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Validation;
using DataEntities;
using DataEntities.Model;
using DataEntities.Logger;

namespace DataEntities.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected TemplateEntities _context;
        protected readonly IDbSet<T> _dbset;

        public GenericRepository(TemplateEntities context)
        {
            _context = context;
            _dbset = _context.Set<T>();
        }

        public TemplateEntities Context()
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
            try
            {
                IEnumerable<T> query = _dbset.Where(predicate).AsEnumerable();
                return query;
            }
            catch (Exception e)
            {
                Logger.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, null);
            }
            return new List<T>();
        }

        public virtual T Add(T entity)
        {
            try
            {
                return _dbset.Add(entity);
            }
            catch (Exception e)
            {
                Logger.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, null);
            }
            return null;
        }

        public virtual T Delete(T entity)
        {
            try
            {
                return _dbset.Remove(entity);
            }
            catch (Exception e)
            {
                Logger.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, null);
            }
            return null;
        }

        public virtual T Delete(int id)
        {
            try
            {
                return _dbset.Remove(_dbset.Find(id));
            }
            catch (Exception e)
            {
                Logger.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "id ="+ id);
            }
            return null;
        }

        public virtual T Delete(Guid id)
        {
            try
            {
                return _dbset.Remove(_dbset.Find(id));
            }
            catch (Exception e)
            {
                Logger.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "id = "+ id);
            }
            return null;
        }

        public virtual T Edit(T entity)
        {
            try
            {
                _context.Entry(entity).CurrentValues.SetValues(entity);
                return entity;
            }
            catch (Exception e)
            {
                if (entity != null)
                    Logger.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "State=" + entity.GetType());
                else
                    Logger.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "null object");
            }
            return null;
        }

        public virtual IEnumerable<T> Edit(IEnumerable<T> entityList)
        {
            try
            {
                foreach (T entity in entityList)
                {
                    _context.Entry(entity).CurrentValues.SetValues(entity);
                }
                return entityList;
            }
            catch (Exception e)
            {
                Logger.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, null);
            }
            return null;
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
                        Logger.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "State=" + eve.Entry.State + " | Type=" + eve.Entry.Entity.GetType().Name + "| PropertyName = " + ve.PropertyName + " | ErrorMessage =" + ve.ErrorMessage);
                    }
                }
            }
            catch (Exception e)
            {
                result = false;
                Logger.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, null);
            }
            return result;
        }


        public virtual bool ExecuteStoredProcedure(string ProcedureName, List<Tuple<string, object>> Parameters=null)
        {
            bool result = true;
            try
            {
                if(Parameters==null)
                {
                    Parameters = new List<Tuple<string, object>>();
                }

                result = _context.ExecuteStoredProcedure(ProcedureName, Parameters);
            }
            catch (Exception e)
            {
                result = false;
                Logger.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "ProcedureName : " + ProcedureName + " and Parameters = " + (Parameters != null ? string.Join(",", Parameters.Select(t => string.Format("[ '{0}', '{1}']", (t.Item1 ?? "NULL"), (t.Item2 ?? "NULL")))) : "NULL"));
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
                Logger.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Parameter = " + id);
            }
            return m;
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
                Logger.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Parameter = " + id);
            }
            return m;
        }

        public virtual void Reload(T entity, string property)
        {
            _context.Entry(entity).Reference(property).Load();
        }

        public virtual bool HasModifications()
        {
            return _context.ChangeTracker.Entries<T>().Where(x => x.State == EntityState.Modified).Any();
        }
    }
}
