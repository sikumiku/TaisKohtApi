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
        public List<SimpleDishDTO> Dishes { get; set; } = new List<SimpleDishDTO>();
        //foreign keys
        public int? PromotionId { get; set;}
        public PromotionDTO Promotion { get; set; }
        public AddressDTO Address { get; set; }
        public Rating Rating { get; set; }

        public static RestaurantDTO CreateFromDomain(Restaurant restaurant)
        {
            if (restaurant == null) { return null; }
            return new RestaurantDTO()
            {
                RestaurantId = restaurant.RestaurantId,
                Name = restaurant.Name,
                Url = restaurant.Url,
                ContactNumber = restaurant.ContactNumber,
                Email = restaurant.Email,
                Address = AddressDTO.CreateFromDomain(restaurant.Address),
                Promotion = PromotionDTO.CreateFromDomain(restaurant.Promotion),
                PromotionId = restaurant.PromotionId,
                Rating = restaurant.RatingLogs.Any() ? Rating.Create(restaurant.RatingLogs) : null
            };
        }

        public static RestaurantDTO CreateFromDomainWithAdditionalInfo(Restaurant r)
        {
            var restaurant = CreateFromDomain(r);
            if (restaurant == null) { return null; }

            restaurant.Menus = r.Menus.Select(SimpleMenuDTO.CreateFromDomain).ToList();
            restaurant.Dishes = r.Dishes.Select(SimpleDishDTO.CreateFromDomain).ToList();
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
            if (restaurant == null) { return null; }
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
        public PostMenuDTO Menu { get; set; }
        public int? PromotionId { get; set; }

        public static PostRestaurantDTO CreateFromDomain(Restaurant restaurant)
        {
            return new PostRestaurantDTO()
            {
                Name = restaurant.Name,
                Url = restaurant.Url,
                ContactNumber = restaurant.ContactNumber,
                Email = restaurant.Email,
                Address = AddressDTO.CreateFromDomain(restaurant.Address),
                PromotionId = restaurant.PromotionId
            };
        }
    }
}
