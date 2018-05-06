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

        MenuDTO AddNewMenu(MenuDTO dto);

        void UpdateMenu(int id, MenuDTO dto);

        void DeleteMenu(int id);
    }
}
