﻿namespace Workshop.WebApi.Authentication.Infrastructure;

public static class Constants
{
    public static class Authentication
    {
        public const string CookieSchemaName = "cookie-auth";
        public const string JwtSchemaName = "jwt-bearer-auth";
        public const string JwtIssuer = "secret-issuer";
        public const string JwtAudience = "secret-audience";
        public const string DataProtectorName = "cookie-protector";
        public const string PolicyName = "test-auth-policy";
        
        public static class Claims
        {
            public const string TestClaimName = "pants_color";
            public const string TestClaimValue = "yellow";
        }
    }
    
    public static class Data
    {
        public const string DatabaseName = "users_db";
    }
    
    public static class ContentType
    {
        public const string Json = "application/json";
        public const string PlainText = "text/plain";
    }
    
    public static class Logging
    {
        public const string ConfigurationState = nameof(ConfigurationState);
    }
}