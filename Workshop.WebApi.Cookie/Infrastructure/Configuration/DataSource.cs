using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Workshop.WebApi.Cookie.Infrastructure.Configuration;

public class DataSource
{
    [JsonConverter(typeof(StringEnumConverter))]
    public DataSourceType Type { get; set; }

    public bool UseTestData { get; set; }

    public string ConnectionString { get; set; }
}

public enum DataSourceType
{
    [EnumMember(Value = "inmemory")]
    InMemory,
    [EnumMember(Value = "mssql")]
    MSSQL
}