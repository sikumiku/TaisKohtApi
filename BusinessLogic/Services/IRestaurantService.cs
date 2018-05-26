using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic.DTO;

namespace BusinessLogic.Services
{
    public interface IRestaurantService
    {
        IEnumerable<RestaurantDTO> GetAllRestaurants();
        RestaurantDTO GetRestaurantById(int id);
        void AddUserToRestaurant(int id, string userId);
        RestaurantDTO AddNewRestaurant(PostRestaurantDTO dto);
        void UpdateRestaurant(int id, PostRestaurantDTO dto);
        void DeleteRestaurant(int id);
        IEnumerable<RestaurantDTO> SearchRestaurantByName(string restaurantName);
        IEnumerable<RestaurantDTO> GetTopRestaurants(int amount);
        List<UserDTO> GetRestaurantUsersById(int id);
    }
}
