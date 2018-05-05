using System;
using System.Collections.Generic;
using System.Text;
using DAL.Interfaces;
using DAL.Interfaces.Repositories;

namespace DAL.TaisKoht.Interfaces.Helpers
{
    public interface IRepositoryProvider
    {
        IRepository<TEntity> GetEntityRepository<TEntity>() where TEntity : class;

        TRepository GetCustomRepository<TRepository>() where TRepository : class;
    }
}
