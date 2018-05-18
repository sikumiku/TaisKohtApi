using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Domain.Helpers;

namespace Domain
{
    public class Restaurant : EssentialEntityBase
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
        public List<Dish> Dishes { get; set; } = new List<Dish>();
        public List<Menu> Menus { get; set; } = new List<Menu>();
        public List<RestaurantUser> RestaurantUsers { get; set; } = new List<RestaurantUser>();
        public List<RatingLog> RatingLogs { get; set; } = new List<RatingLog>();
        //foreign keys
        public int? PromotionId { get; set; }
        public Promotion Promotion { get; set; }
        public int? AddressId { get; set; }
        public Address Address { get; set; }
    }
}
