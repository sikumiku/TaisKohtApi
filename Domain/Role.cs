using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class Role : IdentityRole<int>
    {
        public int RoleId { get; set; }
        [Required]
        [MaxLength(50)]
        public string AccessLevel { get; set; }
        public DateTime AddTime { get; set; } = DateTime.UtcNow;
        public DateTime UpdateTime { get; set; } = DateTime.UtcNow;
        public bool Active { get; set; } = true;
        public int? UserRoleId { get; set; }
        public UserRole UserRole { get; set; }
    }
}