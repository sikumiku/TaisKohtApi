using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessLogic.DTO;
using BusinessLogic.Factories;
using DAL.TaisKoht.Interfaces;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace BusinessLogic.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly ITaisKohtUnitOfWork _uow;
        private readonly IRestaurantFactory _restaurantFactory;
        private readonly IUserFactory _userFactory;
        private readonly UserManager<User> _userManager;

        public RestaurantService(ITaisKohtUnitOfWork uow, IRestaurantFactory restaurantFactory, IUserFactory userFactory, UserManager<User> userManager)
        {
            _uow = uow;
            _restaurantFactory = restaurantFactory;
            _userFactory = userFactory;
            _userManager = userManager;
        }

        public RestaurantDTO AddNewRestaurant(PostRestaurantDTO restaurantDTO, string userId)
        {
            var newRestaurant = _restaurantFactory.Create(restaurantDTO);
            _uow.Restaurants.Add(newRestaurant);
            _uow.RestaurantUsers.Add(new RestaurantUser{ RestaurantId = newRestaurant.RestaurantId, UserId = userId });
            _uow.SaveChanges();
            return _restaurantFactory.CreateComplex(newRestaurant);
        }

        public void AddUserToRestaurant(int id, string userId)
        {
            _uow.RestaurantUsers.Add(new RestaurantUser {RestaurantId = id, UserId = userId});
            _uow.SaveChanges();
        }

        public IEnumerable<SimpleRestaurantDTO> GetAllRestaurants()
        {
            return _uow.Restaurants.All()
                .Select(restaurant => _restaurantFactory.Create(restaurant));
        }

        public RestaurantDTO GetRestaurantById(int id)
        {
            var restaurant = _uow.Restaurants.Find(id);
            if (restaurant == null) return null;

            return _restaurantFactory.CreateComplex(restaurant);
        }
        public List<UserDTO> GetRestaurantUsersById(int restaurantId)
        {
            var restaurant = _uow.Restaurants.Find(restaurantId);
            var restaurantUsers = _uow.RestaurantUsers.FindAllByRestaurantId(restaurant.RestaurantId);
            if (restaurantUsers == null) return null;
            List<UserDTO> users = new List<UserDTO>();
            foreach (var restaurantUser in restaurantUsers)
            {
                var user = _uow.Users.Find(restaurantUser.UserId);
                if (user != null)
                {
                    var userObject = _userFactory.Create(user);
                    users.Add(userObject);
                }
            }
            return users;
        }

        public RestaurantDTO UpdateRestaurant(int id, PostRestaurantDTO updatedRestaurantDTO)
        {
            if (_uow.Restaurants.Exists(id))
            {
                Restaurant restaurant = _uow.Restaurants.Find(id);
                restaurant.Name = updatedRestaurantDTO.Name;
                restaurant.Url = updatedRestaurantDTO.Url;
                restaurant.ContactNumber = updatedRestaurantDTO.ContactNumber;
                restaurant.Email = updatedRestaurantDTO.Email;
                _uow.Restaurants.Update(restaurant);
                _uow.SaveChanges();
            }

            return GetRestaurantById(id);
        }

        public void DeleteRestaurant(int id)
        {
            Restaurant restaurant = _uow.Restaurants.Find(id);
            _uow.Restaurants.Remove(restaurant);
            _uow.SaveChanges();
        }

        public IEnumerable<SimpleRestaurantDTO> SearchRestaurantByName(string restaurantName)
        {

            if (String.IsNullOrEmpty(restaurantName)) return null;
            
            return _uow.Restaurants.All().Where(x => x.Name.Contains(restaurantName))
                .Select(restaurant => _restaurantFactory.Create(restaurant));
        }

        public IEnumerable<SimpleRestaurantDTO> GetTopRestaurants(int amount)
        {
            int topAmount;
            if (!int.TryParse(amount.ToString(), out topAmount)) return null;

            var restaurants = _uow.Restaurants.All().OrderByDescending(x => x.RatingLogs.Select(r => r.Rating))
                .Select(restaurant => _restaurantFactory.Create(restaurant));

            if(restaurants.Count() < amount) return null;

            return restaurants.Take(amount);
        }
    }
}
