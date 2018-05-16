using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class User : IdentityUser
    {
        public int UserId { get; set; }
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        [MaxLength(101)]
        public string FirstLastName => $"{FirstName} {LastName}";
        public DateTime AddTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Active { get; set; }

        //OneToMany
        public List<Menu> Menus { get; set; } = new List<Menu>();
        public List<Dish> Dishes { get; set; } = new List<Dish>();
        public List<RequestLog> RequestLogs { get; set; } = new List<RequestLog>();
        public List<RestaurantUser> RestaurantUsers { get; set; } = new List<RestaurantUser>();

        //foreign keys
        public int PromotionId { get; set; }
        public Promotion Promotion { get; set; }
        public int UserRoleId { get; set; }
        public UserRole UserRole { get; set; }
    }
}