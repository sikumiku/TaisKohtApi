using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Domain.Helpers;

namespace Domain
{
    public class RatingLog : EssentialEntityBase
    {
        public int RatingLogId { get; set; }
        [Range(0, 10)]
        public int Rating { get; set; }
        [MaxLength(2000)]
        public string Comment { get; set; }
        //foreign keys
        public int? RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
        public int? MenuId { get; set; }
        public Menu Menu { get; set; }
        public int? DishId { get; set; }
        public Dish Dish { get; set; }
        [Required]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
