using Domin.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domin.DTOS.Auth_DTO
{
    public class RegisterAsDriver
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Vehicles VehicleType { get; set; }

       
        [MaxLength(100)]
        public string? VehicleRegistration { get; set; }

    }
}
