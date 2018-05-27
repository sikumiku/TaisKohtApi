using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly UserManager<User> _userManager;

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
            var roles = GetRolesForUser(user);
            return _userFactory.CreateComplex(user, roles);
        }

        public void RemoveUser(string id)
        {
            var user = _uow.Users.Find(id);
            _uow.Users.Remove(user);
            _uow.SaveChanges();
        }

        public void UpdateUser(string id, UpdateUserDTO updatedUserDTO)
        {
            User user = _uow.Users.Find(id);
            if (updatedUserDTO.UserName != null) { user.UserName = updatedUserDTO.UserName; }
            if (updatedUserDTO.FirstName != null) { user.FirstName = updatedUserDTO.FirstName; }
            if (updatedUserDTO.LastName != null) { user.LastName = updatedUserDTO.LastName; }
            if(updatedUserDTO.PhoneNumber != null) { user.PhoneNumber = updatedUserDTO.PhoneNumber; }
            if (updatedUserDTO.Password != null)
            {
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, updatedUserDTO.Password);
            }
            _uow.Users.Update(user);
            _uow.SaveChanges();
        }

        private List<String> GetRolesForUser(User user)
        {
            IEnumerable<string> userRolesIds = _uow.UserRoles.getUserRolesForUser(user).Select(ur => ur.RoleId).ToList();
            List<string> roles = new List<string>();
            foreach (var userRoleId in userRolesIds)
            {
                var role = _uow.Roles.Find(userRoleId);
                if (role != null)
                {
                    roles.Add(role.Name);
                }
            }

            return roles;
        }
    }
}
