using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
    public class Address
    {
        public int AddressId { get; set; }
        [MaxLength(50)]
        public string AddressFirstLine { get; set; }
        [MaxLength(50)]
        public string Locality { get; set; }
        [MaxLength(20)]
        public string PostCode { get; set; }
        [MaxLength(50)]
        public string Region { get; set; }
        [MaxLength(50)]
        public string Country { get; set; }
        public DateTime AddTime { get; set; }
        public DateTime UpdateTime { get; set; }

        //OneToMany
        public List<Restaurant> Restaurant { get; set; } = new List<Restaurant>();
    }
}
