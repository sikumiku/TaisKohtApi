using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Helpers;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class User : IdentityUser<string>, IEssentialEntityBase
    {
        [Required]
        [MaxLength(450)]
        public string UserId { get; set; }
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        [NotMapped]
        [MaxLength(101)]
        public string FirstLastName => $"{FirstName} {LastName}";
        public DateTime AddTime { get; set; } = DateTime.UtcNow;
        public DateTime UpdateTime { get; set; } = DateTime.UtcNow;
        public bool Active { get; set; } = true;
        //OneToMany
        public List<Menu> Menus { get; set; } = new List<Menu>();
        public List<Dish> Dishes { get; set; } = new List<Dish>();
        public List<RatingLog> RatingLogs { get; set; } = new List<RatingLog>();
        public List<RestaurantUser> RestaurantUsers { get; set; } = new List<RestaurantUser>();
        public List<Promotion> Promotions { get; set; } = new List<Promotion>();
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
    }
}