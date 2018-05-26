using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic.DTO;

namespace BusinessLogic.Services
{
    public interface IPromotionService
    {
        IEnumerable<PromotionDTO> GetAllPromotions();

        PromotionDTO GetPromotionById(int id);

        PromotionDTO AddNewPromotion(PromotionDTO dto);

        PromotionDTO UpdatePromotion(int id, PromotionDTO dto);

        void DeletePromotion(int id);
    }
}
