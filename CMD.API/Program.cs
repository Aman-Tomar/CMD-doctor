using System.Text;
using CMD.Data.Context;
using CMD.Data.Repositories;
using CMD.Data.Repostories;
using CMD.Domain.Managers;
using CMD.Domain.Repositories;
using CMD.Domain.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Converters;

namespace CMD.API
{
    /// <summary>
    /// The entry point of the CMD API application.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main method to configure and start the application.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            /// <summary>
            /// Configures the database context using the connection string from configuration.
            /// </summary>
            string conncetionString = builder.Configuration.GetConnectionString("Default");
            builder.Services.AddDbContext<DoctorDbContext>(options =>
                options.UseSqlServer(conncetionString)
            );

            /// <summary>
            /// Configures HttpClient for ClinicRepository with a base address.
            /// </summary>
            builder.Services.AddHttpClient<IClinicRepository, ClinicRepository>(client =>
            {
                client.BaseAddress = new Uri(""); // Specify base URI here
            });

            /// <summary>
            /// Configures HttpClient for DepartmentRepository with a base address.
            /// </summary>
            builder.Services.AddHttpClient<IDepartmentRepository, DepartmentRepository>(client =>
            {
                client.BaseAddress = new Uri(""); // Specify base URI here
            });

            /// <summary>
            /// Adds JWT Bearer Authentication service.
            /// </summary>
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                    ClockSkew = TimeSpan.Zero
                };
            });

            /// <summary>
            /// Adds support for controllers with Newtonsoft JSON serialization settings.
            /// </summary>
            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            });

            /// <summary>
            /// Adds support for API documentation using Swagger.
            /// </summary>
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            /// <summary>
            /// Registers services for dependency injection.
            /// </summary>
            builder.Services.AddTransient<IDoctorRepository, DoctorRespository>();
            builder.Services.AddTransient<IDoctorScheduleRepository, DoctorScheduleRepository>();
            builder.Services.AddTransient<IDoctorScheduleManager, DoctorScheduleManager>();
            builder.Services.AddTransient<IDoctorManager, DoctorManager>();
            builder.Services.AddTransient<IClinicRepository, ClinicRepository>();
            builder.Services.AddTransient<IDepartmentRepository, DepartmentRepository>();
            builder.Services.AddTransient<IMessageService, MessageService>();

            var app = builder.Build();

            /// <summary>
            /// Enable Swagger UI for API documentation and testing.
            /// </summary>
            app.UseSwagger();
            app.UseSwaggerUI();

            /// <summary>
            /// Configures CORS to allow any origin, header, and method.
            /// </summary>
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
            });

            /// <summary>
            /// Enforces HTTPS redirection for secure communication.
            /// </summary>
            app.UseHttpsRedirection();

            /// <summary>
            /// Enables authentication and authorization middleware.
            /// </summary>
            app.UseAuthentication();
            app.UseAuthorization();

            /// <summary>
            /// Maps controllers to the request pipeline.
            /// </summary>
            app.MapControllers();

            /// <summary>
            /// Runs the web application.
            /// </summary>
            app.Run();
        }
    }
}
