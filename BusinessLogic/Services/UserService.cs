using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.DTO;
using BusinessLogic.Factories;
using DAL.TaisKoht.EF;
using DAL.TaisKoht.Interfaces;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly ITaisKohtUnitOfWork _uow;
        private readonly IUserFactory _userFactory;
        private static UserManager<User> _userManager;

        public UserService(ITaisKohtUnitOfWork uow, IUserFactory userFactory, UserManager<User> userManager)
        {
            _uow = uow;
            _userFactory = userFactory;
            _userManager = userManager;
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

        public void UpdateUser(string id, UpdateUserDTO updatedUserDTO)
        {
            User user = _uow.Users.Find(id);
            user.UserName = updatedUserDTO.UserName;
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, updatedUserDTO.Password);
            user.Active = updatedUserDTO.Active;
            user.UpdateTime = DateTime.UtcNow;
            _uow.Users.Update(user);
            _uow.SaveChanges();
        }

        public static async Task<IdentityUserRole<string>> GetRolesForUser(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }
    }
}
