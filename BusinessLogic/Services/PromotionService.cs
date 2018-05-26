using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessLogic.DTO;
using BusinessLogic.Factories;
using DAL.TaisKoht.Interfaces;
using Domain;

namespace BusinessLogic.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly ITaisKohtUnitOfWork _uow; 
        private readonly IPromotionFactory _promotionFactory;

        public PromotionService(ITaisKohtUnitOfWork uow, IPromotionFactory promotionFactory)
        {
            _uow = uow;
            _promotionFactory = promotionFactory;
        }

        public PromotionDTO AddNewPromotion(PromotionDTO promotionDTO)
        {
            var newPromotion = _promotionFactory.Create(promotionDTO);
            _uow.Promotions.Add(newPromotion);
            _uow.SaveChanges();
            return _promotionFactory.CreateComplex(newPromotion);
        }

        public IEnumerable<PromotionDTO> GetAllPromotions()
        {
            return _uow.Promotions.All()
                .Select(promotion => _promotionFactory.Create(promotion));
        }

        public PromotionDTO GetPromotionById(int id)
        {
            var promotion = _uow.Promotions.Find(id);
            if (promotion == null) return null;

            return _promotionFactory.CreateComplex(promotion);
        }

        public PromotionDTO UpdatePromotion(int id, PromotionDTO updatedPromotionDTO)
        {
            if (_uow.Promotions.Exists(id))
            {
                Promotion promotion = _uow.Promotions.Find(id);
                promotion.Name = updatedPromotionDTO.Name;
                promotion.Description = updatedPromotionDTO.Description;
                promotion.Type = updatedPromotionDTO.Type;
                promotion.ValidTo = updatedPromotionDTO.ValidTo;
                promotion.UpdateTime = DateTime.UtcNow;
                _uow.Promotions.Update(promotion);
                _uow.SaveChanges();
            }

            return GetPromotionById(id);
        }

        public void DeletePromotion(int id)
        {
            Promotion promotion = _uow.Promotions.Find(id);
            _uow.Promotions.Remove(promotion);
            _uow.SaveChanges();
        }
    }
}
