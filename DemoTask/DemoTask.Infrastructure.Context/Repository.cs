using DemoTask.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DemoTask.Infrastructure.Context
{
    public class Repository<TEntity> : IRepository<TEntity>
       where TEntity : Entity
    {
        protected DemoTaskContext Context;
        public Repository(DemoTaskContext dbContext)
        {
            Context = dbContext;
        }
        public void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
            Context.SaveChanges();
        }
        public virtual void Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity is null");
            if (Context.Set<TEntity>().Local.Count > 0)
            {
                var key = GetEntityKeyValue(entity);
                var currentEntity = Find(key.ToArray());
                this.Context.Entry<TEntity>(currentEntity).CurrentValues.SetValues(entity);
            }
            else
            {
                this.Context.Entry<TEntity>(entity).State = EntityState.Modified;
            }
            this.Context.SaveChanges();
        }
        private IEnumerable<object> GetEntityKeyValue(TEntity entity)
        {
            return GetEntityKeyName().Select(r => r.GetValue(entity, null));
        }
        private IEnumerable<PropertyInfo> GetEntityKeyName()
        {
            var type = typeof(TEntity);
            var entityType = Context.Model.FindEntityType(type);
            var keyProperties = entityType.FindPrimaryKey().Properties;

            return keyProperties.Select(key => type.GetProperty(key.Name));
        }
        public virtual TEntity Find(params object[] keyValues)
        {
            return Context.Set<TEntity>().Find(keyValues);
        }

        public void Delete(int id)
        {
            var entity = Context.Set<TEntity>().Find(id);
            Context.Set<TEntity>().Remove(entity);
            Context.SaveChanges();
        }

        public TEntity Get(int id)
        {
            return Context.Set<TEntity>().Find(id);
        }

        public TEntity Get(Expression<Func<TEntity, bool>> expression)
        {
            TEntity rValue = this.Context.Set<TEntity>().Where(expression).FirstOrDefault();
            if (rValue == null)
            {
                rValue = Activator.CreateInstance<TEntity>();
            }
            return rValue;
        }

        public IQueryable<TEntity> GetAll()
        {
            return Context.Set<TEntity>().AsNoTracking();
        }
        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression)
        {
            return this.Context.Set<TEntity>().Where(expression);
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression, string include)
        {
            return this.Context.Set<TEntity>().Where(expression).Include(include);
        }
        public bool GetAny(Expression<Func<TEntity, bool>> expression)
        {
            return this.Context.Set<TEntity>().Any(expression);
        }
        public int Count()
        {
            return Count(x => true);
        }

        public int Count(Expression<Func<TEntity, bool>> expression)
        {
            return this.Context.Set<TEntity>().Count(expression);
        }
        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return Context.Database.ExecuteSqlRaw(sql, parameters);
        }
        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            return (IEnumerable<TElement>)Context.Set<TEntity>().FromSqlRaw(sql, parameters).AsEnumerable();
        }

        public int GetAutoNumber()
        {

            try
            {
                var maxId = Context.Set<TEntity>().OrderByDescending(e => e.Id).Select(e => e.Id).FirstOrDefault();

                return maxId + 1;
            }
            catch (Exception)
            {
                return 1;
            }

        }
        public string GetTableName<T>() where T : class
        {
            var entityType = Context.Model.FindEntityType(typeof(T));
            if (entityType == null || entityType.GetTableName() == null)
            {
                return null;
            }
            return entityType.GetTableName();
        }



        public void Dispose()
        {
            this.Context.Dispose();
        }
    }
}
