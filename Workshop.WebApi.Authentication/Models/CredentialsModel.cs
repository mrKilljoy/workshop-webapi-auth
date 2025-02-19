using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Workshop.WebApi.Authentication.Models;

public class CredentialsModel
{
    [Required]
    [JsonProperty("login", Required = Required.Always)]
    public string Login { get; set; }

    [Required]
    [JsonProperty("password", Required = Required.Always)]
    public string Password { get; set; }
}