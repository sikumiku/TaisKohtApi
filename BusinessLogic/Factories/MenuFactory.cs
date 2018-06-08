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
        Menu Create(PostMenuDTO menuDTO);
        MenuDTO CreateComplex(Menu newMenu);
        SimpleMenuDTO CreateSimple(Menu newMenu);
    }

    public class MenuFactory : IMenuFactory
    {
        public MenuDTO Create(Menu menu)
        {
            return MenuDTO.CreateFromDomain(menu);
        }

        public Menu Create(PostMenuDTO menuDTO)
        {
            return new Menu
            {
                RestaurantId = menuDTO.RestaurantId,
                PromotionId = menuDTO.PromotionId,
                Name = menuDTO.Name,
                ActiveFrom = menuDTO.ActiveFrom,
                ActiveTo = menuDTO.ActiveTo,
                RepetitionInterval = menuDTO.RepetitionInterval
            };
        }

        public MenuDTO CreateComplex(Menu menu)
        {
            return MenuDTO.CreateFromDomainWithAssociatedTables(menu);
        }

        public SimpleMenuDTO CreateSimple(Menu menu)
        {
            return SimpleMenuDTO.CreateFromDomain(menu);
        }
    }
}
