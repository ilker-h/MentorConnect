using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using API.SignalR;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        // "this" means it's extending the IServiceCollection
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
         IConfiguration config)
         {
            services.AddCors();
            //this is an interface and an implementation class. It's best practice to add an interface so that you can write tests more efficiently, due to it isolating where things can go wrong.
            services.AddScoped<ITokenService, TokenService>();
            // services.AddScoped<IUserRepository, UserRepository>(); // scoped to the level of the HTTP Request
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<LogUserActivity>();
            services.AddSignalR();
             // a singleton is used because this needs to live throughout the whole lifetime of the app. It's not going to be destroyed once an Http Request is completed or something
            services.AddSingleton<PresenceTracker>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
         }
    }
}