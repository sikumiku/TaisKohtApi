using System;
using DAL.Interfaces;
using DAL.TaisKoht.Interfaces.Repositories;

namespace DAL.TaisKoht.Interfaces
{
    public interface ITaisKohtUnitOfWork : IUnitOfWork
    {
        IDishRepository Dishes { get; }
        IMenuDishRepository MenuDishes { get; }
        IIngredientRepository Ingredients { get; }
        IMenuRepository Menus { get; }
        IPromotionRepository Promotions { get; }
        IRestaurantRepository Restaurants { get; }
        IRatingLogRepository RatingLogs { get; }
        IRestaurantUserRepository RestaurantUsers { get; }
        IUserRepository Users { get; }
        IUserRoleRepository UserRoles { get; }
        IRoleRepository Roles { get; }
    }
    
}
