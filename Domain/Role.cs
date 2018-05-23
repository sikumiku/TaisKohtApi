using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Helpers;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class Role : IdentityRole<int>, IEssentialEntityBase
    {
        public int RoleId { get; set; }
        [Required]
        [MaxLength(50)]
        public string AccessLevel { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        public DateTime AddTime { get; set; } = DateTime.UtcNow;
        public DateTime UpdateTime { get; set; } = DateTime.UtcNow;
        public bool Active { get; set; } = true;
        public List<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}