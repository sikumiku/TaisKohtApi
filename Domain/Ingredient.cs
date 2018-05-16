using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
    public class Ingredient
    {
        public int IngredientId { get; set; }
        [MinLength(3)]
        [MaxLength(40)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        [MinLength(1)]
        [MaxLength(10)]
        public string AmountUnit { get; set; }
        public DateTime AddTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Active { get; set; }
        //OneToMany
        public List<DishIngredient> DishIngredients { get; set; } = new List<DishIngredient>();
        //foreign keys
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
