
using BookSwap.Data;
using BookSwap.Hubs;
using BookSwap.Interfaces;
using BookSwap.Models;
using BookSwap.Repository;
using BookSwap.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace BookSwap
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("cs"));

            });
            // Rgister Reposistory classes
            builder.Services.AddScoped<BookPostRepository>();
            builder.Services.AddScoped<BorrowRequestRepository>();
            builder.Services.AddScoped<CommentRepository>();
            builder.Services.AddScoped<LikeRepository>();
            // register signalr service

            builder.Services.AddSignalR();


            //  Register ----> Identity: IdentityUser , IdentityRole
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
               
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers = true;
            }).AddEntityFrameworkStores<AppDbContext>();

            builder.Services.AddScoped< ITokenService, TokenService > ();

            // To make JWT the default
            builder.Services.AddAuthentication(options => {
                //Check JWT Token Header
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //[authrize]
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;//unauth
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>//verified key
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"])),
                    RoleClaimType = ClaimTypes.Role

                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/bookhub"))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });



            // cors policy settings
            builder.Services.AddCors(options=>
            {
                options.AddPolicy("MyPolicy", policy =>
                {
                    policy.WithOrigins("http://localhost:3000").
                    AllowAnyHeader().
                    AllowAnyMethod()
                   .AllowCredentials();
                }
                );
            } );
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "BookSwap API", Version = "v1" });

                // ⬇️ JWT Setup
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your token"
                });

                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
            });


            var app = builder.Build();
        


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles();

            app.UseCors("MyPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
          

            app.MapControllers();
            app.MapHub<BookHub>("/bookhub");

            app.Run();
        }
    }
}
