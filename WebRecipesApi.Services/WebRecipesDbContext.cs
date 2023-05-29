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
                RefreshTokenExpiryTime = DateTime.Now.AddMonths(1)
            };

            User User = new User()
            {
                FirstName = "User",
                LastName = "User",
                FullName = "User User",
                Email = "user@mail.com",
                //PASSWORD : !ABCdef123
                Password = "Yf6wMY9mqjsyrjfRRObOn7JJS4HOaeaaq2d9tQyqdBpwVxA3",
                Role = "User",
                RefreshToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoiVXNlciIsInVuaXF1ZV9uYW1lIjoidXNlckBtYWlsLmNvbSIsImlkIjoxNywibmJmIjoxNjg0OTM0NDU0LCJleHAiOjE2ODQ5MzgwNTQsImlhdCI6MTY4NDkzNDQ1NH0.F5tWJrI5VfE7qAxJi9NyGnnlInj5OU_inq4HYwaCwJs",
                RefreshTokenExpiryTime = DateTime.Now.AddMonths(1)
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
                    Approved = true,
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient { Name = "Chicken breasts", Quantity = "4", QuantityType = "pieces" },
                        new Ingredient { Name = "Breadcrumbs", Quantity = "1", QuantityType = "cup" },
                        new Ingredient { Name = "Eggs", Quantity = "2", QuantityType = "un" },
                        new Ingredient { Name = "Tomato sauce", Quantity = "1", QuantityType = "cup" },
                        new Ingredient { Name = "Mozzarella cheese", Quantity = "1", QuantityType = "cup" },
                        new Ingredient { Name = "Grated Parmesan cheese", Quantity = "1/2", QuantityType = "cup" },
                        new Ingredient { Name = "Salt", Quantity = "1/2", QuantityType = "tea spoon" },
                        new Ingredient { Name = "Black pepper", Quantity = "1/4", QuantityType = "tea spoon" },
                        new Ingredient { Name = "Olive oil", Quantity = "2", QuantityType = "tea spoon" }
                    },
                    Steps = new List<Step>
                    {
                        new Step { StepId = 1, StepDescription = "Preheat the oven to 375°F (190°C)." },
                        new Step { StepId = 2, StepDescription = "In a shallow dish, combine the breadcrumbs and grated Parmesan cheese." },
                        new Step { StepId = 3, StepDescription = "Dip each chicken breast in beaten eggs, then coat it with the breadcrumb mixture." },
                        new Step { StepId = 4, StepDescription = "Heat olive oil in a skillet over medium-high heat. Cook the breaded chicken cutlets until golden brown on both sides." },
                        new Step { StepId = 5, StepDescription = "Place the cooked chicken cutlets in a baking dish. Top each cutlet with tomato sauce, mozzarella cheese, and grated Parmesan cheese." },
                        new Step { StepId = 6, StepDescription = "Bake in the preheated oven for about 10 minutes or until the cheese is melted and bubbly." },
                        new Step { StepId = 7, StepDescription = "Serve hot with pasta or salad." }
                    },
                    Tags = new List<Tag>
                    {
                        new Tag { TagName = "Italian" },
                        new Tag { TagName = "Chicken" },
                        new Tag { TagName = "Baked" },
                        new Tag { TagName = "Main dish" }
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
                    Approved = true,
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient { Name = "Spaghetti", Quantity = "8", QuantityType = "gr" },
                        new Ingredient { Name = "Eggs", Quantity = "3", QuantityType = "un" },
                        new Ingredient { Name = "Pancetta or bacon", Quantity = "4", QuantityType = "gr" },
                        new Ingredient { Name = "Grated Parmesan cheese", Quantity = "1/2", QuantityType = "cup" },
                        new Ingredient { Name = "Salt", Quantity = "1/2", QuantityType = "tea spoon" },
                        new Ingredient { Name = "Black pepper", Quantity = "1/4", QuantityType = "tea spoon" },
                        new Ingredient { Name = "Olive oil", Quantity = "2", QuantityType = "tea spoon" }
                    },
                    Steps = new List<Step>
                    {
                        new Step { StepId = 1, StepDescription = "Cook the spaghetti according to package instructions. Drain and set aside." },
                        new Step { StepId = 2, StepDescription = "In a skillet, cook the pancetta or bacon until crispy. Remove from heat and set aside." },
                        new Step { StepId = 3, StepDescription = "In a bowl, whisk together the eggs, grated Parmesan cheese, salt, and black pepper." },
                        new Step { StepId = 4, StepDescription = "Heat olive oil in the skillet. Add the cooked spaghetti and pancetta or bacon. Stir to combine." },
                        new Step { StepId = 5, StepDescription = "Pour the egg mixture over the spaghetti. Stir quickly to coat the pasta evenly and cook for a minute or two until the eggs are cooked but still creamy." },
                        new Step { StepId = 6, StepDescription = "Remove from heat and serve immediately. Garnish with additional grated Parmesan cheese and black pepper, if desired." }
                    },
                    Tags = new List<Tag>
                    {
                        new Tag { TagName = "Italian" },
                        new Tag { TagName = "Pasta" },
                        new Tag { TagName = "Creamy" },
                        new Tag { TagName = "Main dish" }
                    }
                },
                new Recipe
                {
                    User = Admin,
                    Title = "Chocolate Brownies",
                    Description = "Rich and fudgy chocolate brownies with a melt-in-your-mouth texture.",
                    Difficulty = "Easy",
                    MealsPerRecipe = 12,
                    EstimatedTime = 40,
                    UserId = 1,
                    Approved = true,
                    Ingredients = new List<Ingredient>
            {
                new Ingredient { Name = "Unsalted butter", Quantity = "1", QuantityType = "cup" },
                new Ingredient { Name = "Granulated sugar", Quantity = "2", QuantityType = "cup" },
                new Ingredient { Name = "Eggs", Quantity = "4", QuantityType = "un" },
                new Ingredient { Name = "Vanilla extract", Quantity = "2", QuantityType = "tea spoon" },
                new Ingredient { Name = "All-purpose flour", Quantity = "1", QuantityType = "cup" },
                new Ingredient { Name = "Cocoa powder", Quantity = "1/2", QuantityType = "cup" },
                new Ingredient { Name = "Salt", Quantity = "1/4", QuantityType = "tea spoon" },
                new Ingredient { Name = "Chocolate chips", Quantity = "1", QuantityType = "cup" },
                new Ingredient { Name = "Chopped nuts (optional)", Quantity = "1/2", QuantityType = "cup" },
            },
                    Steps = new List<Step>
            {
                new Step { StepId = 1, StepDescription = "Preheat the oven to 350°F (175°C). Grease a 9x13-inch baking dish." },
                new Step { StepId = 2, StepDescription = "In a microwave-safe bowl, melt the butter. Stir in the sugar until well combined." },
                new Step { StepId = 3, StepDescription = "Add the eggs, one at a time, mixing well after each addition. Stir in the vanilla extract." },
                new Step { StepId = 4, StepDescription = "In a separate bowl, whisk together the flour, cocoa powder, and salt. Gradually add the dry ingredients to the wet ingredients, mixing until just combined." },
                new Step { StepId = 5, StepDescription = "Fold in the chocolate chips and chopped nuts, if using." },
                new Step { StepId = 6, StepDescription = "Pour the batter into the prepared baking dish and spread it evenly." },
                new Step { StepId = 7, StepDescription = "Bake for 25-30 minutes, or until a toothpick inserted into the center comes out with a few moist crumbs." },
                new Step { StepId = 8, StepDescription = "Allow the brownies to cool completely before cutting into squares." },
            },
                    Tags = new List<Tag>
            {
                new Tag { TagName = "Dessert" },
                new Tag { TagName = "Chocolate" },
                new Tag { TagName = "Baking" },
            }
                },
        new Recipe
        {
            User = Admin,
            Title = "Caprese Salad",
            Description = "A simple and refreshing salad with ripe tomatoes, fresh mozzarella, basil, and balsamic glaze.",
            Difficulty = "Easy",
            MealsPerRecipe = 4,
            EstimatedTime = 15,
            UserId = 1,
            Approved = true,
            Ingredients = new List<Ingredient>
            {
                new Ingredient { Name = "Ripe tomatoes", Quantity = "4", QuantityType = "un" },
                new Ingredient { Name = "Fresh mozzarella", Quantity = "8", QuantityType = "gr" },
                new Ingredient { Name = "Fresh basil leaves", Quantity = "1", QuantityType = "cup" },
                new Ingredient { Name = "Extra virgin olive oil", Quantity = "2", QuantityType = "tea spoon" },
                new Ingredient { Name = "Balsamic glaze", Quantity = "2", QuantityType = "tea spoon" },
                new Ingredient { Name = "Salt", Quantity = "1/4", QuantityType = "tea spoon" },
                new Ingredient { Name = "Black pepper", Quantity = "1/4", QuantityType = "tea spoon" },
            },
            Steps = new List<Step>
            {
                new Step { StepId = 1, StepDescription = "Slice the tomatoes and fresh mozzarella into thick rounds." },
                new Step { StepId = 2, StepDescription = "Arrange the tomato and mozzarella slices on a platter, alternating them." },
                new Step { StepId = 3, StepDescription = "Tuck fresh basil leaves between the tomato and mozzarella slices." },
                new Step { StepId = 4, StepDescription = "Drizzle extra virgin olive oil and balsamic glaze over the salad." },
                new Step { StepId = 5, StepDescription = "Season with salt and black pepper to taste." },
                new Step { StepId = 6, StepDescription = "Serve immediately as a refreshing appetizer or side dish." },
            },
            Tags = new List<Tag>
            {
                new Tag { TagName = "Appetizer" },
                new Tag { TagName = "Salad" },
                new Tag { TagName = "Italian" },
                new Tag { TagName = "Vegetarian" },
            }
        },
        new Recipe
        {
            User = Admin,
            Title = "Vegan Lentil Curry",
            Description = "A flavorful and hearty lentil curry made with a blend of aromatic spices and creamy coconut milk.",
            Difficulty = "Intermediate",
            MealsPerRecipe = 4,
            EstimatedTime = 60,
            UserId = 1,
            Approved = true,
            Ingredients = new List<Ingredient>
            {
                new Ingredient { Name = "Red lentils", Quantity = "1", QuantityType = "cup" },
                new Ingredient { Name = "Onion", Quantity = "1", QuantityType = "un" },
                new Ingredient { Name = "Garlic cloves", Quantity = "3", QuantityType = "un" },
                new Ingredient { Name = "Ginger", Quantity = "1-inch", QuantityType = "piece" },
                new Ingredient { Name = "Curry powder", Quantity = "2", QuantityType = "tea spoon" },
                new Ingredient { Name = "Cumin", Quantity = "1", QuantityType = "tea spoon" },
                new Ingredient { Name = "Turmeric", Quantity = "1/2", QuantityType = "tea spoon" },
                new Ingredient { Name = "Coconut milk", Quantity = "1", QuantityType = "can" },
                new Ingredient { Name = "Vegetable broth", Quantity = "1", QuantityType = "cup" },
                new Ingredient { Name = "Spinach", Quantity = "2", QuantityType = "cup" },
                new Ingredient { Name = "Lime juice", Quantity = "1", QuantityType = "tea spoon" },
                new Ingredient { Name = "Salt", Quantity = "1/2", QuantityType = "tea spoon" },
                new Ingredient { Name = "Cilantro (optional)", Quantity = "1/4", QuantityType = "cup" },
            },
            Steps = new List<Step>
            {
                new Step { StepId = 1, StepDescription = "Rinse the red lentils thoroughly and set aside." },
                new Step { StepId = 2, StepDescription = "In a large pot, heat some oil over medium heat. Add the chopped onion and sauté until translucent." },
                new Step { StepId = 3, StepDescription = "Add the minced garlic and grated ginger to the pot. Cook for another minute, stirring constantly." },
                new Step { StepId = 4, StepDescription = "Add the curry powder, cumin, and turmeric to the pot. Stir well to coat the onions, garlic, and ginger with the spices." },
                new Step { StepId = 5, StepDescription = "Add the rinsed red lentils, coconut milk, and vegetable broth to the pot. Bring to a boil, then reduce the heat to low and let simmer for about 30-40 minutes, or until the lentils are tender." },
                new Step { StepId = 6, StepDescription = "Stir in the spinach and let it wilt in the curry." },
                new Step { StepId = 7, StepDescription = "Add lime juice and salt to taste. Adjust the seasoning if needed." },
                new Step { StepId = 8, StepDescription = "Garnish with fresh cilantro, if desired, and serve the lentil curry over steamed rice or with naan bread." },
            },
            Tags = new List<Tag>
            {
                new Tag { TagName = "Vegan" },
                new Tag { TagName = "Curry" },
                new Tag { TagName = "Lentils" },
                new Tag { TagName = "Coconut milk" },
                new Tag { TagName = "Plant-based" },
            }
        },
        new Recipe
        {
            User = Admin,
            Title = "Teriyaki Salmon",
            Description = "Delicious and tender salmon fillets glazed with a homemade teriyaki sauce.",
            Difficulty = "Intermediate",
            MealsPerRecipe = 2,
            EstimatedTime = 30,
            UserId = 1,
            Approved = true,
            Ingredients = new List<Ingredient>
            {
                new Ingredient { Name = "Salmon fillets", Quantity = "2", QuantityType = "fillet" },
                new Ingredient { Name = "Soy sauce", Quantity = "1/4", QuantityType = "cup" },
                new Ingredient { Name = "Mirin", Quantity = "2", QuantityType = "tea spoon" },
                new Ingredient { Name = "Brown sugar", Quantity = "2", QuantityType = "tea spoon" },
                new Ingredient { Name = "Minced ginger", Quantity = "1", QuantityType = "tea spoon" },
                new Ingredient { Name = "Minced garlic", Quantity = "1", QuantityType = "tea spoon" },
                new Ingredient { Name = "Cornstarch", Quantity = "1", QuantityType = "tea spoon" },
                new Ingredient { Name = "Water", Quantity = "1/4", QuantityType = "cup" },
                new Ingredient { Name = "Sesame seeds (optional)", Quantity = "1", QuantityType = "tea spoon" },
                new Ingredient { Name = "Green onions (optional)", Quantity = "2", QuantityType = "un" },
            },
            Steps = new List<Step>
            {
                new Step { StepId = 1, StepDescription = "In a small saucepan, combine the soy sauce, mirin, brown sugar, minced ginger, and minced garlic. Bring the mixture to a simmer over medium heat." },
                new Step { StepId = 2, StepDescription = "In a separate bowl, whisk together the cornstarch and water to make a slurry. Add the slurry to the saucepan and cook, stirring constantly, until the sauce thickens." },
                new Step { StepId = 3, StepDescription = "Preheat the broiler in your oven." },
                new Step { StepId = 4, StepDescription = "Place the salmon fillets on a baking sheet lined with foil. Brush the teriyaki sauce over the salmon, reserving some for later." },
                new Step { StepId = 5, StepDescription = "Broil the salmon for about 6-8 minutes, or until it flakes easily with a fork." },
                new Step { StepId = 6, StepDescription = "Remove the salmon from the oven and brush it with the remaining teriyaki sauce." },
                new Step { StepId = 7, StepDescription = "Garnish with sesame seeds and sliced green onions, if desired." },
                new Step { StepId = 8, StepDescription = "Serve the teriyaki salmon with steamed rice and vegetables." },
            },
            Tags = new List<Tag>
            {
                new Tag { TagName = "Seafood" },
                new Tag { TagName = "Japanese" },
                new Tag { TagName = "Teriyaki" },
                new Tag { TagName = "Salmon" },
            }
        },
        new Recipe
        {
            User = Admin,
            Title = "Chocolate Chip Cookies",
            Description = "Classic homemade chocolate chip cookies with a soft and chewy texture.",
            Difficulty = "Easy",
            MealsPerRecipe = 24,
            EstimatedTime = 40,
            UserId = 1,
            Approved = true,
            Ingredients = new List<Ingredient>
        {
            new Ingredient { Name = "All-purpose flour", Quantity = "2", QuantityType = "cup" },
            new Ingredient { Name = "Baking soda", Quantity = "1", QuantityType = "tea spoon" },
            new Ingredient { Name = "Salt", Quantity = "1/2", QuantityType = "tea spoon" },
            new Ingredient { Name = "Unsalted butter, softened", Quantity = "1", QuantityType = "cup" },
            new Ingredient { Name = "Granulated sugar", Quantity = "3/4", QuantityType = "cup" },
            new Ingredient { Name = "Brown sugar, packed", Quantity = "3/4", QuantityType = "cup" },
            new Ingredient { Name = "Eggs", Quantity = "2", QuantityType = "un" },
            new Ingredient { Name = "Vanilla extract", Quantity = "1", QuantityType = "tea spoon" },
            new Ingredient { Name = "Chocolate chips", Quantity = "2", QuantityType = "cup" },
        },
            Steps = new List<Step>
        {
            new Step { StepId = 1, StepDescription = "Preheat the oven to 375°F (190°C). Line a baking sheet with parchment paper." },
            new Step { StepId = 2, StepDescription = "In a medium bowl, whisk together the flour, baking soda, and salt. Set aside." },
            new Step { StepId = 3, StepDescription = "In a large mixing bowl, cream together the softened butter, granulated sugar, and brown sugar until light and fluffy." },
            new Step { StepId = 4, StepDescription = "Beat in the eggs one at a time, then stir in the vanilla extract." },
            new Step { StepId = 5, StepDescription = "Gradually add the dry ingredients to the wet ingredients and mix until just combined." },
            new Step { StepId = 6, StepDescription = "Fold in the chocolate chips." },
            new Step { StepId = 7, StepDescription = "Drop rounded tablespoons of dough onto the prepared baking sheet, spacing them about 2 inches apart." },
            new Step { StepId = 8, StepDescription = "Bake for 9-11 minutes, or until the edges are golden brown. Allow the cookies to cool on the baking sheet for a few minutes before transferring them to a wire rack to cool completely." },
        },
            Tags = new List<Tag>
        {
            new Tag { TagName = "Dessert" },
            new Tag { TagName = "Cookies" },
            new Tag { TagName = "Chocolate" },
            new Tag { TagName = "Baking" },
        }
        },
        new Recipe
        {
            User = Admin,
            Title = "Chicken Fajitas",
            Description = "Flavorful and sizzling chicken fajitas with tender strips of seasoned chicken, bell peppers, and onions.",
            Difficulty = "Easy",
            MealsPerRecipe = 4,
            EstimatedTime = 25,
            UserId = 1,
            Approved = true,
            Ingredients = new List<Ingredient>
            {
                new Ingredient { Name = "Chicken breasts", Quantity = "2", QuantityType = "un" },
                new Ingredient { Name = "Bell peppers", Quantity = "2", QuantityType = "un" },
                new Ingredient { Name = "Onion", Quantity = "1", QuantityType = "un" },
                new Ingredient { Name = "Lime", Quantity = "1", QuantityType = "un" },
                new Ingredient { Name = "Olive oil", Quantity = "2", QuantityType = "tea spoon" },
                new Ingredient { Name = "Chili powder", Quantity = "1", QuantityType = "tea spoon" },
                new Ingredient { Name = "Cumin", Quantity = "1", QuantityType = "tea spoon" },
                new Ingredient { Name = "Paprika", Quantity = "1/2", QuantityType = "tea spoon" },
                new Ingredient { Name = "Garlic powder", Quantity = "1/2", QuantityType = "tea spoon" },
                new Ingredient { Name = "Salt", Quantity = "1/2", QuantityType = "tea spoon" },
                new Ingredient { Name = "Black pepper", Quantity = "1/4", QuantityType = "tea spoon" },
                new Ingredient { Name = "Flour tortillas", Quantity = "8", QuantityType = "un" },
                new Ingredient { Name = "Sour cream (optional)", Quantity = "1/2", QuantityType = "cup" },
                new Ingredient { Name = "Salsa (optional)", Quantity = "1/2", QuantityType = "cup" },
            },
            Steps = new List<Step>
            {
                new Step { StepId = 1, StepDescription = "Slice the chicken breasts, bell peppers, and onion into thin strips." },
                new Step { StepId = 2, StepDescription = "In a bowl, combine the chili powder, cumin, paprika, garlic powder, salt, and black pepper." },
                new Step { StepId = 3, StepDescription = "Add the sliced chicken to the bowl and toss until the chicken is coated with the spice mixture." },
                new Step { StepId = 4, StepDescription = "Heat olive oil in a large skillet over medium-high heat. Add the seasoned chicken and cook until browned and cooked through." },
                new Step { StepId = 5, StepDescription = "Push the chicken to one side of the skillet and add the bell peppers and onion. Sauté until the vegetables are tender-crisp." },
                new Step { StepId = 6, StepDescription = "Squeeze the juice of one lime over the chicken and vegetables. Toss everything together to combine." },
                new Step { StepId = 7, StepDescription = "Warm the flour tortillas in a dry skillet or in the microwave." },
                new Step { StepId = 8, StepDescription = "Serve the chicken fajitas with warm tortillas and optional toppings like sour cream and salsa." },
            },
            Tags = new List<Tag>
            {
                new Tag { TagName = "Chicken" },
                new Tag { TagName = "Mexican" },
                new Tag { TagName = "Fajitas" },
                new Tag { TagName = "Tex-Mex" },
            }
        },
        new Recipe
        {
            User = Admin,
            Title = "Beef Stroganoff",
            Description = "Classic Russian dish with tender beef strips in a creamy mushroom sauce, served over noodles or rice.",
            Difficulty = "Intermediate",
            MealsPerRecipe = 4,
            EstimatedTime = 45,
            UserId = 1,
            Approved = true,
            Ingredients = new List<Ingredient>
        {
            new Ingredient { Name = "Beef sirloin", Quantity = "1", QuantityType = "kg" },
            new Ingredient { Name = "Mushrooms", Quantity = "226", QuantityType = "gr" },
            new Ingredient { Name = "Onion", Quantity = "1", QuantityType = "un" },
            new Ingredient { Name = "Garlic", Quantity = "3", QuantityType = "un" },
            new Ingredient { Name = "Beef broth", Quantity = "1", QuantityType = "cup" },
            new Ingredient { Name = "Sour cream", Quantity = "1/2", QuantityType = "cup" },
            new Ingredient { Name = "Dijon mustard", Quantity = "1", QuantityType = "tablespoon" },
            new Ingredient { Name = "Worcestershire sauce", Quantity = "1", QuantityType = "tablespoon" },
            new Ingredient { Name = "Paprika", Quantity = "1", QuantityType = "teaspoon" },
            new Ingredient { Name = "Salt", Quantity = "1/2", QuantityType = "teaspoon" },
            new Ingredient { Name = "Black pepper", Quantity = "1/4", QuantityType = "teaspoon" },
            new Ingredient { Name = "Egg noodles or rice", Quantity = "1,6", QuantityType = "kg" },
            new Ingredient { Name = "Fresh parsley (optional)", Quantity = "2", QuantityType = "tablespoons" },
        },
            Steps = new List<Step>
        {
            new Step { StepId = 1, StepDescription = "Slice the beef sirloin into thin strips." },
            new Step { StepId = 2, StepDescription = "Heat some oil in a large skillet over medium-high heat. Add the beef strips and cook until browned. Remove the beef from the skillet and set aside." },
            new Step { StepId = 3, StepDescription = "In the same skillet, add the sliced mushrooms and diced onion. Sauté until the mushrooms and onion are tender." },
            new Step { StepId = 4, StepDescription = "Add the minced garlic to the skillet and cook for another minute." },
            new Step { StepId = 5, StepDescription = "Pour the beef broth into the skillet and bring to a simmer. Cook for a few minutes to reduce the liquid." },
            new Step { StepId = 6, StepDescription = "Reduce the heat to low and stir in the sour cream, Dijon mustard, Worcestershire sauce, paprika, salt, and black pepper. Cook until the sauce is heated through." },
            new Step { StepId = 7, StepDescription = "Meanwhile, cook the egg noodles or rice according to package instructions. Drain well." },
            new Step { StepId = 8, StepDescription = "Return the cooked beef to the skillet and stir to coat it with the creamy mushroom sauce." },
            new Step { StepId = 9, StepDescription = "Serve the beef stroganoff over the cooked egg noodles or rice. Garnish with fresh parsley, if desired." },
        },
            Tags = new List<Tag>
        {
            new Tag { TagName = "Beef" },
            new Tag { TagName = "Russian" },
            new Tag { TagName = "Mushrooms" },
            new Tag { TagName = "Creamy" },
        }
        },
    new Recipe
    {
        User = Admin,
        Title = "BBQ Ribs",
        Description = "Delicious and tender BBQ ribs with a smoky and tangy barbecue sauce.",
        Difficulty = "Intermediate",
        MealsPerRecipe = 4,
        EstimatedTime = 3 * 60 + 30, // 3 hours and 30 minutes
        UserId = 1,
        Approved = true,
        Ingredients = new List<Ingredient>
        {
            new Ingredient { Name = "Pork baby back ribs", Quantity = "2", QuantityType = "racks" },
            new Ingredient { Name = "BBQ rub or seasoning", Quantity = "1/4", QuantityType = "cup" },
            new Ingredient { Name = "BBQ sauce", Quantity = "2", QuantityType = "cup" },
            new Ingredient { Name = "Apple cider vinegar", Quantity = "1/4", QuantityType = "cup" },
            new Ingredient { Name = "Brown sugar", Quantity = "2", QuantityType = "tablespoons" },
            new Ingredient { Name = "Mustard", Quantity = "1", QuantityType = "tablespoon" },
            new Ingredient { Name = "Worcestershire sauce", Quantity = "1", QuantityType = "tablespoon" },
            new Ingredient { Name = "Garlic powder", Quantity = "1", QuantityType = "teaspoon" },
            new Ingredient { Name = "Onion powder", Quantity = "1", QuantityType = "teaspoon" },
            new Ingredient { Name = "Salt", Quantity = "1/2", QuantityType = "teaspoon" },
            new Ingredient { Name = "Black pepper", Quantity = "1/2", QuantityType = "teaspoon" },
        },
        Steps = new List<Step>
        {
            new Step { StepId = 1, StepDescription = "Preheat the oven to 275°F (135°C)." },
            new Step { StepId = 2, StepDescription = "Remove the membrane from the back of the ribs by loosening it with a knife and pulling it off." },
            new Step { StepId = 3, StepDescription = "Rub the BBQ rub or seasoning all over the ribs, covering both sides." },
            new Step { StepId = 4, StepDescription = "Wrap each rack of ribs tightly in aluminum foil, sealing the edges." },
            new Step { StepId = 5, StepDescription = "Place the foil-wrapped ribs on a baking sheet and bake in the preheated oven for 3 hours." },
            new Step { StepId = 6, StepDescription = "While the ribs are baking, prepare the barbecue sauce. In a saucepan, combine the BBQ sauce, apple cider vinegar, brown sugar, mustard, Worcestershire sauce, garlic powder, onion powder, salt, and black pepper. Heat over medium heat until the sauce is heated through." },
            new Step { StepId = 7, StepDescription = "After 3 hours, remove the ribs from the oven and carefully open the foil packets. Brush the ribs generously with the barbecue sauce, reserving some for later." },
            new Step { StepId = 8, StepDescription = "Preheat the grill to medium-high heat." },
            new Step { StepId = 9, StepDescription = "Place the ribs on the grill and cook for 10-15 minutes, basting with more barbecue sauce and flipping occasionally, until the ribs are nicely charred and glazed." },
            new Step { StepId = 10, StepDescription = "Remove the ribs from the grill and let them rest for a few minutes. Serve with the remaining barbecue sauce on the side." },
        },
        Tags = new List<Tag>
        {
            new Tag { TagName = "Pork" },
            new Tag { TagName = "BBQ" },
            new Tag { TagName = "Ribs" },
            new Tag { TagName = "Smoky" },
        }
    }
//    new Recipe
//    {
//        User = Admin,
//        Title = "Tenderloin Steak",
//        Description = "Juicy and flavorful tenderloin steak cooked to perfection.",
//        Difficulty = "Intermediate",
//        MealsPerRecipe = 2,
//        EstimatedTime = 30,
//        UserId = 1,
//        Approved = true,
//        Ingredients = new List<Ingredient>
//        {
//            new Ingredient { Name = "Tenderloin steak", Quantity = "2", QuantityType = "steaks" },
//            new Ingredient { Name = "Olive oil", Quantity = "2", QuantityType = "tablespoons" },
//            new Ingredient { Name = "Butter", Quantity = "2", QuantityType = "tablespoons" },
//            new Ingredient { Name = "Garlic", Quantity = "2", QuantityType = "cloves" },
//            new Ingredient { Name = "Fresh thyme", Quantity = "2", QuantityType = "sprigs" },
//            new Ingredient { Name = "Salt", Quantity = "1", QuantityType = "teaspoon" },
//            new Ingredient { Name = "Black pepper", Quantity = "1/2", QuantityType = "teaspoon" },
//        },
//        Steps = new List<Step>
//        {
//            new Step { StepId = 1, StepDescription = "Take the tenderloin steaks out of the refrigerator and let them come to room temperature for about 30 minutes." },
//            new Step { StepId = 2, StepDescription = "Preheat a skillet or grill pan over high heat." },
//            new Step { StepId = 3, StepDescription = "Rub the steaks with olive oil and season with salt and black pepper." },
//            new Step { StepId = 4, StepDescription = "Place the steaks in the hot skillet or grill pan. Cook for 3-4 minutes per side for medium-rare, or adjust the cooking time to your desired level of doneness." },
//            new Step { StepId = 5, StepDescription = "While the steaks are cooking, melt the butter in a small saucepan. Add the minced garlic and fresh thyme leaves. Cook for a minute or until the garlic becomes fragrant." },
//            new Step { StepId = 6, StepDescription = "Once the steaks are done, remove them from the skillet and let them rest for a few minutes." },
//            new Step { StepId = 7, StepDescription = "Brush the steaks with the garlic butter mixture, coating them evenly." },
//            new Step { StepId = 8, StepDescription = "Slice the steaks against the grain and serve them hot." },
//        },
//        Tags = new List<Tag>
//        {
//            new Tag { TagName = "Beef" },
//            new Tag { TagName = "Steak" },
//            new Tag { TagName = "Grilled" },
//            new Tag { TagName = "Juicy" },
//        }
//    },
//    new Recipe
//    {
//        User = Admin,
//        Title = "Lamb Chops",
//        Description = "Tender and succulent lamb chops with a flavorful herb crust.",
//        Difficulty = "Intermediate",
//        MealsPerRecipe = 2,
//        EstimatedTime = 25,
//        UserId = 1,
//        Approved = true,
//        Ingredients = new List<Ingredient>
//        {
//            new Ingredient { Name = "Lamb chops", Quantity = "4", QuantityType = "chops" },
//            new Ingredient { Name = "Dijon mustard", Quantity = "2", QuantityType = "tablespoons" },
//            new Ingredient { Name = "Fresh rosemary", Quantity = "2", QuantityType = "sprigs" },
//            new Ingredient { Name = "Fresh thyme", Quantity = "2", QuantityType = "sprigs" },
//            new Ingredient { Name = "Garlic", Quantity = "2", QuantityType = "cloves" },
//            new Ingredient { Name = "Breadcrumbs", Quantity = "1/4", QuantityType = "cup" },
//            new Ingredient { Name = "Olive oil", Quantity = "2", QuantityType = "tablespoons" },
//            new Ingredient { Name = "Salt", Quantity = "1/2", QuantityType = "teaspoon" },
//            new Ingredient { Name = "Black pepper", Quantity = "1/4", QuantityType = "teaspoon" },
//        },
//        Steps = new List<Step>
//        {
//            new Step { StepId = 1, StepDescription = "Preheat the oven to 400°F (200°C)." },
//            new Step { StepId = 2, StepDescription = "Season the lamb chops with salt and black pepper." },
//            new Step { StepId = 3, StepDescription = "In a small bowl, mix together the Dijon mustard, minced garlic, chopped rosemary, chopped thyme, breadcrumbs, and olive oil to form a paste." },
//            new Step { StepId = 4, StepDescription = "Spread the herb paste over the top of each lamb chop, pressing it firmly to adhere." },
//            new Step { StepId = 5, StepDescription = "Heat a skillet over high heat. Add the lamb chops, herb-side down, and sear for 2-3 minutes, or until the crust is golden brown." },
//            new Step { StepId = 6, StepDescription = "Flip the chops and transfer the skillet to the preheated oven. Roast for 8-10 minutes for medium-rare, or adjust the cooking time to your desired level of doneness." },
//            new Step { StepId = 7, StepDescription = "Remove the lamb chops from the oven and let them rest for a few minutes before serving." },
//            new Step { StepId = 8, StepDescription = "Serve the lamb chops hot, garnished with additional fresh herbs if desired." },
//        },
//        Tags = new List<Tag>
//        {
//            new Tag { TagName = "Lamb" },
//            new Tag { TagName = "Chops" },
//            new Tag { TagName = "Herb Crust" },
//            new Tag { TagName = "Succulent" },
//        }
//    },
//    new Recipe
//    {
//        User = Admin,
//        Title = "Pork Tenderloin",
//        Description = "Tender and juicy pork tenderloin marinated in a flavorful blend of herbs and spices.",
//        Difficulty = "Intermediate",
//        MealsPerRecipe = 4,
//        EstimatedTime = 1 * 60 + 15, // 1 hour and 15 minutes
//        UserId = 1,
//        Approved = true,
//        Ingredients = new List<Ingredient>
//        {
//            new Ingredient { Name = "Pork tenderloin", Quantity = "2", QuantityType = "kg" },
//            new Ingredient { Name = "Olive oil", Quantity = "2", QuantityType = "tablespoons" },
//            new Ingredient { Name = "Dijon mustard", Quantity = "1", QuantityType = "tablespoon" },
//            new Ingredient { Name = "Garlic", Quantity = "4", QuantityType = "cloves" },
//            new Ingredient { Name = "Fresh rosemary", Quantity = "2", QuantityType = "sprigs" },
//            new Ingredient { Name = "Fresh thyme", Quantity = "2", QuantityType = "sprigs" },
//            new Ingredient { Name = "Paprika", Quantity = "1", QuantityType = "teaspoon" },
//            new Ingredient { Name = "Salt", Quantity = "1", QuantityType = "teaspoon" },
//            new Ingredient { Name = "Black pepper", Quantity = "1/2", QuantityType = "teaspoon" },
//        },
//        Steps = new List<Step>
//        {
//            new Step { StepId = 1, StepDescription = "Preheat the oven to 375°F (190°C)." },
//            new Step { StepId = 2, StepDescription = "In a small bowl, whisk together the olive oil, Dijon mustard, minced garlic, chopped rosemary, chopped thyme, paprika, salt, and black pepper to create a marinade." },
//            new Step { StepId = 3, StepDescription = "Place the pork tenderloin in a baking dish and pour the marinade over the meat, making sure to coat it evenly." },
//            new Step { StepId = 4, StepDescription = "Let the pork tenderloin marinate for at least 30 minutes to allow the flavors to penetrate the meat." },
//            new Step { StepId = 5, StepDescription = "Heat a skillet over medium-high heat. Add a drizzle of olive oil and sear the pork tenderloin on all sides until browned." },
//            new Step { StepId = 6, StepDescription = "Transfer the seared pork tenderloin to the preheated oven and roast for about 25-30 minutes, or until the internal temperature reaches 145°F (63°C)." },
//            new Step { StepId = 7, StepDescription = "Remove the pork tenderloin from the oven and let it rest for 5 minutes before slicing." },
//            new Step { StepId = 8, StepDescription = "Slice the pork tenderloin into medallions and serve them hot." },
//        },
//        Tags = new List<Tag>
//        {
//            new Tag { TagName = "Pork" },
//            new Tag { TagName = "Tenderloin" },
//            new Tag { TagName = "Marinated" },
//            new Tag { TagName = "Juicy" },
//        }
//    },
//    new Recipe
//    {
//        User = Admin,
//        Title = "Grilled Salmon",
//        Description = "Juicy grilled salmon fillets with a delicious lemon and dill marinade.",
//        Difficulty = "Intermediate",
//        MealsPerRecipe = 4,
//        EstimatedTime = 20,
//        UserId = 1,
//        Approved = true,
//        Ingredients = new List<Ingredient>
//        {
//            new Ingredient { Name = "Salmon fillets", Quantity = "4", QuantityType = "fillets" },
//            new Ingredient { Name = "Lemon", Quantity = "1", QuantityType = "fruit" },
//            new Ingredient { Name = "Fresh dill", Quantity = "2", QuantityType = "tablespoons" },
//            new Ingredient { Name = "Olive oil", Quantity = "2", QuantityType = "tablespoons" },
//            new Ingredient { Name = "Salt", Quantity = "1/2", QuantityType = "teaspoon" },
//            new Ingredient { Name = "Black pepper", Quantity = "1/4", QuantityType = "teaspoon" },
//        },
//        Steps = new List<Step>
//        {
//            new Step { StepId = 1, StepDescription = "Preheat the grill to medium-high heat." },
//            new Step { StepId = 2, StepDescription = "In a small bowl, whisk together the juice of the lemon, chopped dill, olive oil, salt, and black pepper to create a marinade." },
//            new Step { StepId = 3, StepDescription = "Place the salmon fillets in a shallow dish and pour the marinade over them. Let them marinate for about 15 minutes." },
//            new Step { StepId = 4, StepDescription = "Grill the salmon fillets for about 4-5 minutes per side, or until cooked to your desired level of doneness." },
//            new Step { StepId = 5, StepDescription = "Remove the salmon from the grill and let it rest for a few minutes before serving." },
//            new Step { StepId = 6, StepDescription = "Serve the grilled salmon hot, garnished with additional fresh dill if desired." },
//        },
//        Tags = new List<Tag>
//        {
//            new Tag { TagName = "Salmon" },
//            new Tag { TagName = "Grilled" },
//            new Tag { TagName = "Seafood" },
//            new Tag { TagName = "Healthy" },
//        }
//    },
//    new Recipe
//    {
//        User = Admin,
//        Title = "Baked Cod",
//        Description = "Tender and flaky baked cod fillets with a flavorful herb crust.",
//        Difficulty = "Easy",
//        MealsPerRecipe = 4,
//        EstimatedTime = 25,
//        UserId = 1,
//        Approved = true,
//        Ingredients = new List<Ingredient>
//        {
//            new Ingredient { Name = "Cod fillets", Quantity = "4", QuantityType = "fillets" },
//            new Ingredient { Name = "Lemon", Quantity = "1", QuantityType = "fruit" },
//            new Ingredient { Name = "Fresh parsley", Quantity = "2", QuantityType = "tablespoons" },
//            new Ingredient { Name = "Fresh dill", Quantity = "2", QuantityType = "tablespoons" },
//            new Ingredient { Name = "Garlic", Quantity = "2", QuantityType = "cloves" },
//            new Ingredient { Name = "Breadcrumbs", Quantity = "1/4", QuantityType = "cup" },
//            new Ingredient { Name = "Olive oil", Quantity = "2", QuantityType = "tablespoons" },
//            new Ingredient { Name = "Salt", Quantity = "1/2", QuantityType = "teaspoon" },
//            new Ingredient { Name = "Black pepper", Quantity = "1/4", QuantityType = "teaspoon" },
//        },
//        Steps = new List<Step>
//        {
//            new Step { StepId = 1, StepDescription = "Preheat the oven to 400°F (200°C)." },
//            new Step { StepId = 2, StepDescription = "Place the cod fillets in a baking dish and squeeze the juice of the lemon over them." },
//            new Step { StepId = 3, StepDescription = "In a small bowl, combine the chopped parsley, chopped dill, minced garlic, breadcrumbs, olive oil, salt, and black pepper to create an herb crust mixture." },
//            new Step { StepId = 4, StepDescription = "Spread the herb crust mixture evenly over the top of each cod fillet." },
//            new Step { StepId = 5, StepDescription = "Bake the cod fillets in the preheated oven for about 15-18 minutes, or until the fish is opaque and flakes easily with a fork." },
//            new Step { StepId = 6, StepDescription = "Remove the baked cod from the oven and let it rest for a few minutes before serving." },
//            new Step { StepId = 7, StepDescription = "Serve the cod fillets hot, garnished with fresh herbs and lemon wedges if desired." },
//        },
//        Tags = new List<Tag>
//        {
//            new Tag { TagName = "Cod" },
//            new Tag { TagName = "Baked" },
//            new Tag { TagName = "Fish" },
//            new Tag { TagName = "Healthy" },
//        }
//    },
//    new Recipe
//    {
//        User = Admin,
//        Title = "Lemon Garlic Shrimp",
//        Description = "Tender and succulent shrimp cooked in a tangy lemon and garlic sauce.",
//        Difficulty = "Easy",
//        MealsPerRecipe = 4,
//        EstimatedTime = 15,
//        UserId = 1,
//        Approved = true,
//        Ingredients = new List<Ingredient>
//        {
//            new Ingredient { Name = "Shrimp", Quantity = "1", QuantityType = "kg" },
//            new Ingredient { Name = "Lemon", Quantity = "1", QuantityType = "fruit" },
//            new Ingredient { Name = "Garlic", Quantity = "4", QuantityType = "cloves" },
//            new Ingredient { Name = "Butter", Quantity = "2", QuantityType = "tablespoons" },
//            new Ingredient { Name = "Olive oil", Quantity = "2", QuantityType = "tablespoons" },
//            new Ingredient { Name = "Fresh parsley", Quantity = "2", QuantityType = "tablespoons" },
//            new Ingredient { Name = "Salt", Quantity = "1/2", QuantityType = "teaspoon" },
//            new Ingredient { Name = "Black pepper", Quantity = "1/4", QuantityType = "teaspoon" },
//        },
//        Steps = new List<Step>
//        {
//            new Step { StepId = 1, StepDescription = "Peel and devein the shrimp, leaving the tails intact." },
//            new Step { StepId = 2, StepDescription = "In a skillet, melt the butter and olive oil over medium heat. Add the minced garlic and cook for about 1 minute until fragrant." },
//            new Step { StepId = 3, StepDescription = "Add the shrimp to the skillet and squeeze the juice of the lemon over them. Season with salt and black pepper." },
//            new Step { StepId = 4, StepDescription = "Cook the shrimp for about 2-3 minutes on each side, or until they turn pink and opaque." },
//            new Step { StepId = 5, StepDescription = "Sprinkle the chopped parsley over the cooked shrimp and toss to coat." },
//            new Step { StepId = 6, StepDescription = "Remove the skillet from heat and serve the lemon garlic shrimp hot." },
//        },
//        Tags = new List<Tag>
//        {
//            new Tag { TagName = "Shrimp" },
//            new Tag { TagName = "Seafood" },
//            new Tag { TagName = "Quick" },
//            new Tag { TagName = "Garlic" },
//        }
//    },
//    new Recipe
//    {
//        User = Admin,
//        Title = "Tuna Steak",
//        Description = "Pan-seared tuna steaks with a flavorful sesame crust.",
//        Difficulty = "Intermediate",
//        MealsPerRecipe = 2,
//        EstimatedTime = 15,
//        UserId = 1,
//        Approved = true,
//        Ingredients = new List<Ingredient>
//        {
//            new Ingredient { Name = "Tuna steaks", Quantity = "2", QuantityType = "steaks" },
//            new Ingredient { Name = "Sesame seeds", Quantity = "2", QuantityType = "tablespoons" },
//            new Ingredient { Name = "Soy sauce", Quantity = "2", QuantityType = "tablespoons" },
//            new Ingredient { Name = "Lime juice", Quantity = "1", QuantityType = "tablespoon" },
//            new Ingredient { Name = "Honey", Quantity = "1", QuantityType = "teaspoon" },
//            new Ingredient { Name = "Olive oil", Quantity = "2", QuantityType = "tablespoons" },
//            new Ingredient { Name = "Salt", Quantity = "1/2", QuantityType = "teaspoon" },
//            new Ingredient { Name = "Black pepper", Quantity = "1/4", QuantityType = "teaspoon" },
//        },
//        Steps = new List<Step>
//        {
//            new Step { StepId = 1, StepDescription = "In a shallow dish, combine the sesame seeds, soy sauce, lime juice, honey, olive oil, salt, and black pepper to create a marinade." },
//            new Step { StepId = 2, StepDescription = "Place the tuna steaks in the marinade, turning to coat them evenly. Let them marinate for about 10 minutes." },
//            new Step { StepId = 3, StepDescription = "Heat a skillet over high heat. Remove the tuna steaks from the marinade, allowing any excess marinade to drip off." },
//            new Step { StepId = 4, StepDescription = "Press the sesame seeds onto both sides of each tuna steak to create a crust." },
//            new Step { StepId = 5, StepDescription = "Add the tuna steaks to the hot skillet and cook for about 1-2 minutes per side, or until seared on the outside but still pink in the center." },
//            new Step { StepId = 6, StepDescription = "Remove the tuna steaks from the skillet and let them rest for a few minutes before slicing." },
//            new Step { StepId = 7, StepDescription = "Slice the tuna steaks and serve them hot, drizzled with any remaining marinade." },
//        },
//        Tags = new List<Tag>
//        {
//            new Tag { TagName = "Tuna" },
//            new Tag { TagName = "Seafood" },
//            new Tag { TagName = "Sesame" },
//            new Tag { TagName = "Asian" },
//        }
//    },
//    new Recipe
//    {
//        User = Admin,
//        Title = "Fish Tacos",
//        Description = "Delicious and flavorful fish tacos with a tangy slaw and creamy avocado sauce.",
//        Difficulty = "Intermediate",
//        MealsPerRecipe = 4,
//        EstimatedTime = 30,
//        UserId = 1,
//        Approved = true,
//        Ingredients = new List<Ingredient>
//        {
//            new Ingredient { Name = "White fish fillets", Quantity = "4", QuantityType = "fillets" },
//            new Ingredient { Name = "Flour tortillas", Quantity = "8", QuantityType = "tortillas" },
//            new Ingredient { Name = "Cabbage", Quantity = "2", QuantityType = "cups" },
//            new Ingredient { Name = "Carrots", Quantity = "1/2", QuantityType = "cup" },
//            new Ingredient { Name = "Lime juice", Quantity = "2", QuantityType = "tablespoons" },
//            new Ingredient { Name = "Cilantro", Quantity = "1/4", QuantityType = "cup" },
//            new Ingredient { Name = "Sour cream", Quantity = "1/2", QuantityType = "cup" },
//            new Ingredient { Name = "Avocado", Quantity = "1", QuantityType = "fruit" },
//            new Ingredient { Name = "Garlic", Quantity = "2", QuantityType = "cloves" },
//            new Ingredient { Name = "Cumin", Quantity = "1", QuantityType = "teaspoon" },
//            new Ingredient { Name = "Paprika", Quantity = "1", QuantityType = "teaspoon" },
//            new Ingredient { Name = "Olive oil", Quantity = "2", QuantityType = "tablespoons" },
//            new Ingredient { Name = "Salt", Quantity = "1/2", QuantityType = "teaspoon" },
//            new Ingredient { Name = "Black pepper", Quantity = "1/4", QuantityType = "teaspoon" },
//        },
//        Steps = new List<Step>
//        {
//            new Step { StepId = 1, StepDescription = "In a small bowl, combine the cumin, paprika, olive oil, salt, and black pepper to create a spice mixture. Rub the spice mixture over the fish fillets." },
//            new Step { StepId = 2, StepDescription = "In a separate bowl, combine the shredded cabbage, grated carrots, lime juice, chopped cilantro, and salt to create a tangy slaw. Toss to coat the vegetables." },
//            new Step { StepId = 3, StepDescription = "In another bowl, mash the avocado with a fork. Add the minced garlic, sour cream, lime juice, salt, and black pepper. Stir until well combined to create the avocado sauce." },
//            new Step { StepId = 4, StepDescription = "Heat a skillet over medium-high heat. Add the fish fillets and cook for about 3-4 minutes per side, or until they are cooked through and flake easily." },
//            new Step { StepId = 5, StepDescription = "Warm the flour tortillas in a dry skillet or microwave." },
//            new Step { StepId = 6, StepDescription = "To assemble the tacos, spread a spoonful of the avocado sauce onto each tortilla. Top with a cooked fish fillet and a generous amount of the tangy slaw." },
//            new Step { StepId = 7, StepDescription = "Serve the fish tacos hot, garnished with additional cilantro if desired." },
//        },
//        Tags = new List<Tag>
//        {
//            new Tag { TagName = "Fish" },
//            new Tag { TagName = "Tacos" },
//            new Tag { TagName = "Mexican" },
//            new Tag { TagName = "Avocado" },
//        }
//    },
//    new Recipe
//    {
//        User = Admin,
//        Title = "Vegan Chickpea Salad",
//        Description = "A refreshing and protein-packed salad made with chickpeas, fresh vegetables, and a zesty dressing.",
//        Difficulty = "Easy",
//        MealsPerRecipe = 2,
//        EstimatedTime = 15,
//        UserId = 1,
//        Approved = true,
//        Ingredients = new List<Ingredient>
//        {
//            new Ingredient { Name = "Canned chickpeas", Quantity = "1", QuantityType = "can" },
//            new Ingredient { Name = "Cucumber", Quantity = "1/2", QuantityType = "medium" },
//            new Ingredient { Name = "Cherry tomatoes", Quantity = "1", QuantityType = "cup" },
//            new Ingredient { Name = "Red onion", Quantity = "1/4", QuantityType = "small" },
//            new Ingredient { Name = "Fresh parsley", Quantity = "1/4", QuantityType = "cup" },
//            new Ingredient { Name = "Lemon juice", Quantity = "2", QuantityType = "tablespoons" },
//            new Ingredient { Name = "Olive oil", Quantity = "1", QuantityType = "tablespoon" },
//            new Ingredient { Name = "Dijon mustard", Quantity = "1", QuantityType = "tea spoon" },
//            new Ingredient { Name = "Garlic powder", Quantity = "1/2", QuantityType = "tea spoon" },
//            new Ingredient { Name = "Salt", Quantity = "1/2", QuantityType = "tea spoon" },
//            new Ingredient { Name = "Black pepper", Quantity = "1/4", QuantityType = "tea spoon" }
//        },
//        Steps = new List<Step>
//        {
//            new Step { StepId = 1, StepDescription = "Rinse and drain the canned chickpeas. Set aside." },
//            new Step { StepId = 2, StepDescription = "Dice the cucumber, halve the cherry tomatoes, finely chop the red onion, and chop the fresh parsley." },
//            new Step { StepId = 3, StepDescription = "In a large bowl, combine the chickpeas, diced cucumber, halved cherry tomatoes, chopped red onion, and fresh parsley." },
//            new Step { StepId = 4, StepDescription = "In a small bowl, whisk together the lemon juice, olive oil, Dijon mustard, garlic powder, salt, and black pepper to create the dressing." },
//            new Step { StepId = 5, StepDescription = "Pour the dressing over the salad ingredients and toss to coat evenly." },
//            new Step { StepId = 6, StepDescription = "Refrigerate the vegan chickpea salad for at least 30 minutes to allow the flavors to meld together." },
//            new Step { StepId = 7, StepDescription = "Serve the salad chilled as a refreshing and nutritious vegan main course." }
//        },
//        Tags = new List<Tag>
//        {
//            new Tag { TagName = "Vegan" },
//            new Tag { TagName = "Salad" },
//            new Tag { TagName = "Chickpeas" },
//            new Tag { TagName = "Healthy" }
//        }
//    },
//    new Recipe
//    {
//        User = Admin,
//        Title = "Creamy Garlic Parmesan Pasta",
//        Description = "A rich and creamy pasta dish with a garlic and Parmesan cheese sauce.",
//        Difficulty = "Easy",
//        MealsPerRecipe = 2,
//        EstimatedTime = 25,
//        UserId = 1,
//        Approved = true,
//        Ingredients = new List<Ingredient>
//        {
//            new Ingredient { Name = "Pasta", Quantity = "8", QuantityType = "ounces" },
//            new Ingredient { Name = "Butter", Quantity = "2", QuantityType = "tablespoons" },
//            new Ingredient { Name = "Garlic cloves", Quantity = "4", QuantityType = "cloves" },
//            new Ingredient { Name = "Heavy cream", Quantity = "1", QuantityType = "cup" },
//            new Ingredient { Name = "Parmesan cheese", Quantity = "1/2", QuantityType = "cup" },
//            new Ingredient { Name = "Salt", Quantity = "1/2", QuantityType = "tea spoon" },
//            new Ingredient { Name = "Black pepper", Quantity = "1/4", QuantityType = "tea spoon" },
//            new Ingredient { Name = "Fresh parsley", Quantity = "2", QuantityType = "tablespoons" }
//        },
//        Steps = new List<Step>
//        {
//            new Step { StepId = 1, StepDescription = "Cook the pasta according to package instructions. Drain and set aside." },
//            new Step { StepId = 2, StepDescription = "In a large skillet, melt the butter over medium heat. Add the minced garlic and cook until fragrant." },
//            new Step { StepId = 3, StepDescription = "Pour in the heavy cream and bring to a simmer. Cook for a few minutes to thicken the sauce." },
//            new Step { StepId = 4, StepDescription = "Stir in the grated Parmesan cheese until melted and smooth. Season with salt and black pepper to taste." },
//            new Step { StepId = 5, StepDescription = "Add the cooked pasta to the skillet and toss to coat the noodles in the creamy sauce." },
//            new Step { StepId = 6, StepDescription = "Garnish with fresh parsley and serve the creamy garlic Parmesan pasta hot." }
//        },
//        Tags = new List<Tag>
//        {
//            new Tag { TagName = "Pasta" },
//            new Tag { TagName = "Creamy" },
//            new Tag { TagName = "Garlic" },
//            new Tag { TagName = "Parmesan" }
//        }
//    },
//    new Recipe
//    {
//        User = Admin,
//        Title = "Margherita Pizza",
//        Description = "A classic Italian pizza topped with fresh tomatoes, mozzarella cheese, and basil leaves.",
//        Difficulty = "Easy",
//        MealsPerRecipe = 2,
//        EstimatedTime = 30,
//        UserId = 1,
//        Approved = true,
//        Ingredients = new List<Ingredient>
//        {
//            new Ingredient { Name = "Pizza dough", Quantity = "1", QuantityType = "ball" },
//            new Ingredient { Name = "Tomatoes", Quantity = "2", QuantityType = "medium" },
//            new Ingredient { Name = "Fresh mozzarella cheese", Quantity = "1", QuantityType = "1kg" },
//            new Ingredient { Name = "Fresh basil leaves", Quantity = "8", QuantityType = "leaves" },
//            new Ingredient { Name = "Olive oil", Quantity = "2", QuantityType = "tablespoons" },
//            new Ingredient { Name = "Salt", Quantity = "1/2", QuantityType = "tea spoon" },
//            new Ingredient { Name = "Black pepper", Quantity = "1/4", QuantityType = "tea spoon" }
//        },
//        Steps = new List<Step>
//        {
//            new Step { StepId = 1, StepDescription = "Preheat the oven to the highest temperature (usually around 500°F or 260°C)." },
//            new Step { StepId = 2, StepDescription = "Roll out the pizza dough into a round shape on a floured surface. Transfer it to a baking sheet or pizza stone." },
//            new Step { StepId = 3, StepDescription = "Slice the tomatoes and fresh mozzarella cheese into thin rounds." },
//            new Step { StepId = 4, StepDescription = "Place the tomato and mozzarella slices evenly on top of the pizza dough." },
//            new Step { StepId = 5, StepDescription = "Tear the fresh basil leaves and scatter them over the pizza." },
//            new Step { StepId = 6, StepDescription = "Drizzle olive oil over the pizza and season with salt and black pepper." },
//            new Step { StepId = 7, StepDescription = "Bake the Margherita pizza in the preheated oven for about 12-15 minutes or until the crust is golden and the cheese is melted and bubbly." },
//            new Step { StepId = 8, StepDescription = "Remove from the oven, let it cool for a few minutes, then slice and serve hot." }
//        },
//        Tags = new List<Tag>
//        {
//            new Tag { TagName = "Pizza" },
//            new Tag { TagName = "Italian" },
//            new Tag { TagName = "Margherita" },
//            new Tag { TagName = "Tomatoes" },
//            new Tag { TagName = "Mozzarella" }
//        }
//    },
//    new Recipe
//    {
//        User = Admin,
//        Title = "Chicken Alfredo Pasta",
//        Description = "A creamy and flavorful pasta dish with tender chicken and a rich Alfredo sauce.",
//        Difficulty = "Intermediate",
//        MealsPerRecipe = 4,
//        EstimatedTime = 40,
//        UserId = 1,
//        Approved = true,
//        Ingredients = new List<Ingredient>
//        {
//            new Ingredient { Name = "Pasta", Quantity = "1,2", QuantityType = "kg" },
//            new Ingredient { Name = "Chicken breasts", Quantity = "2", QuantityType = "pieces" },
//            new Ingredient { Name = "Butter", Quantity = "4", QuantityType = "tablespoons" },
//            new Ingredient { Name = "Garlic cloves", Quantity = "3", QuantityType = "cloves" },
//            new Ingredient { Name = "Heavy cream", Quantity = "1", QuantityType = "cup" },
//            new Ingredient { Name = "Parmesan cheese", Quantity = "1", QuantityType = "cup" },
//            new Ingredient { Name = "Salt", Quantity = "1/2", QuantityType = "tea spoon" },
//            new Ingredient { Name = "Black pepper", Quantity = "1/4", QuantityType = "tea spoon" },
//            new Ingredient { Name = "Fresh parsley", Quantity = "2", QuantityType = "tablespoons" }
//        },
//        Steps = new List<Step>
//        {
//            new Step { StepId = 1, StepDescription = "Cook the pasta according to package instructions. Drain and set aside." },
//            new Step { StepId = 2, StepDescription = "In a large skillet, melt the butter over medium heat. Add the minced garlic and cook until fragrant." },
//            new Step { StepId = 3, StepDescription = "Slice the chicken breasts into thin strips. Season with salt and black pepper." },
//            new Step { StepId = 4, StepDescription = "Add the chicken strips to the skillet and cook until browned and cooked through." },
//            new Step { StepId = 5, StepDescription = "Pour in the heavy cream and bring to a simmer. Cook for a few minutes to thicken the sauce." },
//            new Step { StepId = 6, StepDescription = "Stir in the grated Parmesan cheese until melted and smooth. Season with salt and black pepper to taste." },
//            new Step { StepId = 7, StepDescription = "Add the cooked pasta to the skillet and toss to coat the noodles in the creamy Alfredo sauce." },
//            new Step { StepId = 8, StepDescription = "Garnish with fresh parsley and serve the chicken Alfredo pasta hot." }
//        },
//        Tags = new List<Tag>
//        {
//            new Tag { TagName = "Pasta" },
//            new Tag { TagName = "Chicken" },
//            new Tag { TagName = "Alfredo" },
//            new Tag { TagName = "Creamy" }
//        }
//    },
//    new Recipe
//    {
//        User = Admin,
//        Title = "Loaded Nachos",
//        Description = "Delicious nachos topped with melted cheese, salsa, guacamole, and sour cream.",
//        Difficulty = "Easy",
//        MealsPerRecipe = 4,
//        EstimatedTime = 15,
//        UserId = 1,
//        Approved = true,
//        Ingredients = new List<Ingredient>
//        {
//            new Ingredient { Name = "Tortilla chips", Quantity = "1", QuantityType = "bag" },
//            new Ingredient { Name = "Cheddar cheese", Quantity = "1", QuantityType = "cup" },
//            new Ingredient { Name = "Salsa", Quantity = "1/2", QuantityType = "cup" },
//            new Ingredient { Name = "Guacamole", Quantity = "1/2", QuantityType = "cup" },
//            new Ingredient { Name = "Sour cream", Quantity = "1/4", QuantityType = "cup" },
//            new Ingredient { Name = "Green onions", Quantity = "2", QuantityType = "stalks" },
//            new Ingredient { Name = "Black olives", Quantity = "1/4", QuantityType = "cup" },
//            new Ingredient { Name = "Jalapeno peppers", Quantity = "1", QuantityType = "pepper" }
//        },
//        Steps = new List<Step>
//        {
//            new Step { StepId = 1, StepDescription = "Preheat the oven to 350°F (175°C)." },
//            new Step { StepId = 2, StepDescription = "Spread the tortilla chips in a single layer on a baking sheet." },
//            new Step { StepId = 3, StepDescription = "Sprinkle the shredded cheddar cheese evenly over the tortilla chips." },
//            new Step { StepId = 4, StepDescription = "Bake in the preheated oven for about 5-7 minutes or until the cheese is melted." },
//            new Step { StepId = 5, StepDescription = "Remove from the oven and top with salsa, guacamole, sour cream, sliced green onions, sliced black olives, and sliced jalapeno peppers." },
//            new Step { StepId = 6, StepDescription = "Serve the loaded nachos immediately and enjoy!" }
//        },
//        Tags = new List<Tag>
//        {
//            new Tag { TagName = "Snack" },
//            new Tag { TagName = "Nachos" },
//            new Tag { TagName = "Cheese" },
//            new Tag { TagName = "Salsa" }
//        }
//    },
//    new Recipe
//    {
//        User = Admin,
//        Title = "Caprese Skewers",
//        Description = "Easy and elegant skewers with fresh mozzarella, cherry tomatoes, and basil leaves.",
//        Difficulty = "Easy",
//        MealsPerRecipe = 4,
//        EstimatedTime = 10,
//        UserId = 1,
//        Approved = true,
//        Ingredients = new List<Ingredient>
//        {
//            new Ingredient { Name = "Fresh mozzarella balls", Quantity = "8", QuantityType = "balls" },
//            new Ingredient { Name = "Cherry tomatoes", Quantity = "16", QuantityType = "tomatoes" },
//            new Ingredient { Name = "Fresh basil leaves", Quantity = "16", QuantityType = "leaves" },
//            new Ingredient { Name = "Balsamic glaze", Quantity = "2 tbsp", QuantityType = "tablespoons" },
//            new Ingredient { Name = "Skewers", Quantity = "8", QuantityType = "skewers" }
//        },
//        Steps = new List<Step>
//        {
//            new Step { StepId = 1, StepDescription = "Thread one cherry tomato, one fresh mozzarella ball, and one basil leaf onto each skewer." },
//            new Step { StepId = 2, StepDescription = "Arrange the skewers on a serving platter." },
//            new Step { StepId = 3, StepDescription = "Drizzle the balsamic glaze over the skewers." },
//            new Step { StepId = 4, StepDescription = "Serve the caprese skewers as a delicious and refreshing snack." }
//        },
//        Tags = new List<Tag>
//        {
//            new Tag { TagName = "Snack" },
//            new Tag { TagName = "Caprese" },
//            new Tag { TagName = "Mozzarella" },
//            new Tag { TagName = "Tomatoes" }
//        }
//    },
//    new Recipe
//    {
//        User = Admin,
//        Title = "Stuffed Mushrooms",
//        Description = "Savory mushrooms stuffed with a delicious mixture of breadcrumbs, cheese, and herbs.",
//        Difficulty = "Intermediate",
//        MealsPerRecipe = 4,
//        EstimatedTime = 30,
//        UserId = 1,
//        Approved = true,
//        Ingredients = new List<Ingredient>
//        {
//            new Ingredient { Name = "Mushrooms", Quantity = "16", QuantityType = "mushrooms" },
//            new Ingredient { Name = "Breadcrumbs", Quantity = "1/2", QuantityType = "cup" },
//            new Ingredient { Name = "Parmesan cheese", Quantity = "1/4", QuantityType = "cup" },
//            new Ingredient { Name = "Garlic cloves", Quantity = "2", QuantityType = "cloves" },
//            new Ingredient { Name = "Fresh parsley", Quantity = "2", QuantityType = "tablespoons" },
//            new Ingredient { Name = "Olive oil", Quantity = "2", QuantityType = "tablespoons" }
//        },
//        Steps = new List<Step>
//        {
//            new Step { StepId = 1, StepDescription = "Preheat the oven to 375°F (190°C)." },
//            new Step { StepId = 2, StepDescription = "Remove the stems from the mushrooms and set aside." },
//            new Step { StepId = 3, StepDescription = "In a bowl, combine the breadcrumbs, grated Parmesan cheese, minced garlic, chopped fresh parsley, and olive oil." },
//            new Step { StepId = 4, StepDescription = "Fill each mushroom cap with the breadcrumb mixture." },
//            new Step { StepId = 5, StepDescription = "Place the stuffed mushrooms on a baking sheet and bake in the preheated oven for about 20-25 minutes or until the mushrooms are tender and the tops are golden." },
//            new Step { StepId = 6, StepDescription = "Serve the stuffed mushrooms as a tasty and satisfying snack." }
//        },
//        Tags = new List<Tag>
//        {
//            new Tag { TagName = "Snack" },
//            new Tag { TagName = "Mushrooms" },
//            new Tag { TagName = "Appetizer" },
//            new Tag { TagName = "Cheese" }
//        }
//    },
//    new Recipe
//    {
//        User = Admin,
//        Title = "Chicken Teriyaki",
//        Description = "Juicy grilled chicken glazed with a sweet and savory teriyaki sauce.",
//        Difficulty = "Intermediate",
//        MealsPerRecipe = 4,
//        EstimatedTime = 30,
//        UserId = 1,
//        Approved = true,
//        Ingredients = new List<Ingredient>
//        {
//            new Ingredient { Name = "Chicken thighs", Quantity = "4", QuantityType = "pieces" },
//            new Ingredient { Name = "Soy sauce", Quantity = "1/4", QuantityType = "cup" },
//            new Ingredient { Name = "Brown sugar", Quantity = "2", QuantityType = "tablespoons" },
//            new Ingredient { Name = "Garlic", Quantity = "2" },
//            new Ingredient { Name = "Ginger", Quantity = "1", QuantityType = "tea spoon" },
//            new Ingredient { Name = "Sesame oil", Quantity = "1", QuantityType = "tea spoon" },
//            new Ingredient { Name = "Green onions", Quantity = "2", QuantityType = "stalks" },
//            new Ingredient { Name = "Sesame seeds", Quantity = "1", QuantityType = "tablespoon" }
//        },
//        Steps = new List<Step>
//        {
//            new Step { StepId = 1, StepDescription = "In a bowl, whisk together the soy sauce, brown sugar, minced garlic, grated ginger, and sesame oil to make the teriyaki sauce." },
//            new Step { StepId = 2, StepDescription = "Place the chicken thighs in a shallow dish and pour half of the teriyaki sauce over them. Let marinate for about 15 minutes." },
//            new Step { StepId = 3, StepDescription = "Preheat the grill to medium-high heat. Grill the chicken thighs for about 6-8 minutes per side or until cooked through." },
//            new Step { StepId = 4, StepDescription = "Brush the grilled chicken with the remaining teriyaki sauce and garnish with sliced green onions and sesame seeds." },
//            new Step { StepId = 5, StepDescription = "Serve the chicken teriyaki hot with steamed rice and vegetables." }
//        },
//        Tags = new List<Tag>
//        {
//            new Tag { TagName = "Asian" },
//            new Tag { TagName = "Chicken" },
//            new Tag { TagName = "Teriyaki" },
//            new Tag { TagName = "Grilled" }
//        }
//    },
//    new Recipe
//    {
//        User = Admin,
//        Title = "Pad Thai",
//        Description = "Classic Thai stir-fried rice noodles with shrimp, tofu, eggs, and peanuts.",
//        Difficulty = "Intermediate",
//        MealsPerRecipe = 4,
//        EstimatedTime = 40,
//        UserId = 1,
//        Approved = true,
//        Ingredients = new List<Ingredient>
//        {
//            new Ingredient { Name = "Rice noodles", Quantity = "1", QuantityType = "kg" },
//            new Ingredient { Name = "Shrimp", Quantity = "1", QuantityType = "kg" },
//            new Ingredient { Name = "Firm tofu", Quantity = "1", QuantityType = "kg" },
//            new Ingredient { Name = "Eggs", Quantity = "2" },
//            new Ingredient { Name = "Garlic", Quantity = "2" },
//            new Ingredient { Name = "Green onions", Quantity = "4", QuantityType = "stalks" },
//            new Ingredient { Name = "Bean sprouts", Quantity = "1", QuantityType = "cup" },
//            new Ingredient { Name = "Peanuts", Quantity = "1/4", QuantityType = "cup" },
//            new Ingredient { Name = "Lime wedges", Quantity = "4" }
//        },
//        Steps = new List<Step>
//        {
//            new Step { StepId = 1, StepDescription = "Soak the rice noodles in hot water for about 10 minutes or until softened. Drain and set aside." },
//            new Step { StepId = 2, StepDescription = "In a wok or large skillet, heat some oil over medium heat. Add minced garlic and sliced tofu. Stir-fry for a few minutes until tofu is lightly browned." },
//            new Step { StepId = 3, StepDescription = "Push the tofu to one side of the wok and crack the eggs into the empty space. Scramble the eggs until cooked." },
//            new Step { StepId = 4, StepDescription = "Add the shrimp to the wok and stir-fry until pink and cooked through." },
//            new Step { StepId = 5, StepDescription = "Add the soaked rice noodles to the wok along with the prepared sauce. Stir-fry everything together until well combined and heated through." },
//            new Step { StepId = 6, StepDescription = "Add bean sprouts and sliced green onions to the wok. Toss everything together for another minute or two." },
//            new Step { StepId = 7, StepDescription = "Serve the Pad Thai hot, garnished with chopped peanuts and lime wedges on the side." }
//        },
//        Tags = new List<Tag>
//        {
//            new Tag { TagName = "Asian" },
//            new Tag { TagName = "Thai" },
//            new Tag { TagName = "Noodles" },
//            new Tag { TagName = "Shrimp" }
//        }
//    },
//    new Recipe
//    {
//        User = Admin,
//        Title = "Beef Stir-Fry",
//        Description = "Tender beef slices stir-fried with colorful vegetables in a flavorful sauce.",
//        Difficulty = "Intermediate",
//        MealsPerRecipe = 4,
//        EstimatedTime = 25,
//        UserId = 1,
//        Approved = true,
//        Ingredients = new List<Ingredient>
//        {
//            new Ingredient { Name = "Beef sirloin", Quantity = "1,4", QuantityType = "kg" },
//            new Ingredient { Name = "Bell peppers", Quantity = "2", QuantityType = "pieces" },
//            new Ingredient { Name = "Broccoli florets", Quantity = "2", QuantityType = "cups" },
//            new Ingredient { Name = "Carrots", Quantity = "2", QuantityType = "pieces" },
//            new Ingredient { Name = "Garlic", Quantity = "3" },
//            new Ingredient { Name = "Ginger", Quantity = "1", QuantityType = "tablespoon" },
//            new Ingredient { Name = "Soy sauce", Quantity = "3", QuantityType = "tablespoons" },
//            new Ingredient { Name = "Oyster sauce", Quantity = "2", QuantityType = "tablespoons" },
//            new Ingredient { Name = "Cornstarch", Quantity = "1", QuantityType = "tablespoon" },
//            new Ingredient { Name = "Vegetable oil", Quantity = "2", QuantityType = "tablespoons" },
//            new Ingredient { Name = "Green onions", Quantity = "4", QuantityType = "stalks" }
//        },
//        Steps = new List<Step>
//        {
//            new Step { StepId = 1, StepDescription = "Slice the beef into thin strips. In a bowl, combine the sliced beef, minced garlic, grated ginger, soy sauce, oyster sauce, and cornstarch. Let marinate for about 10 minutes." },
//            new Step { StepId = 2, StepDescription = "Heat some oil in a wok or large skillet over high heat. Add the marinated beef and stir-fry for a few minutes until browned. Remove the beef from the wok and set aside." },
//            new Step { StepId = 3, StepDescription = "In the same wok, add more oil if needed and stir-fry the bell peppers, broccoli florets, and sliced carrots for a couple of minutes until crisp-tender." },
//            new Step { StepId = 4, StepDescription = "Return the beef to the wok and toss everything together. Cook for another minute or two until heated through." },
//            new Step { StepId = 5, StepDescription = "Garnish the beef stir-fry with sliced green onions and serve hot with steamed rice." }
//        },
//        Tags = new List<Tag>
//        {
//            new Tag { TagName = "Asian" },
//            new Tag { TagName = "Beef" },
//            new Tag { TagName = "Stir-Fry" },
//            new Tag { TagName = "Vegetables" }
//        }
//    }
);


            this.SaveChanges();
        }
    }
    }

