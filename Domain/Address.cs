using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Domain.Helpers;

namespace Domain
{
    public class Address : EssentialEntityBase
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
        [Column(TypeName = "decimal(9, 6)")]
        public decimal? LocationLongitude { get; set; }
        [Column(TypeName = "decimal(9, 6)")]
        public decimal? LocationLatitude { get; set; }

        //OneToMany
        public List<Restaurant> Restaurant { get; set; } = new List<Restaurant>();
    }
}
