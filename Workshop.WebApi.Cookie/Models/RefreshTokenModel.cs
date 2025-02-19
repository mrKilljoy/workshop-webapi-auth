using Newtonsoft.Json;

namespace Workshop.WebApi.Cookie.Models;

public class RefreshTokenModel
{
    [JsonProperty("refreshToken", Required = Required.Always)]
    public string RefreshToken { get; set; }
}