using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using BusinessLogic.Factories;
using Domain;

namespace BusinessLogic.DTO
{
    public class AddressDTO
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

        public static AddressDTO CreateFromDomain(Address address)
        {
            if (address == null || !address.Active) { return null; }
            return new AddressDTO
            {
                AddressId = address.AddressId,
                AddressFirstLine = address.AddressFirstLine,
                Locality = address.Locality,
                PostCode = address.PostCode,
                Region = address.Region,
                Country = address.Country,
                LocationLongitude = address.LocationLongitude,
                LocationLatitude = address.LocationLatitude
            };
        }
        public static Address CreateFromDTO(AddressDTO address)
        {
            if (address == null ) { return null; }
            return new Address
            {
                AddressId = address.AddressId,
                AddressFirstLine = address.AddressFirstLine,
                Locality = address.Locality,
                PostCode = address.PostCode,
                Region = address.Region,
                Country = address.Country,
                LocationLongitude = address.LocationLongitude,
                LocationLatitude = address.LocationLatitude
            };
        }
    }
}
