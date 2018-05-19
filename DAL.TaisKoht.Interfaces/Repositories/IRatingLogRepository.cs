using DAL.Interfaces.Repositories;
using Domain;

namespace DAL.TaisKoht.Interfaces.Repositories
{
    public interface IRatingLogRepository : IRepository<RatingLog>
    {
        /// <summary>
        /// Check for entity existance by PK value
        /// </summary>
        /// <param name="id">RatingLog PK value</param>
        /// <returns></returns>
        bool Exists(int id);
    }
}