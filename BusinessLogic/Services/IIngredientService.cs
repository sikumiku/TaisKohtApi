﻿using BusinessLogic.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Services
{
    public interface IIngredientService
    {
        IEnumerable<IngredientDTO> GetAllUserIngredients(string userId);
        IEnumerable<IngredientDTO> GetAllIngredients();

        IngredientDTO GetIngredientById(int id);

        IngredientDTO AddNewIngredient(PostIngredientDTO dto, string userId);

        IngredientDTO UpdateIngredient(int id, PostIngredientDTO dto);

        void DeleteIngredient(int id);
    }
}
