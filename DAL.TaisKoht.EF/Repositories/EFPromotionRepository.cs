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
    public class EFPromotionRepository : EFRepository<Promotion>, IPromotionRepository
    {
        public EFPromotionRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public bool Exists(int id)
        {
            return RepositoryDbSet.Any(e => e.PromotionId == id);
        }

        public override Promotion Find(params object[] id)
        {
            return RepositoryDbSet
                //.Include(y => y.Menus)
                //.Include(x => x.Dishes)
                .SingleOrDefault(x => (int)id[0] == x.PromotionId);
        }

        public override IEnumerable<Promotion> All()
        {
            return RepositoryDbSet.AsQueryable()
                //.Include(x => x.Menus)
                //.ThenInclude(y => y.Dishes)
                .ToList();
        }
    }
}
