using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Workshop.WebApi.Authentication.Infrastructure.Configuration;

public class DataSourceOptions
{
    public const string SectionName = "DataSource";
    
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