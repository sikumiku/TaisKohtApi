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
    public class EFMenuRepository : EFRepository<Menu>, IMenuRepository
    {
        public EFMenuRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public bool Exists(int id)
        {
            return RepositoryDbSet.Any(e => e.MenuId == id);
        }

        public override Menu Find(params object[] id)
        {
            return RepositoryDbSet
                .Include(m => m.MenuDishes)
                    .ThenInclude(md => md.Dish)
                .SingleOrDefault(x => (int)id[0] == x.MenuId);
        }

        public override IEnumerable<Menu> All()
        {
            return RepositoryDbSet.AsQueryable()
                .ToList();
        }
    }
}
