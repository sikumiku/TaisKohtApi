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

        DishDTO AddNewDish(DishDTO dto);

        void UpdateDish(int id, DishDTO dto);

        void DeleteDish(int id);

        IEnumerable<DishDTO> GetAllDailyDishes(bool vegan, bool glutenFree);

        IEnumerable<DishDTO> SearchDishByTitle(string title);

        IEnumerable<DishDTO> SearchDishByPriceLimit(decimal dishPrice);

        IEnumerable<DishDTO> GetTopDishes(int amount);
    }
}
