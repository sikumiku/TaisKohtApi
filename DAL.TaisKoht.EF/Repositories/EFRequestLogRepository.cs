using System;
using System.Collections.Generic;
using System.Text;
using DAL.EF;
using DAL.TaisKoht.Interfaces.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.TaisKoht.EF.Repositories
{
    public class EFRequestLogRepository : EFRepository<RequestLog>, IRequestLogRepository
    {
        public EFRequestLogRepository(DbContext dataContext) : base(dataContext)
        {
        }
    }
}
