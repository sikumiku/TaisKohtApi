using Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BusinessLogic.DTO
{
    public class IngredientDTO
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
        public Decimal? Amount { get; set; }
        //foreign keys
        [Required]
        public string UserId { get; set; }

        public static IngredientDTO CreateFromDomain(Ingredient ingredient)
        {
            if (ingredient == null) { return null; }
            return new IngredientDTO()
            {
                IngredientId = ingredient.IngredientId,
                Name = ingredient.Name,
                Description = ingredient.Description,
                AmountUnit = ingredient.AmountUnit,
                UserId = ingredient.UserId
            };
        }

        public static IngredientDTO CreateFromDishIngredientDomain(DishIngredient di)
        {
            if (di == null) { return null; }

            var ingredient = CreateFromDomain(di.Ingredient);
            ingredient.Amount = di.Amount;

            return ingredient;
        }
    }

    public class PostIngredientDTO
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(255)]
        public string Description { get; set; }
        [MinLength(1)]
        [MaxLength(10)]
        public string AmountUnit { get; set; }
    }
}
