﻿using System;
using System.Collections.Generic;
using System.Text;
using DAL.Interfaces.Repositories;
using Domain;

namespace DAL.TaisKoht.Interfaces.Repositories
{
    public interface IPromotionRepository : IRepository<Promotion>
    {
        /// <summary>
        /// Check for entity existance by PK value
        /// </summary>
        /// <param name="id">Promotion PK value</param>
        /// <returns></returns>
        bool Exists(int id);
    }
}
