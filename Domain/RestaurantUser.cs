using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Domain.Helpers;

namespace Domain
{
    public class RestaurantUser : EssentialEntityBase
    {
        public int RestaurantUserId { get; set; }
        public DateTime? StartedAt { get; set; }
        //foreign keys
        [Required]
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
        [Required]
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
