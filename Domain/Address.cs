using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Address
    {
        public int AddressId { get; set; }
        public string AddressFirstLine { get; set; }
        public string Locality { get; set; }
        public string PostCode { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        //foreign keys
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}
