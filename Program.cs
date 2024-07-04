using LibraryManageSystem.Database;

using Microsoft.AspNetCore.Identity;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LibraryManageSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);
         

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                         options.UseMySql(
                             builder.Configuration.GetConnectionString("DefaultConnection"),
                             new MySqlServerVersion(new Version(8,4,1))
                             ));
            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme; 
            }).AddCookie(IdentityConstants.ApplicationScheme).AddBearerToken(IdentityConstants.BearerScheme); 


            builder.Services.AddIdentityCore<User>().AddEntityFrameworkStores<ApplicationDbContext>().AddApiEndpoints();
            builder.Services.AddSwaggerGen();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "LibraryManageSystem API V1");
                c.RoutePrefix = string.Empty; 
            });

            app.MapGet("/", () => "Hello World!");
            app.MapGet("users/me", async (ClaimsPrincipal claims, ApplicationDbContext context) =>
            {
                string userId = claims.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                return await context.Users.FindAsync(userId);
            }).RequireAuthorization();

            

            app.MapIdentityApi<User>();
            app.Run();
        }

    }
}
