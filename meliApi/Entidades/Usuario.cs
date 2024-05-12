using meliApi.DTOs;
using System.Net;
using System;
using System.ComponentModel.DataAnnotations;

namespace meliApi.Entidades
{
    using System.ComponentModel.DataAnnotations;

    namespace meliApi.Entidades
    {
        public class Usuario
        {
            [Key]
            public int Id { get; set; }
            public string? Nickname { get; set; }
            public string? CountryId { get; set; }
            public string? Address { get; set; }
            public string? UserType { get; set; }
            public string? SiteId { get; set; }
            public string? Permalink { get; set; }
            public string? SellerReputation { get; set; }
            public string? Status { get; set; }
        }
    }

}
