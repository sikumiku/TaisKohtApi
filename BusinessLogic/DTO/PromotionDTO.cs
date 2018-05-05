using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Domain;

namespace BusinessLogic.DTO
{
    public class PromotionDTO
    {
        public int PromotionId { get; set; }
        [MinLength(3)]
        [MaxLength(40)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        public string Type { get; set; }
        public DateTime ValidTo { get; set; }
        public DateTime AddTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Active { get; set; }
        public List<Menu> Menus { get; set; }
        public List<Dish> Dishes { get; set; }
        public List<Restaurant> Restaurants { get; set; }

        public static PromotionDTO CreateFromDomain(Promotion promotion)
        {
            if(promotion == null) { return null; }
            return new PromotionDTO()
            {
                PromotionId = promotion.PromotionId,
                Name = promotion.Name,
                Description = promotion.Description,
                Type = promotion.Type,
                ValidTo = promotion.ValidTo,
                AddTime = promotion.AddTime,
                UpdateTime = promotion.UpdateTime,
                Active = promotion.Active
            };
        }

        public static PromotionDTO CreateFromDomainWithAssociatedTables(Promotion p)
        {

            throw new NotImplementedException();
            //var promotion = CreateFromDomain(p);
            //if (promotion == null) { return null; }

            //promotion.Menus = promotion?.Menus
            //    .Select(menu => MenuDTO.CreateFromDomain(menu)).ToList(); //menuDTO not implemented
            //return promotion;
        }

    }
}
