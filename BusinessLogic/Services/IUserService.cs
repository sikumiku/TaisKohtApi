using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.DTO;
using Domain;

namespace BusinessLogic.Services
{
    public interface IUserService
    {
        UserDTO GetUserById(string id);
        void UpdateUser(string id, UpdateUserDTO dto);
        void DeactivateUser(string id);
    }
}
