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
    public class DishService : IDishService
    {
        private readonly ITaisKohtUnitOfWork _uow;
        private readonly IDishFactory _dishFactory;

        public DishService(ITaisKohtUnitOfWork uow, IDishFactory dishFactory)
        {
            _uow = uow;
            _dishFactory = dishFactory;
        }

        public DishDTO AddNewDish(DishDTO dishDTO)
        {
            var newDish = _dishFactory.Create(dishDTO);
            _uow.Dishes.Add(newDish);
            _uow.SaveChanges();
            return _dishFactory.CreateComplex(newDish);
        }

        public IEnumerable<DishDTO> GetAllDishes()
        {
            return _uow.Dishes.All().Where(x => x.Active)
                .Select(dish => _dishFactory.Create(dish));
        }

        public DishDTO GetDishById(int id)
        {
            var dish = _uow.Dishes.Find(id);
            if (dish == null || !dish.Active) return null;

            return _dishFactory.CreateComplex(dish);
        }

        public void UpdateDish(int id, DishDTO updatedDishDTO)
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
            dish.UpdateTime = DateTime.UtcNow;
            _uow.Dishes.Update(dish);
            _uow.SaveChanges();
        }

        public void DeleteDish(int id)
        {
            Dish dish = _uow.Dishes.Find(id);
            dish.UpdateTime = DateTime.UtcNow;
            dish.Active = false;
            _uow.Dishes.Update(dish);
            _uow.SaveChanges();
        }

        public IEnumerable<DishDTO> SearchDishByTitle(string title)
        {
            if (String.IsNullOrEmpty(title)) return null;

            return _uow.Dishes.All().Where(x => x.Title.Contains(title))
                .Select(dish => _dishFactory.Create(dish));
        }

        public IEnumerable<DishDTO> SearchDishByPriceLimit(decimal dishPrice)
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

            var dishes = _uow.Dishes.All().Where(x => x.Active).OrderByDescending(x => x.RatingLogs.Select(r => r.Rating))
                .Select(dish => _dishFactory.Create(dish));

            if (dishes.Count() < amount) return null;

            return dishes.Take(amount);
        }
    }
}
