using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessLogic.DTO;
using BusinessLogic.Factories;
using DAL.TaisKoht.Interfaces;
using Domain;

namespace BusinessLogic.Services
{
    public class MenuService : IMenuService
    {
        private readonly ITaisKohtUnitOfWork _uow;
        private readonly IMenuFactory _menuFactory;


        public MenuService(ITaisKohtUnitOfWork uow, IMenuFactory menuFactory)
        {
            _uow = uow;
            _menuFactory = menuFactory;
        }

        public MenuDTO AddNewMenu(PostMenuDTO menuDTO)
        {
            var newMenu = _menuFactory.Create(menuDTO);
            _uow.Menus.Add(newMenu);
            _uow.SaveChanges();
            return _menuFactory.Create(newMenu);
        }

        //Seda pole vb vaja
        public IEnumerable<MenuDTO> GetAllMenus()
        {
            return _uow.Menus.All().Where(x => x.Active)
                .Select(menu => _menuFactory.Create(menu));
        }

        public MenuDTO GetMenuById(int id)
        {
            var menu = _uow.Menus.Find(id); //also returns related dishes and promotion if any
            if (menu == null || !menu.Active) return null;

            return _menuFactory.CreateComplex(menu);
        }

        public int GetUserMenuCount(string userId)
        {
            return _uow.Menus.GetUserMenuCount(userId);
        }

        public void UpdateMenu(int id, PostMenuDTO updatedMenuDTO)
        {
            if (_uow.Menus.Exists(id))
            {
                Menu menu = _uow.Menus.Find(id);
                menu.ActiveFrom = updatedMenuDTO.ActiveFrom;
                menu.ActiveTo = updatedMenuDTO.ActiveTo;
                menu.PromotionId = updatedMenuDTO.PromotionId;
                menu.RepetitionInterval = updatedMenuDTO.RepetitionInterval;
                _uow.Menus.Update(menu);
                _uow.SaveChanges();
            }
        }

        public void DeleteMenu(int id)
        {
            Menu menu = _uow.Menus.Find(id);
            _uow.Menus.Remove(menu);
            _uow.SaveChanges();
        }
    }
}
