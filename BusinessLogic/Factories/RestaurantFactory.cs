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
        Restaurant Create(PostRestaurantDTO restaurantDTO);
        RestaurantDTO CreateComplex(Restaurant restaurant);
    }

    public class RestaurantFactory : IRestaurantFactory
    {
        public RestaurantDTO Create(Restaurant restaurant)
        {
            return RestaurantDTO.CreateFromDomain(restaurant);
        }

        public Restaurant Create(PostRestaurantDTO restaurantDTO)
        {
            return new Restaurant
            {
                Name = restaurantDTO.Name,
                Url = restaurantDTO.Url,
                ContactNumber = restaurantDTO.ContactNumber,
                Email = restaurantDTO.Email,
                Address = AddressDTO.CreateFromDTO(restaurantDTO.Address)
            };
        }

        public RestaurantDTO CreateComplex(Restaurant restaurant)
        {
            return RestaurantDTO.CreateFromDomainWithMenusAndRating(restaurant);
        }
    }
}

