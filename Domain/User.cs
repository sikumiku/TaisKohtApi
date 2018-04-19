using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class User : IdentityUser
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime AddTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public string FirstLastName => $"{FirstName} {LastName}";
        //OneToMany
        public List<Menu> Menus { get; set; }
        public List<Dish> Dishes { get; set; }
        public List<RequestLog> RequestLogs { get; set; }
        public List<RestaurantUser> RestaurantUsers { get; set; }
        //foreign keys
        public int PromotionId { get; set; }
        public Promotion Promotion { get; set; }
        public int UserRoleId { get; set; }
        public UserRole UserRole { get; set; }
    }
}
