using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Domain.Helpers;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class UserRole : IdentityUserRole<string>, IEssentialEntityBase
    {
        [Required]
        [MaxLength(450)]
        public string UserRoleId { get; set; }
        public DateTime AddTime { get; set; } = DateTime.UtcNow;
        public DateTime UpdateTime { get; set; } = DateTime.UtcNow;
        public bool Active { get; set; } = true;
        [Required]
        public override string UserId { get; set; }
        public User User { get; set; }
        [Required]
        public override string RoleId { get; set; }
        public Role Role { get; set; }
    }
}
