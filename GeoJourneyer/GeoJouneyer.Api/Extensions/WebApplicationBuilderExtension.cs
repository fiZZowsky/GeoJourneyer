using DotNetEnv;

namespace GeoJouneyer.Api.Extensions
{
    public static class WebApplicationBuilderExtension
    {
        public static WebApplicationBuilder AddProjectConfiguration(this WebApplicationBuilder builder)
        {
            Env.TraversePath().Load();

            builder.Configuration
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            var envConnection = Environment.GetEnvironmentVariable("CONNECTION_STRING");
            if (!string.IsNullOrEmpty(envConnection))
            {
                builder.Configuration["ConnectionStrings:Default"] = envConnection;
            }

            var envJWTKey = Environment.GetEnvironmentVariable("JWT_KEY");
            var envJWTIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
            var envJWTAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");

            if (!string.IsNullOrEmpty(envJWTKey))
            {
                builder.Configuration["Jwt:Key"] = envJWTKey;
            }

            if (!string.IsNullOrEmpty(envJWTIssuer))
            {
                builder.Configuration["Jwt:Issuer"] = envJWTIssuer;
            }

            if (!string.IsNullOrEmpty(envJWTAudience))
            {
                builder.Configuration["Jwt:Audience"] = envJWTAudience;
            }

            return builder;
        }
    }
}
