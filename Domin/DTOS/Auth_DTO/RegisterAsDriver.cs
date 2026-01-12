using Domin.Enum;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
