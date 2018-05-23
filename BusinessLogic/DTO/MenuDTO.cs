using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
        //foreign keys
        [Required]
        public int RestaurantId { get; set; }
        [Required]
        public int UserId { get; set; }
        public int? PromotionId { get; set; }
        public List<DishDTO> Dishes { get; set; } 

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
                UserId = menu.UserId
            };
        }

        public static MenuDTO CreateFromDomainWithAssociatedTables(Menu m)
        {

            var menuDTO = CreateFromDomain(m);
            if (menuDTO == null) { return null; }

            menuDTO.PromotionId = m.PromotionId;
            List<DishDTO> dishes = m.MenuDishes
                .FindAll(md => md.MenuId == m.MenuId)
                .Select(DishDTO.CreateFromMenuDish)
                .ToList();
            menuDTO.Dishes = dishes;
            return menuDTO;
        }
    }

    public class PostMenuDTO
    {
        //public int MenuId { get; set; }
        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }
        [Range(1, 365)]
        public int? RepetitionInterval { get; set; }
        //foreign keys
        [Required]
        public int RestaurantId { get; set; }
        [Required]
        public int UserId { get; set; }
        public int? PromotionId { get; set; }
    }
}
