using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class RestaurantUser
    {
        public int RestaurantUserId { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime AddTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Active { get; set; }
        //foreign keys
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int DishId { get; set; }
        public Dish Dish { get; set; }
    }
}
