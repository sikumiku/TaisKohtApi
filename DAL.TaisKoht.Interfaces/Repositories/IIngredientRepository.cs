using DAL.Interfaces.Repositories;
using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.TaisKoht.Interfaces.Repositories
{
    public interface IIngredientRepository : IRepository<Ingredient>
    {
        /// <summary>
        /// Check for entity existance by PK value
        /// </summary>
        /// <param name="id">Ingredient PK value</param>
        /// <returns></returns>
        bool Exists(int id);
    }
}
