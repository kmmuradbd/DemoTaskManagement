using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DemoTask.Core
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(int id);
        TEntity Get(int id);
        IQueryable<TEntity> GetAll();
        TEntity Get(Expression<Func<TEntity, bool>> expression);
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression);
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression, string include);
        bool GetAny(Expression<Func<TEntity, bool>> expression);
        int ExecuteSqlCommand(string sql, params object[] parameters);
        IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters);
        int Count();
        int Count(Expression<Func<TEntity, bool>> expression);
        int GetAutoNumber();

    }
}
