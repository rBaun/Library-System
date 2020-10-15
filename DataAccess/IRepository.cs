using System;
using System.Data;

namespace DataAccess
{
    public interface IRepository<TEntity>
    {
        Func<IDbConnection> Connection { get; set; }

        TEntity Create(TEntity entity);
        TEntity Get(TEntity entity);
        TEntity Update(TEntity entity);
        TEntity Delete(TEntity entity);
    }
}
