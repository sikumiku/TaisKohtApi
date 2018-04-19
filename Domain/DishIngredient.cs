using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class DishIngredient
    {
        public int DishIngredientId { get; set; }
        public int Amount { get; set; }
        public DateTime AddTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Active { get; set; }
        //foreign keys
        public int DishId { get; set; }
        public Dish Dish { get; set; }
        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }
    }
}
