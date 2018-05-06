using DAL.EF;
using DAL.TaisKoht.Interfaces.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.TaisKoht.EF.Repositories
{
    public class EFIngredientRepository : EFRepository<Ingredient>, IIngredientRepository
    {
        public EFIngredientRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public bool Exists(int id)
        {
            return RepositoryDbSet.Any(e => e.IngredientId == id);
        }

        public override Ingredient Find(params object[] id)
        {
            return RepositoryDbSet
                .SingleOrDefault(x => (int)id[0] == x.IngredientId);
        }

        public override IEnumerable<Ingredient> All()
        {
            return RepositoryDbSet.AsQueryable()
                .ToList();
        }
    }
}
