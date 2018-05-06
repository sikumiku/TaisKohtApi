using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Domain;

namespace BusinessLogic.DTO
{
    public class RestaurantDTO
    {
        public int RestaurantId { get; set; }
        [MinLength(3)]
        [MaxLength(40)]
        public string Name { get; set; }
        public decimal LocationLongitude { get; set; }
        public decimal LocationLatitude { get; set; }
        [MinLength(5)]
        [MaxLength(100)]
        public string Url { get; set; }
        [MinLength(5)]
        [MaxLength(20)]
        public string ContactNumber { get; set; }
        [MinLength(10)]
        [MaxLength(40)]
        public string Email { get; set; }
        public DateTime AddTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Active { get; set; }
        //OneToOne
        public Address Address { get; set; }
        //OneToMany
        public List<Dish> Dishes { get; set; }
        public List<Menu> Menus { get; set; }
        public List<RestaurantUser> RestaurantUsers { get; set; }
        public List<RequestLog> RequestLogs { get; set; }

        public static RestaurantDTO CreateFromDomain(Restaurant restaurant)
        {
            if (restaurant == null) { return null; }
            return new RestaurantDTO()
            {
                RestaurantId = restaurant.RestaurantId,
                Name = restaurant.Name,
                LocationLongitude = restaurant.LocationLongitude,
                LocationLatitude = restaurant.LocationLatitude,
                Url = restaurant.Url,
                ContactNumber = restaurant.ContactNumber,
                Email = restaurant.Email,
                Address = restaurant.Address
            };
        }

        public static RestaurantDTO CreateFromDomainWithMenus(Restaurant restaurant)
        {
            throw new NotImplementedException();
        }

        public static RestaurantDTO CreateFromDomainWithDishes(Restaurant restaurant)
        {
            throw new NotImplementedException();
        }
    }
}
