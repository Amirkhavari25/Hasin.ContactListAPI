using ContactList.API.Middleware;
using ContactList.Application;
using ContactList.DI;
using ContactList.Infrastracture.Persistance;
using Microsoft.OpenApi;

namespace ContactList.API
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add infrastructure services to the container.
            builder.Services.AddInfrastructureServices(builder.Configuration);

            //add mediatR dependency
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationAssemblyReference).Assembly));

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();

            #region Swagger  Config
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT token below (no need to type 'Bearer ' prefix)."
                });

                options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                 {
                     {
                         new OpenApiSecuritySchemeReference("Bearer", document),
                         new List<string>()
                     }
                 });
            });
            #endregion
            var app = builder.Build();


            //Create database and tables automaticly 
            //note:it's not good for production ,I just do this for this project for auto creating db on running API
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider
                    .GetRequiredService<EFDbContext>();

                await dbContext.Database.EnsureCreatedAsync();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<GlobalExceptionMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            await app.RunAsync();
        }
    }
}