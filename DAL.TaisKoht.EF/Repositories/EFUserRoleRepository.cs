using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.EF;
using DAL.TaisKoht.Interfaces.Repositories;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DAL.TaisKoht.EF.Repositories
{
    public class EFUserRoleRepository : EFRepository<IdentityUserRole<string>>, IUserRoleRepository
    {
        public EFUserRoleRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public IEnumerable<IdentityUserRole<string>> getUserRolesForUser(User user)
        {
            return RepositoryDbSet.AsQueryable().ToList().Where(ur => ur.UserId == user.Id);
        }
    }
}
