using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic.DTO;
using Domain;

namespace BusinessLogic.Factories
{
    public interface IMenuFactory
    {
        MenuDTO Create(Menu menu);
        Menu Create(MenuDTO menuDTO);
    }

    public class MenuFactory : IMenuFactory
    {
        public MenuDTO Create(Menu menu)
        {
            return MenuDTO.CreateFromDomain(menu);
        }

        public Menu Create(MenuDTO menuDTO)
        {
            return new Menu
            {
                MenuId = menuDTO.MenuId,
                RestaurantId = menuDTO.RestaurantId,
                UserId = menuDTO.UserId,
                PromotionId = menuDTO.PromotionId,
                ActiveFrom = menuDTO.ActiveFrom,
                ActiveTo = menuDTO.ActiveTo,
                RepetitionInterval = menuDTO.RepetitionInterval,
                AddTime = menuDTO.AddTime,
                UpdateTime = menuDTO.UpdateTime,
                Active = menuDTO.Active
            };
        }
    }
}
