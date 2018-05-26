using System;
using System.Collections.Generic;
using System.Text;
using DAL.Interfaces.Repositories;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace DAL.TaisKoht.Interfaces.Repositories
{
    public interface IUserRoleRepository : IRepository<IdentityUserRole<string>>
    {
        IEnumerable<IdentityUserRole<string>> getUserRolesForUser(User user);
    }
}
