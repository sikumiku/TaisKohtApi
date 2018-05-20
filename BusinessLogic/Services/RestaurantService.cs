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
            return _restaurantFactory.CreateComplex(newRestaurant);
        }

        public IEnumerable<RestaurantDTO> GetAllRestaurants()
        {
            return _uow.Restaurants.All().Where(x => x.Active)
                .Select(restaurant => _restaurantFactory.Create(restaurant));
        }

        public RestaurantDTO GetRestaurantById(int id)
        {
            var restaurant = _uow.Restaurants.Find(id);
            if (restaurant == null || restaurant.Active) return null;

            return _restaurantFactory.CreateComplex(restaurant);
        }

        public void UpdateRestaurant(int id, RestaurantDTO updatedRestaurantDTO)
        {
            Restaurant restaurant = _uow.Restaurants.Find(id);
            restaurant.RestaurantId = updatedRestaurantDTO.RestaurantId;
            restaurant.Name = updatedRestaurantDTO.Name;
            restaurant.Url = updatedRestaurantDTO.Url;
            restaurant.ContactNumber = updatedRestaurantDTO.ContactNumber;
            restaurant.Email = updatedRestaurantDTO.Email;
            //restaurant.Address = updatedRestaurantDTO.Address;
            restaurant.UpdateTime = DateTime.UtcNow;
            _uow.Restaurants.Update(restaurant);
            _uow.SaveChanges();
        }

        public void DeleteRestaurant(int id)
        {
            Restaurant restaurant = _uow.Restaurants.Find(id);
            restaurant.UpdateTime = DateTime.UtcNow;
            restaurant.Active = false;
            _uow.Restaurants.Update(restaurant);
            _uow.SaveChanges();
        }
    }
}
