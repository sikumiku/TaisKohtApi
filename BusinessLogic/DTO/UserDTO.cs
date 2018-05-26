using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Services;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace BusinessLogic.DTO
{
    public class UserDTO
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public bool Active { get; set; }
        public IList<string> UserRoles { get; set; }

        public static UserDTO CreateFromDomain(User user)
        {
            if (user == null || !user.Active) { return null; }
            return new UserDTO()
            {
                UserId = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Active = user.Active,
                UserRoles = UserService.GetRolesForUser(user).Result
            };
        }
    }

    public class UpdateUserDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }
    }
}
