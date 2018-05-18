using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Domain.Helpers;

namespace Domain
{
    public class Ingredient : EssentialEntityBase
    {
        public int IngredientId { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(255)]
        public string Description { get; set; }
        [MinLength(1)]
        [MaxLength(10)]
        public string AmountUnit { get; set; }
        //OneToMany
        public List<DishIngredient> DishIngredients { get; set; } = new List<DishIngredient>();
        //foreign keys
        [Required]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
