using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Domain.Helpers;

namespace Domain
{
    public class DishIngredient : EssentialEntityBase
    {
        public int DishIngredientId { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public Decimal Amount { get; set; }
        //foreign keys
        [Required]
        public int DishId { get; set; }
        public Dish Dish { get; set; }
        [Required]
        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }
    }
}
