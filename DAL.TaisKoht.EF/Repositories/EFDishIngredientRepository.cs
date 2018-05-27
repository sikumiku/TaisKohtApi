using System;
using System.Collections.Generic;
using System.Text;
using DAL.EF;
using DAL.TaisKoht.Interfaces.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.TaisKoht.EF.Repositories
{
    public class EFDishIngredientRepository : EFRepository<DishIngredient>, IDishIngredientRepository
    {
        public EFDishIngredientRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public bool Exists(int id)
        {
            throw new NotImplementedException();
        }
    }
}