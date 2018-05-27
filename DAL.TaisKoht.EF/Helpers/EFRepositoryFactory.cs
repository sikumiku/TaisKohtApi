using System;
using System.Collections.Generic;
using System.Text;
using DAL.EF;
using DAL.Interfaces;
using DAL.TaisKoht.EF.Repositories;
using DAL.TaisKoht.Interfaces.Helpers;
using DAL.TaisKoht.Interfaces.Repositories;

namespace DAL.TaisKoht.EF.Helpers
{
    public class EFRepositoryFactory : IRepositoryFactory
    {
        private readonly Dictionary<Type, Func<IDataContext, object>> _customRepositoryFactories
            = GetCustomRepoFactories();

        private static Dictionary<Type, Func<IDataContext, object>> GetCustomRepoFactories()
        {
            return new Dictionary<Type, Func<IDataContext, object>>()
            {
                {typeof(IDishRepository), (dataContext) => new EFDishRepository(dataContext as ApplicationDbContext) },
                {typeof(IIngredientRepository), (dataContext) => new EFIngredientRepository(dataContext as ApplicationDbContext) },
                {typeof(IMenuRepository), (dataContext) => new EFMenuRepository(dataContext as ApplicationDbContext) },
                {typeof(IPromotionRepository), (dataContext) => new EFPromotionRepository(dataContext as ApplicationDbContext) },
                {typeof(IRestaurantRepository), (dataContext) => new EFRestaurantRepository(dataContext as ApplicationDbContext) },
                {typeof(IRestaurantUserRepository), (dataContext) => new EFRestaurantUserRepository(dataContext as ApplicationDbContext) },
                {typeof(IUserRepository), (dataContext) => new EFUserRepository(dataContext as ApplicationDbContext) },
                {typeof(IRatingLogRepository), (dataContext) => new EFRatingLogRepository(dataContext as ApplicationDbContext) },
                {typeof(IUserRoleRepository), (dataContext) => new EFUserRoleRepository(dataContext as ApplicationDbContext) },
                {typeof(IRoleRepository), (dataContext) => new EFRoleRepository(dataContext as ApplicationDbContext) },
                {typeof(IMenuDishRepository), (dataContext) => new EFMenuDishRepository(dataContext as ApplicationDbContext) },
                {typeof(IRequestLogRepository), (dataContext) => new EFRequestLogRepository(dataContext as ApplicationDbContext) },
                {typeof(IDishIngredientRepository), (dataContext) => new EFDishIngredientRepository(dataContext as ApplicationDbContext) }
            };
        }

        public Func<IDataContext, object> GetCustomRepositoryFactory<TRepoInterface>() where TRepoInterface : class
        {
            _customRepositoryFactories.TryGetValue(
                typeof(TRepoInterface),
                out Func<IDataContext, object> factory
            );
            return factory;
        }

        public Func<IDataContext, object> GetStandardRepositoryFactory<TEntity>() where TEntity : class
        {

            return (dataContext) => new EFRepository<TEntity>(dataContext as ApplicationDbContext);
        }
    }
}
