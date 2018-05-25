using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic.DTO;
using BusinessLogic.Factories;
using DAL.TaisKoht.Interfaces;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly ITaisKohtUnitOfWork _uow;
        private readonly UserManager<User> _userManager;
        private readonly IUserFactory _userFactory;

        public UserService(ITaisKohtUnitOfWork uow, UserManager<User> userManager, IUserFactory userFactory)
        {
            _uow = uow;
            _userManager = userManager;
            _userFactory = userFactory;
        }

        public UserDTO GetUserById(string id)
        {
            var user = _uow.Users.Find(id);
            if (user == null) return null;
            return _userFactory.Create(user);
        }

        public void updateUser(string id, UserDTO dto)
        {
            throw new NotImplementedException();
        }

        public void DeactivateUser(string id)
        {
            var user = _uow.Users.Find(id);
            user.UpdateTime = DateTime.UtcNow;
            user.Active = false;
            _uow.Users.Update(user);
            _uow.SaveChanges();
        }

        public void UpdateUser(string id, UserDTO updatedUserDTO)
        {
            User user = _uow.Users.Find(id);
            user.UserName = updatedUserDTO.UserName;
            user.Email = updatedUserDTO.Email;
            user.UpdateTime = DateTime.UtcNow;
            _uow.Users.Update(user);
            _uow.SaveChanges();
        }
    }
}
