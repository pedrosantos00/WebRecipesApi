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
    public class UserServiceTests
    {
        private DbContextOptions<WebRecipesDbContext> _dbContextOptions;
        private  IConfiguration _configuration;
        private UserService _userService;
        private RecipeService _recipeService;
        private User user = new User();
        private int Id;

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
                FirstName = "User",
                LastName = "Tester",
                FullName = "User Tester",
                Email = "email@email.com",
                // pass == !ABCdef123 but need to be encrypted
                Password = "As/Wjx13Jhe3kIGhRK3uRTzWpgzNpvK4UekwycicfDJ9YFKM",
                RefreshToken = "v1u+AlP9IVM/h6JVTwjQrUsn9RWHoxWZpTMmcau8I1AV03YvYE4y9aFOZdz7GSNHWE59gGJELJ9AHX2E+swsQw==",
            };


            RecipeRepository recipeRepository = new RecipeRepository(new WebRecipesDbContext(_dbContextOptions));
            _recipeService = new RecipeService(recipeRepository);

            UserRepository userRepository = new UserRepository(new WebRecipesDbContext(_dbContextOptions));
            _userService = new UserService(userRepository, recipeRepository);



        }


        [Test, Order(1)]
        public async Task CreateUser_ReturnsUser()
        {
            // Arrange
            User user = this.user;

            // Act
            Id = await _userService.Create(user);


            // Assert
            Assert.NotNull(user);
            Assert.That(user.Id, Is.EqualTo(Id));
        }

        [Test, Order(2)]
        public async Task GetById_ReturnsUser()
        {
            // Arrange
            int id = this.Id;

            // Act
            User user = await _userService.GetById(id);

            // Assert
            Assert.NotNull(user);
            Assert.That(user.Id, Is.EqualTo(id));
        }

        [Test, Order(3)]
        public async Task GetByName_ReturnsUser()
        {
            // Arrange
            string existingName = "User Tester";

            // Act
            User user = await _userService.GetByName(existingName);

            // Assert
            Assert.NotNull(user);
            Assert.That(user.FullName, Is.EqualTo(existingName));
        }

        [Test, Order(4)]
        public async Task GetByEmail_ReturnsUser()
        {
            // Arrange
            string existingEmail = "email@email.com";

            // Act
            User user = await _userService.GetByEmail(existingEmail);

            // Assert
            Assert.NotNull(user);
            Assert.That(user.Email, Is.EqualTo(existingEmail));
        }

        [Test, Order(5)]
        public async Task Search_ReturnsMatchingUsers()
        {
            // Arrange
            string filterWord = "User Tester";

            // Act
            List<User> userList = await _userService.Search(filterWord);

            // Assert
            Assert.NotNull(userList);
            Assert.That(userList.Count, Is.EqualTo(1));
            Assert.That(userList.First().FullName, Contains.Substring(filterWord));
        }

        [Test, Order(6)]
        public async Task Update_UserAndUpdatedUser_ReturnsUpdatedUserCount()
        {
            // Arrange
            User user = await _userService.GetById(Id);
            User updatedUser = user;
            updatedUser.FullName = "Teste";

            // Act
            int updatedUserCount = await _userService.Update(user, updatedUser);

            // Assert
            Assert.That(updatedUserCount, Is.EqualTo(Id));
        }

        [Test, Order(7)]
        public async Task CheckEmailExistsAsync_ExistingEmail_ReturnsTrue()
        {
            // Arrange
            string existingEmail = "email@email.com";

            // Act
            bool result = await _userService.CheckEmailExistsAsync(existingEmail);

            // Assert
            Assert.IsTrue(result);
        }

        [Test, Order(8)]
        public async Task CheckEmailExistsAsync_NonExistingEmail_ReturnsFalse()
        {
            // Arrange
            string nonExistingEmail = "nonexistent@example.com";

            // Act
            bool result = await _userService.CheckEmailExistsAsync(nonExistingEmail);

            // Assert
            Assert.IsFalse(result);
        }

        [Test, Order(9)]
        public void CheckPasswordStrength_ValidPassword_ReturnsEmptyString()
        {
            // Arrange
            string validPassword = "StrongPassword@123";

            // Act
            string result = _userService.CheckPasswordStrength(validPassword);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test, Order(10)]
        public void CheckPasswordStrength_InvalidPassword_ReturnsErrorMessage()
        {
            // Arrange
            string invalidPassword = "123";

            // Act
            string result = _userService.CheckPasswordStrength(invalidPassword);

            // Assert
            Assert.That(result, Is.Not.Empty);
        }

        [Test, Order(11)]
        public void RefreshTokenExists_ExistingRefreshToken_ReturnsTrue()
        {
            // Arrange
            string existingRefreshToken = "v1u+AlP9IVM/h6JVTwjQrUsn9RWHoxWZpTMmcau8I1AV03YvYE4y9aFOZdz7GSNHWE59gGJELJ9AHX2E+swsQw==";

            // Act
            bool result = _userService.RefreshTokenExists(existingRefreshToken);

            // Assert
            Assert.IsTrue(result);
        }

        [Test, Order(12)]
        public void RefreshTokenExists_NonExistingRefreshToken_ReturnsFalse()
        {
            // Arrange
            string nonExistingRefreshToken = "nonexistentToken";

            // Act
            bool result = _userService.RefreshTokenExists(nonExistingRefreshToken);

            // Assert
            Assert.IsFalse(result);
        }

        [Test, Order(13)]
        public async Task Delete_NonExistingId_ReturnsFalse()
        {
            // Arrange
            int nonExistingId = -9;

            // Act
            bool result = await _userService.Delete(nonExistingId);

            // Assert
            Assert.IsFalse(result);
        }

        [Test, Order(14)]
        public async Task Delete_ExistingId_ReturnsTrue()
        {
            // Arrange
            int id = this.Id;

            // Act
            bool result = await _userService.Delete(id);

            // Assert
            Assert.IsTrue(result);
        }

    }
}
