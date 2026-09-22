using ContactList.Infrastracture.Persistance;
using ContactList.Infrastracture.Persistance.Database;
using ContactList.Infrastracture.Persistance.Database.Dapper;
using ContactList.Infrastracture.Persistance.Database.SQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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


            return services;
        }
    }
}
