using System;
using System.Collections.Generic;
using System.Text;
using DAL.Interfaces.Repositories;
using Domain;

namespace DAL.TaisKoht.Interfaces.Repositories
{
    public interface IDishIngredientRepository : IRepository<DishIngredient>
    {
        bool Exists(int id);
    }
}
