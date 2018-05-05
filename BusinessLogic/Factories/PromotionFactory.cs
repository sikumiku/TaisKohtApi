using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic.DTO;
using Domain;

namespace BusinessLogic.Factories
{
    public interface IPromotionFactory
    {
        PromotionDTO Create(Promotion promotion);
        Promotion Create(PromotionDTO promotionDTO);
    }

    public class PromotionFactory : IPromotionFactory
    {
        public PromotionDTO Create(Promotion promotion)
        {
            return PromotionDTO.CreateFromDomainWithAssociatedTables(promotion);
        }

        public Promotion Create(PromotionDTO promotionDTO)
        {
            return new Promotion
            {
                PromotionId = promotionDTO.PromotionId,
                Name = promotionDTO.Name,
                Description = promotionDTO.Description,
                Type = promotionDTO.Type,
                ValidTo = promotionDTO.ValidTo,
                AddTime = promotionDTO.AddTime,
                UpdateTime = promotionDTO.UpdateTime,
                Active = promotionDTO.Active
            };
        }
    }
}
