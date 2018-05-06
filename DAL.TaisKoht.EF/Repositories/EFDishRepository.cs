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
    public class EFDishRepository : EFRepository<Dish>, IDishRepository
    {
        public EFDishRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public bool Exists(int id)
        {
            return RepositoryDbSet.Any(e => e.DishId == id);
        }

        public override Dish Find(params object[] id)
        {
            return RepositoryDbSet
                .SingleOrDefault(x => (int)id[0] == x.DishId);
        }

        public override IEnumerable<Dish> All()
        {
            return RepositoryDbSet.AsQueryable()
                .ToList();
        }
    }
}
