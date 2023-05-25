using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebRecipesApi.BusinessLogic;
using WebRecipesApi.DAL;
using WebRecipesApi.Domain;

namespace WebRecipesApi.NUnitTest
{
    [TestFixture]
    public class RecipeServiceTests
    {
        private DbContextOptions<WebRecipesDbContext> _dbContextOptions;
        private  IConfiguration _configuration;
        private RecipeService _recipeService;
        private UserService _userService;
        private Recipe recipe = new Recipe();
        private User user = new User();
        private int Id;
        private int UserId;

        [SetUp]
        public void Setup()
        {
            // get the config from the appsettings.json file
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            _dbContextOptions = new DbContextOptionsBuilder<WebRecipesDbContext>()
            .UseSqlServer(_configuration.GetConnectionString("SqlServerCon"))
            .Options;

            user = new User()
            {
                FirstName = "Recipe",
                LastName = "Recipe",
                FullName = "Recipe Recipe",
                Email = "email@email.com",
                // pass == !ABCdef123 but need to be encrypted
                Password = "As/Wjx13Jhe3kIGhRK3uRTzWpgzNpvK4UekwycicfDJ9YFKM",
                RefreshToken = "v1u+AlP9IVM/h6JVTwjQrUsn9RWHoxWZpTMmcau8I1AV03YvYE4y9aFOZdz7GSNHWE59gGJELJ9AHX2E+swsQw==",
            };

            recipe = new Recipe
            {
                Title = "Spaghetti Bolognese",
                Description = "Classic Italian pasta dish with a rich tomato and meat sauce.",
                EstimatedTime = 30.5f,
                Difficulty = "Easy",
                MealsPerRecipe = 4,
                Rate = 4.5f,
                TotalRates = 10,
                Approved = true,
                User = user
            };

            // Add recipe ingredients
            recipe.Ingredients = new List<Ingredient>
            {
                new Ingredient { Name = "Lorem", Quantity = "Lorem" , QuantityType ="g"},
                new Ingredient { Name = "Lorem Lorem", Quantity = "Lorem", QuantityType ="g" },
                new Ingredient { Name = "Lorem", Quantity = "Lorem", QuantityType ="g" },
                new Ingredient { Name = "Lorem", Quantity = "Lorem" , QuantityType ="un"},
                new Ingredient { Name = "Lorem", Quantity = "Lorem ", QuantityType ="cloves" },
                new Ingredient { Name = "Lorem Lorem", Quantity = "Lorem ", QuantityType ="tablespoons" },
                new Ingredient { Name = "Lorem Lorem", Quantity = "Lorem ", QuantityType ="teaspoon" },
                new Ingredient { Name = "Lorem", Quantity = "Lorem", QuantityType ="x" },
                new Ingredient { Name = "Lorem Lorem", Quantity = "Lorem", QuantityType ="x" }
            };

            // Add recipe steps
            recipe.Steps = new List<Step>
            {
                new Step { StepId = 1, StepDescription = "Lorem Ipsum is simply dummy text o." },
                new Step { StepId = 2, StepDescription = "Lorem Ipsum is simply dummy text o." },
                new Step { StepId = 3, StepDescription = "Lorem Ipsum is simply dummy text o." },
                new Step { StepId = 4, StepDescription = "Lorem Ipsum is simply dummy text o." },
                new Step { StepId = 5, StepDescription = "Lorem Ipsum is simply dummy text o." }
            };


            RecipeRepository recipeRepository = new RecipeRepository(new WebRecipesDbContext(_dbContextOptions));
            _recipeService = new RecipeService(recipeRepository);

            UserRepository userRepository = new UserRepository(new WebRecipesDbContext(_dbContextOptions));
            _userService = new UserService(userRepository);



        }


        [Test, Order(0)]
        public async Task CreateUser_ReturnsUser()
        {
            // Arrange
            User user = this.user;

            // Act
            UserId = await _userService.Create(user);


            // Assert
            Assert.NotNull(user);
            Assert.That(user.Id, Is.EqualTo(UserId));
        }


        [Test, Order(1)]
        public async Task CreateRecipe_ReturnsRecipe()
        {
            // Arrange
            Recipe recipe = this.recipe;
            recipe.User = this.user;
            recipe.UserId = this.user.Id;

            // Act
            Id = await _recipeService.Create(recipe);


            // Assert
            Assert.NotNull(recipe);
            Assert.That(recipe.Id, Is.EqualTo(Id));
        }

        [Test, Order(2)]
        public async Task GetById_ReturnsRecipe()
        {
            // Arrange
            int id = this.Id;

            // Act
            Recipe recipe = await _recipeService.GetById(id);

            // Assert
            Assert.NotNull(recipe);
            Assert.That(recipe.Id, Is.EqualTo(id));
        }

        [Test, Order(3)]
        public async Task GetByUserId_ReturnsRecipes()
        {
            // Arrange
            int id = this.recipe.UserId;

            // Act
            List<Recipe> recipes = await _recipeService.GetByUserId(id);

            // Assert
            Assert.NotNull(recipes);
        }

        [Test, Order(4)]
        public async Task GetByFavUserId_ReturnsRecipes()
        {
            // Arrange
            int id = 2;

            // Act
            List<Recipe> recipes = await _recipeService.GetFavByUserId(id);

            // Assert
            Assert.NotNull(recipes);
        }

        [Test, Order(5)]
        public async Task GetByName_ReturnsRecipe()
        {
            // Arrange
            string existingName = "Spaghetti Bolognese";

            // Act
            Recipe recipe = await _recipeService.GetByName(existingName);

            // Assert
            Assert.NotNull(recipe);
        }


        [Test, Order(6)]
        public async Task Update_Recipe_ReturnsUpdatedRecipe()
        {
            // Arrange
            Recipe recipe = await _recipeService.GetById(Id);
            Recipe updatedRecipe = recipe;
            updatedRecipe.Description = "Teste";

            // Act
            int updatedRecipeCount = await _recipeService.Update(recipe, updatedRecipe);

            // Assert
            Assert.That(updatedRecipeCount, Is.EqualTo(Id));
        }


        [Test, Order(7)]
        public async Task Delete_NonExistingId_ReturnsFalse()
        {
            // Arrange
            int nonExistingId = -9;

            // Act
            bool result = await _recipeService.Delete(nonExistingId);

            // Assert
            Assert.IsFalse(result);
        }

        [Test, Order(8)]
        public async Task Delete_ExistingId_ReturnsTrue()
        {
            // Arrange
            int recipeId= Id;
            int userId = UserId;

            // Act
            bool result2 = await _userService.Delete(userId);
            bool result = await _recipeService.Delete(recipeId);


            // Assert

            Assert.IsTrue(result);
            Assert.IsTrue(result2);
        }


    }
}
