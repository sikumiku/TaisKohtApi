using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessLogic.DTO;
using BusinessLogic.Factories;
using DAL.TaisKoht.Interfaces;
using Domain;

namespace BusinessLogic.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly ITaisKohtUnitOfWork _uow;
        private readonly IIngredientFactory _ingredientFactory;


        public IngredientService(ITaisKohtUnitOfWork uow, IIngredientFactory ingredientFactory)
        {
            _uow = uow;
            _ingredientFactory = ingredientFactory;
        }

        public IngredientDTO AddNewIngredient(PostIngredientDTO ingredientDTO)
        {
            var newIngredient = _ingredientFactory.Create(ingredientDTO);
            _uow.Ingredients.Add(newIngredient);
            _uow.SaveChanges();
            return _ingredientFactory.Create(newIngredient);
        }

        public IEnumerable<IngredientDTO> GetAllIngredients()
        {
            return _uow.Ingredients.All().Where(x => x.Active)
                .Select(ingredient => _ingredientFactory.Create(ingredient));
        }

        public IngredientDTO GetIngredientById(int id)
        {
            var ingredient = _uow.Ingredients.Find(id);
            if (ingredient == null || !ingredient.Active) return null;

            return _ingredientFactory.Create(ingredient);
        }

        public void UpdateIngredient(int id, PostIngredientDTO updatedIngredientDTO)
        {
            Ingredient ingredient = _uow.Ingredients.Find(id);
            ingredient.Name = updatedIngredientDTO.Name;
            ingredient.Description = updatedIngredientDTO.Description;
            ingredient.AmountUnit = updatedIngredientDTO.AmountUnit;
            ingredient.UpdateTime = DateTime.UtcNow;
            _uow.Ingredients.Update(ingredient);
            _uow.SaveChanges();
        }

        public void DeleteIngredient(int id)
        {
            Ingredient ingredient = _uow.Ingredients.Find(id);
            ingredient.Active = false;
            ingredient.UpdateTime = DateTime.UtcNow;
            _uow.Ingredients.Update(ingredient);
            _uow.SaveChanges();
        }
    }
}
