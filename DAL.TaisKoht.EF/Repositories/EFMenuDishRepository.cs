using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.EF;
using DAL.TaisKoht.Interfaces.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.TaisKoht.EF.Repositories
{
    class EFMenuDishRepository : EFRepository<MenuDish>, IMenuDishRepository
    {
        public EFMenuDishRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public bool Exists(int menuId)
        {
            return RepositoryDbSet.Any(e => e.MenuId == menuId);
        }

        public IEnumerable<MenuDish> FindByMenuId(params object[] id)
        {
            return RepositoryDbSet
                .Include(md => md.Dish)
                .Where(x => (int)id[0] == x.MenuId)
                .ToList();
        }
    }
}
