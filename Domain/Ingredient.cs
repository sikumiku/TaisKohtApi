using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Ingredient
    {
        public int IngredientId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AmountUnit { get; set; }
        public DateTime AddTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Active { get; set; }
        //OneToMany
        public List<DishIngredient> DishIngredients { get; set; }
        //foreign keys
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
