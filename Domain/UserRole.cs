using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
    public class UserRole
    {
        public int UserRoleId { get; set; }
        [MaxLength(50)]
        public string RoleName { get; set; }
        [MaxLength(50)]
        public string AccessLevel { get; set; }
        public DateTime AddTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Active { get; set; }
        
        //OneToMany
        public List<User> Users { get; set; } = new List<User>();
        public List<RestaurantUser> RestaurantUsers { get; set; } = new List<RestaurantUser>();
    }
}
