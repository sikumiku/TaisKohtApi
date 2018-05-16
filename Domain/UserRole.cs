using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Domain.Helpers;

namespace Domain
{
    public class UserRole : EssentialEntityBase
    {
        public int UserRoleId { get; set; }
        [MaxLength(50)]
        [Required]
        public string RoleName { get; set; }
        [Required]
        [MaxLength(50)]
        public string AccessLevel { get; set; }
        //OneToMany
        public List<User> Users { get; set; } = new List<User>();
        public List<RestaurantUser> RestaurantUsers { get; set; } = new List<RestaurantUser>();
    }
}
