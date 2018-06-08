using System.Collections.Generic;
using DAL.Interfaces.Repositories;
using Domain;

namespace DAL.TaisKoht.Interfaces.Repositories
{
    public interface IMenuDishRepository : IRepository<MenuDish>
    {
        /// <summary>
        /// Check for entity existance by MenuId value
        /// </summary>
        /// <param name="id">Dish MenuId value</param>
        /// <returns></returns>
        bool Exists(int id);

        IEnumerable<MenuDish> FindByMenuId(params object[] id);
    }
}