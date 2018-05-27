using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessLogic.DTO;
using DAL.TaisKoht.Interfaces;
using Domain;

namespace BusinessLogic.Services
{
    public class RequestLogService : IRequestLogService
    {

        private readonly ITaisKohtUnitOfWork _uow;

        public RequestLogService(ITaisKohtUnitOfWork uow)
        {
            _uow = uow;
        }

        public IEnumerable<RequestLogDTO> GetAllRequestLogsForUser(string userId)
        {
            return _uow.RequestLogs.All().Where(rq => rq.UserId != null && rq.UserId == userId)
                .Select(RequestLogDTO.CreateFromDomain);
        }

        public void SaveRequest(string userId, string requestMethod, string queryString, string requestName)
        {
            _uow.RequestLogs.Add(new RequestLog
            {
                UserId = userId,
                RequestMethod = requestMethod,
                QueryString = queryString,
                RequestName = requestName
            });
            _uow.SaveChanges();
        }
    }
}
