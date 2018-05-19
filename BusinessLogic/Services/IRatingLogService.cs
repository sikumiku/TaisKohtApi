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

        RatingLogDTO AddNewRatingLog(RatingLogDTO dto);

        void UpdateRatingLog(int id, RatingLogDTO dto);

        void DeleteRatingLog(int id);
    }
}
