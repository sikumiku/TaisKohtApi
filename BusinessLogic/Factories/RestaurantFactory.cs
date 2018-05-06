using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic.DTO;
using Domain;

namespace BusinessLogic.Factories
{
    public interface IRestaurantFactory
    {
        RestaurantDTO Create(Restaurant promotion);
        Restaurant Create(RestaurantDTO restaurantDTO);
    }

    public class RestaurantFactory : IRestaurantFactory
    {
        public RestaurantDTO Create(Restaurant restaurant)
        {
            return RestaurantDTO.CreateFromDomain(restaurant);
        }

        public Restaurant Create(RestaurantDTO restaurantDTO)
        {
            return new Restaurant
            {
                RestaurantId = restaurantDTO.RestaurantId,
                Name = restaurantDTO.Name,
                LocationLongitude = restaurantDTO.LocationLongitude,
                LocationLatitude = restaurantDTO.LocationLatitude,
                Url = restaurantDTO.Url,
                ContactNumber = restaurantDTO.ContactNumber,
                Email = restaurantDTO.Email,
                Address = restaurantDTO.Address
            };
        }
    }

    //public class PromotionFactory : IPromotionFactory
    //{
    //    public PromotionDTO Create(Promotion promotion)
    //    {
    //        return PromotionDTO.CreateFromDomainWithAssociatedTables(promotion);
    //    }

    //    public Promotion Create(PromotionDTO promotionDTO)
    //    {
    //        return new Promotion
    //        {
    //            PromotionId = promotionDTO.PromotionId,
    //            Name = promotionDTO.Name,
    //            Description = promotionDTO.Description,
    //            Type = promotionDTO.Type,
    //            ValidTo = promotionDTO.ValidTo,
    //            AddTime = promotionDTO.AddTime,
    //            UpdateTime = promotionDTO.UpdateTime,
    //            Active = promotionDTO.Active
    //        };
    //    }
    //}
}
