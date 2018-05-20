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
        RestaurantDTO CreateComplex(Restaurant restaurant);
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
                Url = restaurantDTO.Url,
                ContactNumber = restaurantDTO.ContactNumber,
                Email = restaurantDTO.Email
            };
        }

        public RestaurantDTO CreateComplex(Restaurant restaurant)
        {
            return RestaurantDTO.CreateFromDomainWithMenus(restaurant);
        }
    }
}

