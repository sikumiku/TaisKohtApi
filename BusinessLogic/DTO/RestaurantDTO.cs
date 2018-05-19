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
        public List<MenuDTO> MenusDTOs { get; set; } = new List<MenuDTO>();
        public List<RatingLog> RatingLogs { get; set; } = new List<RatingLog>();
        //foreign keys
        public int? PromotionId { get; set; }
        public Promotion Promotion { get; set; }
        public int? AddressId { get; set; }
        public Address Address { get; set; }

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
                Address = restaurant.Address,
                Promotion = restaurant.Promotion
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
