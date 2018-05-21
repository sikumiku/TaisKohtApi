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
    public class EFRestaurantUserRepository : EFRepository<RestaurantUser>, IRestaurantUserRepository
    {
        public EFRestaurantUserRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public bool Exists(int id)
        {
            return RepositoryDbSet.Any(e => e.RestaurantUserId == id);
        }

        public IEnumerable<RestaurantUser> FindAll(params object[] restaurantId)
        {
            return RepositoryDbSet.AsQueryable().ToList().Where(r => r.RestaurantId == (int)restaurantId[0]);
        }
    }
}
