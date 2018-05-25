using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Domain.Helpers;

namespace Domain
{
    public class Dish : EssentialEntityBase
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
        //OneToMany
        public List<DishIngredient> DishIngredients { get; set; } = new List<DishIngredient>();
        public List<MenuDish> MenuDishes { get; set; } = new List<MenuDish>();
        public List<RatingLog> RatingLogs { get; set; } = new List<RatingLog>();
        //foreign keys
        [Required]
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
        [Required]
        public string UserId { get; set; }
        public User User { get; set; }
        public int? PromotionId { get; set; }
        public Promotion Promotion { get; set; }
    }
}