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
        public int RestaurantId { get; set; }
        public int UserId { get; set; }
        public int PromotionId { get; set; }

        public DateTime ActiveFrom { get; set; }
        public DateTime ActiveTo { get; set; }
        public int RepetitionInterval { get; set; }
        public DateTime AddTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Active { get; set; }

        public List<DishIngredient> DishIngredient { get; set; }
        

        public static MenuDTO CreateFromDomain(Menu menu)
        {
            if (menu == null) { return null; }
            return new MenuDTO()
            {
                MenuId = menu.MenuId,
                RestaurantId = menu.RestaurantId,
                UserId = menu.UserId,
                PromotionId = menu.PromotionId,
                ActiveFrom = menu.ActiveFrom,
                ActiveTo = menu.ActiveTo,
                RepetitionInterval = menu.RepetitionInterval,
                AddTime = menu.AddTime,
                UpdateTime = menu.UpdateTime,
                Active = menu.Active
            };
        }

        public static MenuDTO CreateFromDomainWithAssociatedTables(Menu m)
        {

            throw new NotImplementedException();
            
        }
    }
}
