using System;
using System.Collections.Generic;
using System.Text;
using DAL.Interfaces;

namespace DAL.TaisKoht.Interfaces.Helpers
{
    public interface IRepositoryFactory
    {
        Func<IDataContext, object> GetCustomRepositoryFactory<TRepoInterface>() where TRepoInterface : class;

        Func<IDataContext, object> GetStandardRepositoryFactory<TEntity>() where TEntity : class;

    }
}
