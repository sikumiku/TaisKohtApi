using System;
using System.Collections.Generic;
using System.Text;
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

        public IEnumerable<RequestLog> GetAllRequestLogsForUser(string userId)
        {
            throw new NotImplementedException();
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
