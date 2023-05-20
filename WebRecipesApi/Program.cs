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
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(option =>
            {
                option.AddPolicy("Policy", builder =>
                {
                    builder.
                    AllowAnyOrigin().
                    AllowAnyMethod().
                    AllowAnyHeader();
                });
            });

            builder.Services.AddDbContext<WebRecipesDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerCon"));
                option.UseSqlServer(b => b.MigrationsAssembly("WebRecipesApi"));
            });
            builder.Services.AddScoped<CommentRepository>();
            builder.Services.AddScoped<CommentService>();

            builder.Services.AddScoped<IngredientRepository>();
            builder.Services.AddScoped<IngredientService>();

            builder.Services.AddScoped<RecipeRepository>();
            builder.Services.AddScoped<RecipeService>();

            builder.Services.AddScoped<StepRepository>();
            builder.Services.AddScoped<StepService>();

            builder.Services.AddScoped<TagRepository>();
            builder.Services.AddScoped<TagService>();

            builder.Services.AddScoped<UserRepository>();
            builder.Services.AddScoped<UserService>();

            builder.Services.AddScoped<UserFavRecipeRepository>();
            builder.Services.AddScoped<UserFavoriteRecipeService>();


            builder.Services.AddScoped<JWTService>();

            builder.Services.AddScoped<WebRecipesDbContext>();

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