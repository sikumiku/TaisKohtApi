﻿using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic.DTO;

namespace BusinessLogic.Services
{
    public interface IRestaurantService
    {
        IEnumerable<SimpleRestaurantDTO> GetAllRestaurants();
        RestaurantDTO GetRestaurantById(int id);
        int GetUserRestaurantCount(string userId);
        void AddUserToRestaurant(int id, string userId);
        RestaurantDTO AddNewRestaurant(PostRestaurantDTO dto, string userId);
        void UpdateRestaurant(int id, PostRestaurantDTO dto);
        void DeleteRestaurant(int id);
        IEnumerable<SimpleRestaurantDTO> SearchRestaurantByName(string restaurantName);
        IEnumerable<SimpleRestaurantDTO> GetTopRestaurants(int amount);
        List<UserDTO> GetRestaurantUsersById(int id);
    }
}
