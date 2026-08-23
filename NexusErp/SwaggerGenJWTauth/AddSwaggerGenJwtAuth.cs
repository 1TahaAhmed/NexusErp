using Microsoft.OpenApi;

namespace NexusErp.API.SwaggerGenJWTauth
{
    public static class SwaggerExtensions
    {
        public static void AddSwaggerGenJwtAuth(this IServiceCollection services)
        {
            services.AddSwaggerGen(o =>
            {
                o.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Version = "v1",
                    Title = "NexusErp API",
                    Description = "Testing my Api",
                    Contact = new OpenApiContact()
                    {
                        Name = "Taha Ahmed",
                        Email = "tahaahmed@gmail.com",
                        Url = new Uri("https://mydomain.com")
                    }
                });

                var scheme = new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter the JWT"
                };

                o.AddSecurityDefinition("Bearer", scheme);

                o.AddSecurityRequirement(doc => new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecuritySchemeReference("Bearer", doc),
                        new List<string>()
                    }
                });
            });
        }
    }
}