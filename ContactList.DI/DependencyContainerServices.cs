using ContactList.Application.Contracts.Persistance;
using ContactList.Application.Contracts.Security;
using ContactList.Infrastracture.Persistance;
using ContactList.Infrastracture.Persistance.Database;
using ContactList.Infrastracture.Persistance.Database.Dapper;
using ContactList.Infrastracture.Persistance.Database.SQL;
using ContactList.Infrastracture.Persistance.Repositories;
using ContactList.Infrastracture.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ContactList.DI
{
    public static class DependencyContainerServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EFDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IDbConnectionFactory, SqlConnectionFactory>();
            services.AddScoped<IDatabaseExecuter, DapperDbExecuter>();

            //set up jwt authenticatoin setting
            var JWTSetting = configuration.GetSection("JwtSettings");
            var SecretKey = Encoding.UTF8.GetBytes(JWTSetting["SecretKey"]);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                  .AddJwtBearer(options =>
                  {
                      options.TokenValidationParameters = new TokenValidationParameters
                      {
                          ValidateIssuer = true,
                          ValidateAudience = true,
                          ValidateLifetime = true,
                          ValidIssuer = JWTSetting["Issuer"],
                          ValidAudience = JWTSetting["Audience"],
                          IssuerSigningKey = new SymmetricSecurityKey(SecretKey)
                      };

                      options.Events = new JwtBearerEvents
                      {
                          OnChallenge = context =>
                          {
                              context.Response.StatusCode = 401; // Unauthorized
                              context.Response.ContentType = "application/json";
                              return Task.CompletedTask;
                          }
                      };
                  });


            //repository services dependencies
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IContactRepository, ContactRepository>();

            //security services
            services.AddScoped<IPasswordEncryptionService, PasswordEncryptionService>();
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}
