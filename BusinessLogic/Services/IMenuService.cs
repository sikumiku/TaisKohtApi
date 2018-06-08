using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic.DTO;

namespace BusinessLogic.Services
{
    public interface IMenuService
    {
        IEnumerable<MenuDTO> GetAllMenus();

        IEnumerable<SimpleMenuDTO> GetAllMenusByUser(string userId);

        MenuDTO GetMenuById(int id);

        int GetUserMenuCount(string userId);

        MenuDTO AddNewMenu(PostMenuDTO dto, string userId);

        MenuDTO UpdateMenu(int id, PostMenuDTO dto);

        void UpdateMenuDishes(int id, int[] dishIds);

        void DeleteMenu(int id);
    }
}
