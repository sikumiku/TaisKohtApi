using BusinessLogic.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Services
{
    public interface IIngredientService
    {
        IEnumerable<IngredientDTO> GetAllIngredients();

        IngredientDTO GetIngredientById(int id);

        IngredientDTO AddNewIngredient(IngredientDTO dto);

        void UpdateIngredient(int id, IngredientDTO dto);

        void DeleteIngredient(int id);
    }
}
