using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic.DTO;
using Domain;

namespace BusinessLogic.Factories
{
    public interface IUserFactory
    {
        UserDTO Create(User user);
        User Create(UserDTO userDTO);
    }

    public class UserFactory : IUserFactory
    {
        public UserDTO Create(User user)
        {
            return UserDTO.CreateFromDomain(user);
        }

        public User Create(UserDTO userDTO)
        {
            return new User
            {
                UserId = userDTO.UserId,
                UserName = userDTO.UserName,
                Email = userDTO.Email,
                Active = userDTO.Active
            };
        }
    }
}
