using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic.DTO;
using Domain;

namespace BusinessLogic.Services
{
    public interface IUserService
    {
        UserDTO GetUserById(string id);
        void UpdateUser(string id, UserDTO dto);
        void DeactivateUser(string id);
    }
}
