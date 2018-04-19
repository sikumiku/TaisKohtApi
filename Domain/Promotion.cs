using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Promotion
    {
        public int PromotionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public DateTime ValidTo { get; set; }
        public DateTime AddTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Active { get; set; }
        //OneToMany
        public List<Menu> Menus { get; set; }
        public List<Dish> Dishes { get; set; }
        public List<User> Users { get; set; }
        public List<Restaurant> Restaurants { get; set; }
    }
}
