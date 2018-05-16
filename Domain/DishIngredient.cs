using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
    public class DishIngredient
    {
        public int DishIngredientId { get; set; }
        [MaxLength(500)]
        public int Amount { get; set; }
        public DateTime AddTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Active { get; set; }

        //foreign keys
        [Required]
        public int DishId { get; set; }
        [Required]
        public Dish Dish { get; set; }
        [Required]
        public int IngredientId { get; set; }
        [Required]
        public Ingredient Ingredient { get; set; }
    }
}
