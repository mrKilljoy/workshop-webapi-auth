namespace Workshop.WebApi.Cookie.Infrastructure;

public static class Constants
{
    public static class Authentication
    {
        public const string Cookie = "cookie-auth";
        public const string DataProtectorName = "cookie-protector";
    }
    
    public static class Data
    {
        public const string DatabaseName = "users_db";
    }
    
    public static class Configuration
    {
        public const string DataSourceSection = "DataSource";
    }
}