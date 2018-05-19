﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessLogic.DTO;
using BusinessLogic.Factories;
using DAL.TaisKoht.Interfaces;
using Domain;

namespace BusinessLogic.Services
{
    class RatingLogService : IRatingLogService
    {
        private readonly ITaisKohtUnitOfWork _uow;
        private readonly IRatingLogFactory _ratingLogFactory;
        public RatingLogService(ITaisKohtUnitOfWork uow, IRatingLogFactory ratingLogFactory)
        {
            _uow = uow;
            _ratingLogFactory = ratingLogFactory;
        }

        public RatingLogDTO AddNewRatingLog(RatingLogDTO ratingLogDTO)
        {
            var newRatingLog = _ratingLogFactory.Create(ratingLogDTO);
            _uow.RatingLogs.Add(newRatingLog);
            _uow.SaveChanges();
            return _ratingLogFactory.Create(newRatingLog);
        }

        public IEnumerable<RatingLogDTO> GetAllRatingLogs()
        {
            return _uow.RatingLogs.All().Where(x => x.Active)
                .Select(ratingLog => _ratingLogFactory.Create(ratingLog));
        }

        public RatingLogDTO GetRatingLogById(int id)
        {
            var ratingLog = _uow.RatingLogs.Find(id);
            if (ratingLog == null || ratingLog.Active) return null;

            return _ratingLogFactory.Create(ratingLog);
        }

        public void UpdateRatingLog(int id, RatingLogDTO updatedRatingLogDTO)
        {
            RatingLog ratingLog = _uow.RatingLogs.Find(id);
            ratingLog.Comment = updatedRatingLogDTO.Comment;
            ratingLog.Rating = updatedRatingLogDTO.Rating;
            ratingLog.RestaurantId = updatedRatingLogDTO.RestaurantId;
            ratingLog.DishId = updatedRatingLogDTO.DishId;
            ratingLog.UpdateTime = DateTime.UtcNow;
            _uow.RatingLogs.Update(ratingLog);
            _uow.SaveChanges();
        }

        public void DeleteRatingLog(int id)
        {
            RatingLog ratingLog = _uow.RatingLogs.Find(id);
            ratingLog.Active = false;
            ratingLog.UpdateTime = DateTime.UtcNow;
            _uow.RatingLogs.Update(ratingLog);
            _uow.SaveChanges();
        }
    }
}