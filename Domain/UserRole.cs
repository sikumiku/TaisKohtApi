using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class UserRole
    {
        public int UserRoleId { get; set; }
        public string RoleName { get; set; }
        public string AccessLevel { get; set; }
        public DateTime AddTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Active { get; set; }
        //OneToMany
        public List<User> Users { get; set; }
        public List<RestaurantUser> RestaurantUsers { get; set; }
    }
}
