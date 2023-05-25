using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRecipesApi.Domain;

namespace WebRecipesApi.DAL
{
    public class WebRecipesDbContext : DbContext
    {
        public WebRecipesDbContext(DbContextOptions<WebRecipesDbContext> options) : base(options) { }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Step> Steps { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserFavoriteRecipe> UserFavoriteRecipes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recipe>()
                .HasOne(r => r.User)
                .WithMany(u => u.Recipes)
                .HasForeignKey(r => r.UserId);

        }

        public bool DatabaseExists()
        {
            return Database.CanConnect();
        }


        public void CreateData()
        {

            User Admin = new User()
            {
                FirstName = "Admin",
                LastName = "Admin",
                FullName = "Admin Admin",
                Email = "admin@mail.com",
                //PASSWORD : !ABCdef123
                Password = "F6aENnmfMwIunR0aMBpa7FPaO1IoPM49eVlwL52HuRiqkg/g",
                Role = "Admin",
                RefreshToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoiQWRtaW4iLCJ1bmlxdWVfbmFtZSI6ImFkbWluQG1haWwuY29tIiwiaWQiOjE2LCJuYmYiOjE2ODQ5MzQ0NzAsImV4cCI6MTY4NDkzODA3MCwiaWF0IjoxNjg0OTM0NDcwfQ.V_VcM2j7168-LI-L4kR5sYZNz0bf95PpF323LKF3LTQ",
                RefreshTokenExpiryTime = DateTime.Parse("30/12/2050 14:21:10")
            };

            User User = new User()
            {
                FirstName = "User",
                LastName = "User",
                FullName = "User User",
                Email = "user@mail.com",
                //PASSWORD : !ABCdef123
                Password = "Yf6wMY9mqjsyrjfRRObOn7JJS4HOaeaaq2d9tQyqdBpwVxA3",
                Role = "False",
                RefreshToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoiVXNlciIsInVuaXF1ZV9uYW1lIjoidXNlckBtYWlsLmNvbSIsImlkIjoxNywibmJmIjoxNjg0OTM0NDU0LCJleHAiOjE2ODQ5MzgwNTQsImlhdCI6MTY4NDkzNDQ1NH0.F5tWJrI5VfE7qAxJi9NyGnnlInj5OU_inq4HYwaCwJs",
                RefreshTokenExpiryTime = DateTime.Parse("30/12/2050 14:20:54")
            };
            this.Users.AddRange(Admin, User);

            // Seed recipes
            this.Recipes.AddRange(
                new Recipe
                {
                    User = Admin,
                    Title = "Chicken Parmesan",
                    Description = "Classic Italian-American dish with breaded chicken cutlets topped with tomato sauce, mozzarella, and Parmesan cheese.",
                    Difficulty = "Easy",
                    MealsPerRecipe = 4,
                    EstimatedTime = 20,
                    UserId = 1,
                    Approved = false,
                    Ingredients = new List<Ingredient>
                    {
                new Ingredient { Name = "Chicken breasts", Quantity = "4", QuantityType = "pieces"},
                new Ingredient { Name = "Breadcrumbs", Quantity = "1 cup" , QuantityType = "cup"},
                new Ingredient { Name = "Eggs", Quantity = "2" ,  QuantityType = "un"}
                    },
                    Steps = new List<Step>
                    {
                new Step { StepId = 1, StepDescription = "Preheat the oven to 375°F (190°C)." },
                new Step { StepId = 2, StepDescription = "In a shallow dish, combine the breadcrumbs and grated Parmesan cheese." },
                    },
                    Tags = new List<Tag>
                    {
                new Tag { TagName = "Italian" },
                new Tag { TagName = "Chicken" }
                    }
                },
                new Recipe
                {
                    User = Admin,
                    Title = "Spaghetti Carbonara",
                    Description = "Classic Italian pasta dish with eggs, cheese, pancetta or bacon, and black pepper.",
                    Difficulty = "Easy",
                    MealsPerRecipe = 2,
                    EstimatedTime = 50,
                    UserId = 1,
                    Approved = false,
                    Ingredients = new List<Ingredient>
                    {
                new Ingredient { Name = "Spaghetti", Quantity = "8 un" },
                new Ingredient { Name = "Eggs", Quantity = "2" },
                new Ingredient { Name = "Pancetta or bacon", Quantity = "4 un" }
                    },
                    Steps = new List<Step>
                    {
                new Step { StepId = 1, StepDescription = "Cook the spaghetti according to package instructions. Drain and set aside." },
                new Step { StepId = 2, StepDescription = "In a bowl, whisk together the eggs and grated Parmesan cheese." },
                    },
                    Tags = new List<Tag>
                    {
                new Tag { TagName = "Italian" },
                new Tag { TagName = "Pasta" }
                    }
                }
            );

            this.SaveChanges();
        }
    }
    }

