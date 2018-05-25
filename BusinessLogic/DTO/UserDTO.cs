using System;
using System.Collections.Generic;
using System.Text;
using Domain;

namespace BusinessLogic.DTO
{
    public class UserDTO
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public bool Active { get; set; }

        public static UserDTO CreateFromDomain(User user)
        {
            if (user == null || !user.Active) { return null; }
            return new UserDTO()
            {
                UserId = user.UserId,
                Email = user.Email,
                UserName = user.UserName,
                Active = user.Active
            };
        }
    }
}
