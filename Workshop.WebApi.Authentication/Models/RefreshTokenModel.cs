using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Workshop.WebApi.Authentication.Models;

public class RefreshTokenModel
{
    [Required]
    [JsonProperty("refreshToken", Required = Required.Always)]
    public string RefreshToken { get; set; }
}