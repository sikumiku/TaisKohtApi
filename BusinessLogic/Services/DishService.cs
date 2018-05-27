using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic.DTO;
using BusinessLogic.Factories;
using DAL.TaisKoht.Interfaces;
using Domain;

namespace BusinessLogic.Services
{
    public class DishService : IDishService
    {
        private readonly ITaisKohtUnitOfWork _uow;
        private readonly IDishFactory _dishFactory;

        public DishService(ITaisKohtUnitOfWork uow, IDishFactory dishFactory)
        {
            _uow = uow;
            _dishFactory = dishFactory;
        }

        public DishDTO AddNewDish(PostDishDTO dishDTO, string userId)
        {
            var newDish = _dishFactory.Create(dishDTO);
            newDish.UserId = userId;
            _uow.Dishes.Add(newDish);
            _uow.SaveChanges();

            if (dishDTO.MenuId != null &&_uow.Menus.Exists((int)dishDTO.MenuId))
            {
                MenuDish md = new MenuDish
                {
                    DishId = newDish.DishId,
                    MenuId = dishDTO.MenuId.Value
                };

                _uow.MenuDishes.Add(md);
                _uow.SaveChanges();
            }
            
            
            return _dishFactory.CreateComplex(newDish);
        }

        public void UpdateDishIngredients(int dishId, PostIngredientForDishDTO[] postIngredientForDishDTOs)
        {
            if (_uow.Dishes.Exists(dishId))
            {
                Dish dish = _uow.Dishes.Find(dishId);
                var existingDishIngredients = dish.DishIngredients;

                var requestDishIngredients = new List<DishIngredient>();

                Array.ForEach(postIngredientForDishDTOs, pid => requestDishIngredients.Add(new DishIngredient { DishId = dishId, IngredientId = pid.IngredientId, Amount = pid.Amount}));

                var deletedDishIngredients = existingDishIngredients.FindAll(x => !requestDishIngredients.Exists(y => IsEqualDishIngredient(x, y)));

                var addedDishIngredients = requestDishIngredients.FindAll(x => !existingDishIngredients.Exists(y => IsEqualDishIngredient(x, y)));

                deletedDishIngredients.ForEach(di => _uow.DishIngredients.Remove(di));
                addedDishIngredients.ForEach(di => _uow.DishIngredients.Add(di));

                _uow.SaveChanges();
            }
        }

        private static bool IsEqualDishIngredient(DishIngredient x, DishIngredient y)
        {
            return x.DishId == y.DishId && x.IngredientId == y.IngredientId;
        }

        public IEnumerable<DishDTO> GetAllDishes()
        {
            return _uow.Dishes.All()
                .Select(dish => _dishFactory.Create(dish));
        }

        public DishDTO GetDishById(int id)
        {
            var dish = _uow.Dishes.Find(id);
            if (dish == null) return null;

            return _dishFactory.CreateComplex(dish);
        }

        public DishDTO UpdateDish(int id, PostDishDTO updatedDishDTO)
        {
            if (_uow.Dishes.Exists(id))
            {
                Dish dish = _uow.Dishes.Find(id);
                dish.Title = updatedDishDTO.Title;
                dish.Description = updatedDishDTO.Description;
                dish.AvailableFrom = updatedDishDTO.AvailableFrom;
                dish.AvailableTo = updatedDishDTO.AvailableTo;
                dish.ServeTime = updatedDishDTO.ServeTime;
                dish.Vegan = updatedDishDTO.Vegan;
                dish.LactoseFree = updatedDishDTO.LactoseFree;
                dish.GlutenFree = updatedDishDTO.GlutenFree;
                dish.Kcal = updatedDishDTO.Kcal;
                dish.WeightG = updatedDishDTO.WeightG;
                dish.Price = updatedDishDTO.Price;
                dish.DailyPrice = updatedDishDTO.DailyPrice;
                dish.Daily = updatedDishDTO.Daily;
                dish.PromotionId = updatedDishDTO.PromotionId;
                _uow.Dishes.Update(dish);
                _uow.SaveChanges();
            }

            return GetDishById(id);
        }

        public void DeleteDish(int id)
        {
            Dish dish = _uow.Dishes.Find(id);
            _uow.Dishes.Remove(dish);
            _uow.SaveChanges();
        }

        public IEnumerable<DishDTO> SearchDishByTitle(string title)
        {
            if (String.IsNullOrEmpty(title)) return null;

            return _uow.Dishes.All().Where(x => x.Title.Contains(title))
                .Select(dish => _dishFactory.Create(dish));
        }

        public IEnumerable<DishDTO> SearchDishByPriceLimit(decimal? dishPrice)
        {
            decimal price;
            if (!decimal.TryParse(dishPrice.ToString(), out price)) return null;

            return _uow.Dishes.All().Where(x => x.Price <= dishPrice)
                .Select(dish => _dishFactory.Create(dish));
        }

        public IEnumerable<DishDTO> GetTopDishes(int amount)
        {
            int topAmount;
            if (!int.TryParse(amount.ToString(), out topAmount)) return null;

            var dishes = _uow.Dishes.All().OrderByDescending(x => x.RatingLogs.Select(r => r.Rating))
                .Select(dish => _dishFactory.Create(dish));

            return dishes.Take(amount);
        }

        public IEnumerable<SimpleDishDTO> GetAllDailyDishes(bool vegan, bool glutenFree, bool lactoseFree)
        {
            var dishes = _uow.Dishes.All().Where(
                x => x.Daily == true &&
                     (x.AvailableFrom == null || x.AvailableFrom <= DateTime.UtcNow) &&
                     (x.AvailableTo == null || x.AvailableTo >= DateTime.UtcNow));

            if (vegan) dishes = dishes.Where(d => d.Vegan == true);

            if (glutenFree) dishes = dishes.Where(d => d.GlutenFree == true);

            if (lactoseFree) dishes = dishes.Where(d => d.LactoseFree == true);

            return dishes.Select(dish => _dishFactory.CreateSimple(dish));
        }
    }
}
