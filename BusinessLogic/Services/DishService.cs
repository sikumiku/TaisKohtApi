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

        public DishDTO AddNewDish(PostDishDTO dishDTO, string userId)
        {
            var newDish = _dishFactory.Create(dishDTO);
            newDish.UserId = userId;
            _uow.Dishes.Add(newDish);
            _uow.SaveChanges();
            return _dishFactory.CreateComplex(newDish);
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

        public IEnumerable<DishDTO> GetAllDailyDishes(bool vegan, bool glutenFree, bool lactoseFree)
        {
            var dishes = _uow.Dishes.All().Where(x => x.Active && x.Daily == true && x.AvailableFrom <= DateTime.Today && x.AvailableTo >= DateTime.Today);

            if (vegan == true && glutenFree == true && lactoseFree == true) return dishes.Where(x => x.Vegan == true && x.GlutenFree == true && x.LactoseFree == true).Select(dish => _dishFactory.Create(dish));

            if (vegan == true && glutenFree == true) return dishes.Where(x => x.Vegan == true && x.GlutenFree == true).Select(dish => _dishFactory.Create(dish));

            if (vegan == true && lactoseFree == true) return dishes.Where(x => x.Vegan == true && x.LactoseFree == true).Select(dish => _dishFactory.Create(dish));

            if (glutenFree == true && lactoseFree == true) return dishes.Where(x => x.GlutenFree == true && x.LactoseFree == true).Select(dish => _dishFactory.Create(dish));

            if (vegan == true) return dishes.Where(v => v.Vegan == true).Select(dish => _dishFactory.Create(dish));

            if (glutenFree == true) return dishes.Where(g => g.GlutenFree == true).Select(dish => _dishFactory.Create(dish));

            if (lactoseFree == true) return dishes.Where(l => l.LactoseFree == true).Select(dish => _dishFactory.Create(dish));

            return dishes.Select(dish => _dishFactory.Create(dish));
        }
    }
}
