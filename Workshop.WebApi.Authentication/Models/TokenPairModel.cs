using Newtonsoft.Json;

namespace Workshop.WebApi.Authentication.Models;

public class TokenPairModel
{
    [JsonProperty("accessToken", Required = Required.Always)]
    public string AccessToken { get; set; }

    [JsonProperty("refreshToken", Required = Required.Always)]
    public string RefreshToken { get; set; }
}