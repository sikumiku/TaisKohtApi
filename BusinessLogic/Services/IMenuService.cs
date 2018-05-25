using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic.DTO;

namespace BusinessLogic.Services
{
    public interface IMenuService
    {
        IEnumerable<MenuDTO> GetAllMenus();

        MenuDTO GetMenuById(int id);

        int GetUserMenuCount(string userId);

        MenuDTO AddNewMenu(PostMenuDTO dto);

        void UpdateMenu(int id, PostMenuDTO dto);

        void DeleteMenu(int id);
    }
}
