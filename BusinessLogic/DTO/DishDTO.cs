using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Domain;

namespace BusinessLogic.DTO
{
    public class DishDTO
    {
        public int DishId { get; set; }
        public int RestaurantId { get; set; }
        public int UserId { get; set; }
        public int PromotionId { get; set; }

        [MinLength(3)]
        [MaxLength(40)]
        public string Title { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        public DateTime AvailableFrom { get; set; }
        public DateTime AvailableTo { get; set; }
        public DateTime ServeTime { get; set; }
        public bool Vegan { get; set; }
        public bool Lactose { get; set; }
        public bool Gluten { get; set; }
        public Decimal Kcal { get; set; }
        public Decimal WeightG { get; set; }
        public Decimal Price { get; set; }
        public Decimal DailyPrice { get; set; }
        public bool Daily { get; set; }
        public DateTime AddTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Active { get; set; }

        public List<DishIngredient> DishIngredients { get; set; }
        public List<MenuDish> MenuDishes { get; set; }
        public List<RequestLog> RequestLogs { get; set; }

        public static DishDTO CreateFromDomain(Dish dish)
        {
            if (dish == null) { return null; }
            return new DishDTO()
            {
                DishId = dish.DishId,
                RestaurantId = dish.RestaurantId,
                UserId = dish.UserId,
                PromotionId = dish.PromotionId,
                Title = dish.Title,
                Description = dish.Description,
                AvailableFrom = dish.AvailableFrom,
                AvailableTo = dish.AvailableTo,
                ServeTime = dish.ServeTime,
                Vegan = dish.Vegan,
                Lactose = dish.Lactose,
                Gluten = dish.Gluten,
                Kcal = dish.Kcal,
                WeightG = dish.WeightG,
                Price = dish.Price,
                DailyPrice = dish.DailyPrice,
                Daily = dish.Daily,
                AddTime = dish.AddTime,
                UpdateTime = dish.UpdateTime,
                Active = dish.Active
            };
        }

        public static DishDTO CreateFromDomainWithAssociatedTables(Dish d)
        {

            throw new NotImplementedException();
            
        }
    }
}
