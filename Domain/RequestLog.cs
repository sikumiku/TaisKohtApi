using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Domain.Helpers;

namespace Domain
{
    public class RequestLog : EssentialEntityBase
    {
        public int RequestLogId { get; set; }
        [MaxLength(10)]
        public string RequestMethod { get; set; }
        [MaxLength(2000)]
        public string QueryString { get; set; }
        [MaxLength(50)]
        public string RequestName { get; set; }
        //foreign keys
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
