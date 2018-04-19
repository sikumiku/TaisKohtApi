using System;
using System.Collections.Generic;

namespace Domain
{
    public class Menu
    {
        public int MenuId { get; set; }
        public DateTime ActiveFrom { get; set; }
        public DateTime ActiveTo { get; set; }
        public int RepetitionInterval { get; set; }
        public DateTime AddTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Active { get; set; }
        //OneToMany
        public List<DishIngredient> DishIngredients { get; set; }
        //foreign keys
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int PromotionId { get; set; }
        public Promotion Promotion { get; set; }
    }
}
