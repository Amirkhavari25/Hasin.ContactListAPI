using ContactList.DI;
using ContactList.Infrastracture.Persistance;
using Microsoft.EntityFrameworkCore;

namespace ContactList.API
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add infrastructure services to the container.
            builder.Services.AddInfrastructureServices(builder.Configuration);

            


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            //automatic jobs during running app for the first time
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                //Auto create database if not exist
                var dbContext = services.GetRequiredService<EFDbContext>();
                await dbContext.Database.MigrateAsync();
            }


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            await app.RunAsync();
        }
    }
}