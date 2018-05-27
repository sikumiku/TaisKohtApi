using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic.DTO;

namespace BusinessLogic.Services
{
    public interface IRatingLogService
    {
        IEnumerable<RatingLogDTO> GetAllRatingLogs();

        RatingLogDTO GetRatingLogById(int id);

        RatingLogDTO GetRestaurantRatingLog(int? restaurantId, string userId);

        RatingLogDTO GetDishRatingLog(int? dishId, string userId);

        RatingLogDTO AddNewRatingLog(RatingLogForEntityDTO ratingLogDTO, string userId);

        RatingLogDTO UpdateRatingLog(int id, RatingLogForEntityDTO dto);

        void DeleteRatingLog(int id);
    }
}
