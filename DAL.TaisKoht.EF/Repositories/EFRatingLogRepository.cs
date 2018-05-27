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
    public class EFRatingLogRepository : EFRepository<RatingLog>, IRatingLogRepository
    {
        public EFRatingLogRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public bool Exists(int id)
        {
            return RepositoryDbSet.Any(e => e.RatingLogId == id);
        }

        public RatingLog FindUsersRestaurantRating(int? restaurantId, string userId)
        {
            return RepositoryDbSet.SingleOrDefault(log => log.RestaurantId == restaurantId && log.UserId == userId);
        }

        public RatingLog FindUsersDishRating(int? dishId, string userId)
        {
            return RepositoryDbSet.SingleOrDefault(log => log.DishId == dishId && log.UserId == userId);
        }

        public override RatingLog Find(params object[] id)
        {
            return RepositoryDbSet
                .SingleOrDefault(x => (int)id[0] == x.RatingLogId);
        }

        public override IEnumerable<RatingLog> All()
        {
            return RepositoryDbSet.AsQueryable()
                .ToList();
        }
    }
}