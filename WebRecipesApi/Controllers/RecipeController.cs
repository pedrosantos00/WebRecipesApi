using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebRecipesApi.Domain;
using WebRecipesApi.BusinessLogic;
namespace WebRecipesApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly RecipeService _recipeService;
        private readonly UserService _userService;
        private readonly UserFavoriteRecipeService _userFavoriteRecipeService;


        public RecipeController(RecipeService recipeService, UserService userService, UserFavoriteRecipeService userFavoriteRecipeService)
        {
            _recipeService = recipeService;
            _userService = userService;
            _userFavoriteRecipeService = userFavoriteRecipeService;
        }


        [HttpPost("create")]
        public async Task<IActionResult> Create(int userId,[FromBody]Recipe? recipe)
        {
            //NULO
            if (recipe == null) return BadRequest();

            User userExists = await _userService.GetById(userId);
            if (userExists == null) return NotFound(new { Message = "User Not Found!" });

            recipe.User = userExists;
            recipe.UserId = userId;
            recipe.FavoritedBy = new List<UserFavoriteRecipe>();

            await _recipeService.Create(recipe);

            return Ok();
        }



        // GET: api/<RecipeController>
        [HttpGet]
        public async Task<IEnumerable<Recipe>> Get(string? searchFilter = null)
        {
            return await _recipeService.Search(searchFilter);
        }

        [HttpGet("Aproove")]
        public async Task<IEnumerable<Recipe>> ToAprove()
        {
            return await _recipeService.ToAprove();
        }

        // GET api/<RecipeController>/5
        [HttpGet("{id}")]
        public async Task<Recipe> Get(int id)
        {
            return await _recipeService.GetById(id);
        }


        [HttpGet("user/{id}")]
        public async Task<IEnumerable<Recipe>> GetByUserId(int id)
        {
            return await _recipeService.GetByUserId(id);
        }


        // POST api/<RecipeController>
        [HttpPost]
        public void Post([FromBody] Recipe recipe)
        {
            _recipeService.Update(recipe);
        }

        [HttpPost("r/{id}/{rate}")]
        public async Task<IActionResult> Rate(int id, float rate)
        {
            if (id == null) return BadRequest();
            Recipe recipe = await _recipeService.GetById(id);
            if (recipe == null) return NotFound(new { Message = "Recipe Not Found!" });

            recipe.Rate = (recipe.Rate * recipe.TotalRates + rate) / (recipe.TotalRates + 1);
            recipe.TotalRates++;

            await _recipeService.Update(recipe);
            return Ok();
        }

        [HttpPost("c/{id}")]
        public async Task<IActionResult> AddComment(int id,[FromBody] Comment? comment)
        {
            if (id == null) return BadRequest();
            Recipe recipe = await _recipeService.GetById(id);
            if (recipe == null) return NotFound(new { Message = "Recipe Not Found!" });

            User userExists = await _userService.GetById(comment.UserId);
            if (userExists == null) return NotFound(new { Message = "User Not Found!" });

            comment.CreatedDate = DateTime.Now;
            comment.RecipeId = id;
            comment.Name = userExists.FullName;
            

            recipe.Comments.Add(comment);
            await _recipeService.Update(recipe);
            return Ok();
        }

        // PUT api/<RecipeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Recipe recipe)
        {
            _recipeService.Update(recipe);
        }

        // PUT api/<RecipeController>/5
        [HttpPut("fav/{id}/{recipeId}")]
        public async Task<IActionResult> AddFavorite(int id, int recipeId)
        {
           
            //NULO
            if (recipeId == null) return BadRequest();
            Recipe recipe = await _recipeService.GetById(recipeId);
            if (recipe == null) return NotFound(new { Message = "Recipe Not Found!" });

            recipe.FavoritedBy = new List<UserFavoriteRecipe>();

            User userExists = await _userService.GetById(id);
            if (userExists == null) return NotFound(new { Message = "User Not Found!" });

            UserFavoriteRecipe userFavoriteRecipe = await _userFavoriteRecipeService.Exists(recipeId, id);

            if (userFavoriteRecipe != null)
            {
                recipe.FavoritedBy.Remove(userFavoriteRecipe);
                await _recipeService.Update(recipe);
            }
            else
            {
                var userFavorite = new UserFavoriteRecipe
                {
                    RecipeId = recipeId,
                    UserId = id,
                };

                recipe.FavoritedBy.Add(userFavorite);
                await _recipeService.Update(recipe);
            }

            return Ok();
        }

        // DELETE api/<RecipeController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _recipeService.Delete(id);
            return Ok();
        }
    }
}
