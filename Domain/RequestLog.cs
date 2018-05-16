using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class RequestLog
    {
        public int RequestLogId { get; set; }
        public DateTime AddTime { get; set; }
        public string Query { get; set; }

        //foreign keys
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
        public int DishId { get; set; }
        public Dish Dish { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
