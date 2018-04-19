using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class MenuDish
    {
        public int MenuDishId { get; set; }
        public DateTime AddTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Active { get; set; }
        //foreign keys
        public int MenuId { get; set; }
        public Menu Menu { get; set; }
        public int DishId { get; set; }
        public Dish Dish { get; set; }
    }
}
