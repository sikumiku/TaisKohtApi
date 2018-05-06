using Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BusinessLogic.DTO
{
    public class IngredientDTO
    {
        public int IngredientId { get; set; }
        public int UserId { get; set; }

        [MinLength(3)]
        [MaxLength(40)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        [MinLength(3)]
        [MaxLength(10)]
        public string AmountUnit { get; set; }
        public DateTime AddTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Active { get; set; }

        public List<DishIngredient> DishIngredients { get; set; }

        public static IngredientDTO CreateFromDomain(Ingredient ingredient)
        {
            if (ingredient == null) { return null; }
            return new IngredientDTO()
            {
                IngredientId = ingredient.IngredientId,
                UserId = ingredient.UserId,
                Name = ingredient.Name,
                Description = ingredient.Description,
                AmountUnit = ingredient.AmountUnit,
                AddTime = ingredient.AddTime,
                UpdateTime = ingredient.UpdateTime,
                Active = ingredient.Active
            };
        }

        public static IngredientDTO CreateFromDomainWithAssociatedTables(Ingredient i)
        {

            throw new NotImplementedException();

        }
    }
}
