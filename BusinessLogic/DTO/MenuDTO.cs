using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Domain;

namespace BusinessLogic.DTO
{
    public class MenuDTO
    {
        public int MenuId { get; set; }
        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }
        [Range(1, 365)]
        public int? RepetitionInterval { get; set; }
        //OneToMany
        public List<DishIngredient> DishIngredients { get; set; } = new List<DishIngredient>();
        //foreign keys
        [Required]
        public int RestaurantId { get; set; }
        [Required]
        public int UserId { get; set; }
        public int? PromotionId { get; set; }
        public Promotion Promotion { get; set; }


        public static MenuDTO CreateFromDomain(Menu menu)
        {
            if (menu == null || !menu.Active) { return null; }
            return new MenuDTO()
            {
                MenuId = menu.MenuId,
                ActiveFrom = menu.ActiveFrom,
                ActiveTo = menu.ActiveTo,
                RepetitionInterval = menu.RepetitionInterval,
                RestaurantId = menu.RestaurantId,
                UserId = menu.UserId,
                PromotionId = menu.PromotionId
            };
        }

        public static MenuDTO CreateFromDomainWithAssociatedTables(Menu m)
        {

            throw new NotImplementedException();
            
        }
    }
}
