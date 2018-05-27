using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic.DTO;

namespace BusinessLogic.Services
{
    public interface IDishService
    {
        IEnumerable<DishDTO> GetAllDishes();

        DishDTO GetDishById(int id);

        DishDTO AddNewDish(PostDishDTO dto, string userId);

        void UpdateDishIngredients(int dishId, PostIngredientForDishDTO[] postIngredientForDishDTOs);

        DishDTO UpdateDish(int id, PostDishDTO dto);

        void DeleteDish(int id);

        IEnumerable<SimpleDishDTO> GetAllDailyDishes(bool vegan, bool glutenFree, bool lactoseFree);

        IEnumerable<DishDTO> SearchDishByTitle(string title);

        IEnumerable<DishDTO> SearchDishByPriceLimit(decimal dishPrice);

        IEnumerable<DishDTO> GetTopDishes(int amount);
    }
}
