using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using BusinessLogic.Factories;
using BusinessLogic.Helpers;
using BusinessLogic.Services;
using Domain;

namespace BusinessLogic.DTO
{
    public class DishDTO
    {
        public int DishId { get; set; }
        [MinLength(3)]
        [MaxLength(40)]
        public string Title { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        public DateTime? AvailableFrom { get; set; }
        public DateTime? AvailableTo { get; set; }
        public DateTime? ServeTime { get; set; }
        public bool? Vegan { get; set; }
        public bool? LactoseFree { get; set; }
        public bool? GlutenFree { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public Decimal? Kcal { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public Decimal? WeightG { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public Decimal? Price { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public Decimal? DailyPrice { get; set; }
        public bool? Daily { get; set; }
        //foreign keys
        [Required]
        public int RestaurantId { get; set; }
        //additional data
        public SimplePromotionDTO Promotion { get; set; }
        public List<IngredientDTO> Ingredients { get; set; }
        public Rating Rating { get; set; }

        public static DishDTO CreateFromDomain(Dish dish)
        {
            if (dish == null || !dish.Active) { return null; }
            return new DishDTO()
            {
                DishId = dish.DishId,
                Title = dish.Title,
                Description = dish.Description,
                AvailableFrom = dish.AvailableFrom,
                AvailableTo = dish.AvailableTo,
                ServeTime = dish.ServeTime,
                Vegan = dish.Vegan,
                LactoseFree = dish.LactoseFree,
                GlutenFree = dish.GlutenFree,
                Kcal = dish.Kcal,
                WeightG = dish.WeightG,
                Price = dish.Price,
                DailyPrice = dish.DailyPrice,
                Daily = dish.Daily,
                RestaurantId = dish.RestaurantId,
                Rating = dish.RatingLogs.Any() ? Rating.Create(dish.RatingLogs) : null
        };
        }

        public static DishDTO CreateFromMenuDish(MenuDish md)
        {
            if (md == null || !md.Active) { return null; }

            var dish = CreateFromDomain(md.Dish);

            return dish;
        }

        public static DishDTO CreateFromDomainWithAssociatedTables(Dish d)
        {
            var dish = CreateFromDomain(d);
            if (dish == null) { return null; }

            dish.Promotion = SimplePromotionDTO.CreateFromDomain(d.Promotion);
            dish.Ingredients = d.DishIngredients.Select(IngredientDTO.CreateFromDishIngredientDomain).ToList();
            dish.Rating = d.RatingLogs.Any() ? Rating.CreateWithComments(d.RatingLogs) : null;
            return dish;
        }
    }

    public class PostDishDTO
    {
        [MinLength(3)]
        [MaxLength(40)]
        public string Title { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        public DateTime? AvailableFrom { get; set; }
        public DateTime? AvailableTo { get; set; }
        public DateTime? ServeTime { get; set; }
        public bool? Vegan { get; set; }
        public bool? LactoseFree { get; set; }
        public bool? GlutenFree { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public Decimal? Kcal { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public Decimal? WeightG { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public Decimal? Price { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public Decimal? DailyPrice { get; set; }
        public bool? Daily { get; set; }
        //foreign keys
        [Required]
        public int RestaurantId { get; set; }
        public int MenuId { get; set; }
        public int? PromotionId { get; set; }
    }
}
