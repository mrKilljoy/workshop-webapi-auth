using Newtonsoft.Json;

namespace Workshop.WebApi.Authentication.Models;

public class ResourceResponse : ErrorResponse
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("content")]
    public string Content { get; set; }
}