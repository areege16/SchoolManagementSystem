
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SchoolManagementSystem.Application.Account.Commands;
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Infrastructure.Context;
using SchoolManagementSystem.Web.Seed;
using SchoolManagementSystem.Application;
using System.Reflection;
using SchoolManagementSystem.Application.Validations.Account;
using SchoolManagementSystem.Domain.RepositoryContract;
using SchoolManagementSystem.Web.RepositoryImplementation;
using SchoolManagementSystem.Application.AutoMapperProfile;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

namespace SchoolManagementSystem.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(RegisterCommandHandler).Assembly);
            });

            builder.Services.AddAutoMapper(typeof(SchoolManagementSystem_Profiler).Assembly);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ApplicationContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })

             .AddJwtBearer(options =>
             {
                 options.SaveToken = true;
                 options.RequireHttpsMetadata = false;
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidIssuer = builder.Configuration["JWT:Iss"],
                     ValidateAudience = true,
                     ValidAudience = builder.Configuration["JWT:Aud"],
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
                     RoleClaimType = ClaimTypes.Role,
                 };
             });

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            //builder.Services.AddValidatorsFromAssembly(SchoolManagementSystem.Application.AssemblyReference.Assembly,
            //  includeInternalTypes: true);

            //builder.Services.AddValidatorsFromAssemblyContaining<Program>();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                await RoleSeeder.SeedRoles(scopedServices);
            }
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
