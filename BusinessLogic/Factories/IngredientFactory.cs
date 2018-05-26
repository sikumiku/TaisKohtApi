using BusinessLogic.DTO;
using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Factories
{
    public interface IIngredientFactory
    {
        IngredientDTO Create(Ingredient ingredient);
        Ingredient Create(PostIngredientDTO ingredientDTO);
    }

    public class IngredientFactory : IIngredientFactory
    {
        public IngredientDTO Create(Ingredient ingredient)
        {
            return IngredientDTO.CreateFromDomain(ingredient);
        }

        public Ingredient Create(PostIngredientDTO ingredientDTO)
        {
            return new Ingredient
            {
                Name = ingredientDTO.Name,
                Description = ingredientDTO.Description,
                AmountUnit = ingredientDTO.AmountUnit
            };
        }
    }
}
