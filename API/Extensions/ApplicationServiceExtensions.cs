using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        // "this" means it's extending the IServiceCollection
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
         IConfiguration config)
         {
            services.AddDbContext<DataContext>(opt => 
            {
                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            services.AddCors();
            //this is an interface and an implemenetation class. It's best practice to add an interface so that you can write tests more efficiently, due to it isolating where things can go wrong.
            services.AddScoped<ITokenService, TokenService>();

            return services;
         }
    }
}