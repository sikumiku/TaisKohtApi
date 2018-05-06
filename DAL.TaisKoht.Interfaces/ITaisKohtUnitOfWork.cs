using System;
using DAL.Interfaces;
using DAL.TaisKoht.Interfaces.Repositories;

namespace DAL.TaisKoht.Interfaces
{
    public interface ITaisKohtUnitOfWork : IUnitOfWork
    {
        //2 erinevat tüüpi reposid, peopleites on midagi sisse lisatud, all olevates ei ole.
        IPromotionRepository Promotions { get; }

        IRestaurantRepository Restaurants { get; }

    }
}
