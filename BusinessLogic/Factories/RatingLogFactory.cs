using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using BusinessLogic.DTO;
using Domain;

namespace BusinessLogic.Factories
{
    public interface IRatingLogFactory
    {
        RatingLogDTO Create(RatingLog ratingLog);
        RatingLog Create(RatingLogDTO ratingLogDTO);
    }
    class RatingLogFactory : IRatingLogFactory
    {
        public RatingLogDTO Create(RatingLog ratingLog)
        {
            return RatingLogDTO.CreateFromDomain(ratingLog);
        }

        public RatingLog Create(RatingLogDTO ratingLogDTO)
        {
            return new RatingLog
            {
                RatingLogId = ratingLogDTO.RatingLogId,
                Rating = ratingLogDTO.Rating,
                Comment = ratingLogDTO.Comment,
                RestaurantId = ratingLogDTO.RestaurantId,
                DishId = ratingLogDTO.DishId,
                UserId = ratingLogDTO.UserId
            };
        }
    }
}
