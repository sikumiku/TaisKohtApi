using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using BusinessLogic.Factories;
using BusinessLogic.Helpers;
using Domain;

namespace BusinessLogic.DTO
{
    public class RestaurantDTO
    {
        public int RestaurantId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(255)]
        public string Url { get; set; }
        [MaxLength(50)]
        public string ContactNumber { get; set; }
        [EmailAddress]
        [MaxLength(50)]
        public string Email { get; set; }
        //OneToMany
        public List<SimpleMenuDTO> Menus { get; set; } = new List<SimpleMenuDTO>();
        //foreign keys
        public PromotionDTO Promotion { get; set; }
        public AddressDTO Address { get; set; }
        public Rating Rating { get; set; }

        public static RestaurantDTO CreateFromDomain(Restaurant restaurant)
        {
            if (restaurant == null || !restaurant.Active) { return null; }
            return new RestaurantDTO()
            {
                RestaurantId = restaurant.RestaurantId,
                Name = restaurant.Name,
                Url = restaurant.Url,
                ContactNumber = restaurant.ContactNumber,
                Email = restaurant.Email,
                Address = AddressDTO.CreateFromDomain(restaurant.Address),
                Promotion = PromotionDTO.CreateFromDomain(restaurant.Promotion),
                Rating = restaurant.RatingLogs.Any() ? Rating.Create(restaurant.RatingLogs) : null            };
        }

        public static RestaurantDTO CreateFromDomainWithMenusAndRating(Restaurant r)
        {
            var restaurant = CreateFromDomain(r);
            if (restaurant == null) { return null; }

            restaurant.Menus = r.Menus.Select(SimpleMenuDTO.CreateFromDomain).ToList();
            restaurant.Rating = r.RatingLogs.Any() ? Rating.CreateWithComments(r.RatingLogs) : null;
            return restaurant;
        }
    }

    public class SimpleRestaurantDTO
    {
        public int RestaurantId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(255)]
        public string Url { get; set; }
        [MaxLength(50)]
        public string ContactNumber { get; set; }
        [EmailAddress]
        [MaxLength(50)]
        public string Email { get; set; }
        public AddressDTO Address { get; set; }
        public Rating Rating { get; set; }

        public static SimpleRestaurantDTO CreateFromDomain(Restaurant restaurant)
        {
            if (restaurant == null || !restaurant.Active) { return null; }
            return new SimpleRestaurantDTO()
            {
                RestaurantId = restaurant.RestaurantId,
                Name = restaurant.Name,
                Url = restaurant.Url,
                ContactNumber = restaurant.ContactNumber,
                Email = restaurant.Email,
                Address = AddressDTO.CreateFromDomain(restaurant.Address),
                Rating = restaurant.RatingLogs.Any() ? Rating.Create(restaurant.RatingLogs) : null
            };
        }
    }

    public class PostRestaurantDTO
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(255)]
        public string Url { get; set; }
        [MaxLength(50)]
        public string ContactNumber { get; set; }
        [EmailAddress]
        [MaxLength(50)]
        public string Email { get; set; }
        public AddressDTO Address { get; set; }
        public string UserId { get; set; }

        public static PostRestaurantDTO CreateFromDomain(Restaurant restaurant)
        {
            return new PostRestaurantDTO()
            {
                Name = restaurant.Name,
                Url = restaurant.Url,
                ContactNumber = restaurant.ContactNumber,
                Email = restaurant.Email,
                Address = AddressDTO.CreateFromDomain(restaurant.Address)
            };
        }
    }
}
