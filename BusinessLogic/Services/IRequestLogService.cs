using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic.DTO;
using Domain;

namespace BusinessLogic.Services
{
    public interface IRequestLogService
    {
        IEnumerable<RequestLogDTO> GetAllRequestLogsForUser(string userId);
        void SaveRequest(string userId, string requestMethod, string queryString, string requestName);
    }
}
