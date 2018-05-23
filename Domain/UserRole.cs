using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Domain.Helpers;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class UserRole : IdentityUserRole<int>, IEssentialEntityBase
    {
        public int UserRoleId { get; set; }
        public DateTime AddTime { get; set; } = DateTime.UtcNow;
        public DateTime UpdateTime { get; set; } = DateTime.UtcNow;
        public bool Active { get; set; } = true;
        [Required]
        public override int UserId { get; set; }
        public User User { get; set; }
        [Required]
        public override int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
