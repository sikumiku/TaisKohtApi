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
            return _uow.Restaurants.All().Where(x => x.Active)
                .Select(restaurant => _restaurantFactory.Create(restaurant));
        }

        public RestaurantDTO GetRestaurantById(int id)
        {
            var restaurant = _uow.Restaurants.Find(id);
            if (restaurant == null || !restaurant.Active) return null;

            return _restaurantFactory.CreateComplex(restaurant);
        }
        public List<UserDTO> GetRestaurantUsersById(int restaurantId)
        {
            var restaurant = _uow.Restaurants.Find(restaurantId);
            var restaurantUsers = _uow.RestaurantUsers.FindAll(restaurant.RestaurantId);
            if (restaurantUsers == null || !restaurant.Active) return null;
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

        public void UpdateRestaurant(int id, PostRestaurantDTO updatedRestaurantDTO)
        {
            Restaurant restaurant = _uow.Restaurants.Find(id);
            restaurant.Name = updatedRestaurantDTO.Name;
            restaurant.Url = updatedRestaurantDTO.Url;
            restaurant.ContactNumber = updatedRestaurantDTO.ContactNumber;
            restaurant.Email = updatedRestaurantDTO.Email;
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

            var restaurants = _uow.Restaurants.All().Where(x => x.Active).OrderByDescending(x => x.RatingLogs.Select(r => r.Rating))
                .Select(restaurant => _restaurantFactory.Create(restaurant));

            if(restaurants.Count() < amount) return null;

            return restaurants.Take(amount);
        }

        public int GetUserRestaurantCount(string userId)
        {
            return _uow.RestaurantUsers.GetUserRestaurantCount(userId);
        }
    }
}
