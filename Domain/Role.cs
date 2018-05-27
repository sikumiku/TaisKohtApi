using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Helpers;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class Role : IdentityRole<string>, IEssentialEntityBase
    {
        public Role()
        {
            Id = Guid.NewGuid().ToString();
        }
        [MaxLength(50)]
        public string AccessLevel { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        public DateTime AddTime { get; set; } = DateTime.UtcNow;
        public DateTime UpdateTime { get; set; } = DateTime.UtcNow;
    }
}