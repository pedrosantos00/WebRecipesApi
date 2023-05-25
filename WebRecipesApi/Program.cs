using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebRecipesApi.DAL;
using WebRecipesApi.BusinessLogic;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebRecipesApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Configure Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configure CORS policy
            builder.Services.AddCors(option =>
            {
                option.AddPolicy("Policy", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            // Configure DbContext
            builder.Services.AddDbContext<WebRecipesDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerCon"));
                option.UseSqlServer(b => b.MigrationsAssembly("WebRecipesApi"));
            });

            // Register repositories
            builder.Services.AddScoped<RecipeRepository>();
            builder.Services.AddScoped<UserRepository>();
            builder.Services.AddScoped<UserFavRecipeRepository>();

            // Register services
            builder.Services.AddScoped<RecipeService>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<UserFavoriteRecipeService>();


            // Register JWT service
            builder.Services.AddScoped<JWTService>();

            // Register DbContext
            builder.Services.AddScoped<WebRecipesDbContext>();

            // Configure authentication and JWT Bearer
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("NxgW6KsCk6O0XpQdvnuy16jfRk5ceag9ZhjgERymnhI=")),
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ClockSkew = TimeSpan.Zero
                };
            });

            var app = builder.Build();


            // check if db exists, if not, create it and add data to it
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<WebRecipesDbContext>();
                if (!dbContext.DatabaseExists())
                {
                    dbContext.Database.Migrate();
                    dbContext.CreateData();
                }
            }


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("Policy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
