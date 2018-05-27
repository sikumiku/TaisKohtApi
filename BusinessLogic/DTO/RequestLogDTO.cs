using System;
using System.Collections.Generic;
using System.Text;
using Domain;

namespace BusinessLogic.DTO
{
    public class RequestLogDTO
    {
        public int RequestLogId { get; set; }
        public string RequestMethod { get; set; }
        public string QueryString { get; set; }
        public string RequestName { get; set; }
        public UserDTO User { get; set; }

        public static RequestLogDTO CreateFromDomain(RequestLog requestLog)
        {
            if (requestLog == null) { return null; }
            return new RequestLogDTO()
            {
                RequestLogId = requestLog.RequestLogId,
                RequestMethod = requestLog.RequestMethod,
                QueryString = requestLog.QueryString,
                RequestName = requestLog.RequestName,
                User = UserDTO.CreateFromDomain(requestLog.User)
            };
        }
    }
}
