using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class User : IdentityUser<int>
    {
        public int UserId { get; set; }
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
        //foreign keys
        public int? PromotionId { get; set; }
        public Promotion Promotion { get; set; }
        public int? UserRoleId { get; set; }
        public UserRole UserRole { get; set; }
    }
}