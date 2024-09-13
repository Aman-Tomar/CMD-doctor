
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

namespace CMD.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Configure Database
            string conncetionString = builder.Configuration.GetConnectionString("Default");
            builder.Services.AddDbContext<DoctorDbContext>(options =>
                options.UseSqlServer(conncetionString)
            );

            
            // Configure HttpClient for ClinicRepository
            builder.Services.AddHttpClient<IClinicRepository, ClinicRepository>(client =>
            {
                client.BaseAddress = new Uri("");
            });

            // Configure HttpClient for DepartmentRepository
            builder.Services.AddHttpClient<IDepartmentRepository, DepartmentRepository>(client =>
            {
                client.BaseAddress = new Uri("");
            });

            // Add Authentication
            //builder.Services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(options =>
            //{
            //    options.RequireHttpsMetadata = false;
            //    options.SaveToken = true;
            //    options.TokenValidationParameters = new TokenValidationParameters()
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateLifetime = true,
            //        ValidateIssuerSigningKey = true,
            //        ValidIssuer = builder.Configuration["Jwt:Issuer"],
            //        ValidAudience = builder.Configuration["Jwt:Audience"],
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            //        ClockSkew = TimeSpan.Zero
            //    };
            //});

            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddTransient<IDoctorRepository,DoctorRespository>();
            builder.Services.AddTransient<IDoctorScheduleRepository,DoctorScheduleRepository>();
            builder.Services.AddTransient<IDoctorScheduleManager,DoctorScheduleManager>();
            builder.Services.AddTransient<IDoctorManager, DoctorManager>();
            builder.Services.AddTransient<IClinicRepository, ClinicRepository>();
            builder.Services.AddTransient<IDepartmentRepository, DepartmentRepository>();
            builder.Services.AddTransient<IMessageService, MessageService>();
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Add cors
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
            });

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
