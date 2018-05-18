using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Domain.Helpers;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class UserRole : IdentityRole
    {
        public int UserRoleId { get; set; }
        [Required]
        [MaxLength(50)]
        public string AccessLevel { get; set; }
        public DateTime AddTime { get; set; } = DateTime.UtcNow;
        public DateTime UpdateTime { get; set; } = DateTime.UtcNow;
        public bool Active { get; set; } = true;
        //OneToMany
        public List<User> Users { get; set; } = new List<User>();
        public List<RestaurantUser> RestaurantUsers { get; set; } = new List<RestaurantUser>();
    }
}
