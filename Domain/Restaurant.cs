using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Restaurant
    {
        public int RestaurantId { get; set; }
        public string Name { get; set; }
        public decimal LocationLongitude { get; set; }
        public decimal LocationLatitude { get; set; }
        public string Url { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public DateTime AddTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Active { get; set; }
        //OneToOne
        public Address Address { get; set; }
        //OneToMany
        public List<Dish> Dishes { get; set; }
        public List<Menu> Menus { get; set; }
        public List<RestaurantUser> RestaurantUsers { get; set; }
        public List<RequestLog> RequestLogs { get; set; }
        //foreign keys
        public int PromotionId { get; set; }
        public Promotion Promotion { get; set; }
    }
}
