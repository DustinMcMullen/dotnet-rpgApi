using System.Text.Json.Serialization;

namespace dotnet_rpgApi.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RpgClass
    {
        Warrior = 1,
        Wizard = 2,
        Healer = 3
    }
}