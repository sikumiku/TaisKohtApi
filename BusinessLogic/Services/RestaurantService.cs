using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessLogic.DTO;
using BusinessLogic.Factories;
using DAL.TaisKoht.Interfaces;
using Domain;

namespace BusinessLogic.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly ITaisKohtUnitOfWork _uow;
        private readonly IRestaurantFactory _restaurantFactory;

        public RestaurantService(ITaisKohtUnitOfWork uow, IRestaurantFactory restaurantFactory)
        {
            _uow = uow;
            _restaurantFactory = restaurantFactory;
        }

        public RestaurantDTO AddNewRestaurant(RestaurantDTO restaurantDTO)
        {
            var newRestaurant = _restaurantFactory.Create(restaurantDTO);
            _uow.Restaurants.Add(newRestaurant);
            _uow.SaveChanges();
            return _restaurantFactory.Create(newRestaurant);
        }

        public IEnumerable<RestaurantDTO> GetAllRestaurants()
        {
            return _uow.Restaurants.All()
                .Select(restaurant => _restaurantFactory.Create(restaurant));
        }

        public RestaurantDTO GetRestaurantById(int id)
        {
            var restaurant = _uow.Restaurants.Find(id);
            if (restaurant == null) return null;

            return _restaurantFactory.Create(restaurant);
        }

        public void UpdateRestaurant(int id, RestaurantDTO updatedRestaurantDTO)
        {
            Restaurant restaurant = _uow.Restaurants.Find(id);
            //Id'd ei peaks vist muuta saama? See tekib automaatselt andmebaasis.
            restaurant.RestaurantId = updatedRestaurantDTO.RestaurantId;
            restaurant.Name = updatedRestaurantDTO.Name;
            restaurant.Url = updatedRestaurantDTO.Url;
            restaurant.ContactNumber = updatedRestaurantDTO.ContactNumber;
            restaurant.Email = updatedRestaurantDTO.Email;
            //restaurant.Address = updatedRestaurantDTO.Address;
            _uow.Restaurants.Update(restaurant);
            _uow.SaveChanges();
        }

        public void DeleteRestaurant(int id)
        {
            Restaurant restaurant = _uow.Restaurants.Find(id);
            _uow.Restaurants.Remove(restaurant);
            _uow.SaveChanges();
        }
    }
}
