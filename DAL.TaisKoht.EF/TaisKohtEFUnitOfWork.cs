using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;
using DAL.Interfaces.Repositories;
using DAL.TaisKoht.Interfaces;
using DAL.TaisKoht.Interfaces.Helpers;
using DAL.TaisKoht.Interfaces.Repositories;

namespace DAL.TaisKoht.EF
{
    public class TaisKohtEFUnitOfWork : ITaisKohtUnitOfWork
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IRepositoryProvider _repositoryProvider;

        public TaisKohtEFUnitOfWork(IDataContext dataContext, IRepositoryProvider repositoryProvider)
        {
            _repositoryProvider = repositoryProvider;
            _applicationDbContext = dataContext as ApplicationDbContext;
            if (_applicationDbContext == null)
            {
                throw new NullReferenceException("No EF dbcontext found in UOW");
            }
        }

        public IDishRepository Dishes => GetCustomRepository<IDishRepository>();
        public IMenuDishRepository MenuDishes => GetCustomRepository<IMenuDishRepository>();
        public IIngredientRepository Ingredients => GetCustomRepository<IIngredientRepository>();
        public IMenuRepository Menus => GetCustomRepository<IMenuRepository>();
        public IPromotionRepository Promotions => GetCustomRepository<IPromotionRepository>();
        public IRestaurantRepository Restaurants => GetCustomRepository<IRestaurantRepository>();
        public IRatingLogRepository RatingLogs => GetCustomRepository<IRatingLogRepository>();
        public IRestaurantUserRepository RestaurantUsers => GetCustomRepository<IRestaurantUserRepository>();
        public IUserRepository Users => GetCustomRepository<IUserRepository>();
        public IUserRoleRepository UserRoles => GetCustomRepository<IUserRoleRepository>();
        public IRoleRepository Roles => GetCustomRepository<IRoleRepository>();

        public void SaveChanges()
        {
            _applicationDbContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _applicationDbContext.SaveChangesAsync();
        }

        public IRepository<TEntity> GetEntityRepository<TEntity>() where TEntity : class
        {
            return _repositoryProvider.GetEntityRepository<TEntity>();
        }

        public TRepositoryInterface GetCustomRepository<TRepositoryInterface>() where TRepositoryInterface : class
        {
            return _repositoryProvider.GetCustomRepository<TRepositoryInterface>();
        }
    }
}
