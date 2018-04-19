using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Dish
    {
        public int DishId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime AvailableFrom { get; set; }
        public DateTime AvailableTo { get; set; }
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
        //OneToMany
        public List<DishIngredient> DishIngredients { get; set; }
        public List<MenuDish> MenuDishes { get; set; }
        public List<RequestLog> RequestLogs { get; set; }
        //foreign keys
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int PromotionId { get; set; }
        public Promotion Promotion { get; set; }
    }
}
