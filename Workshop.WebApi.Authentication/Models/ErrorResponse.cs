using Newtonsoft.Json;

namespace Workshop.WebApi.Authentication.Models;

public class ErrorResponse
{
    [JsonProperty("error", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string Error { get; set; }

    [JsonProperty("httpCode")]
    public int HttpCode { get; set; }
}