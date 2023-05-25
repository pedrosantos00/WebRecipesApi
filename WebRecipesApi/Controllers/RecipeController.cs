using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebRecipesApi.Domain;
using WebRecipesApi.BusinessLogic;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using System.Collections;

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
        [Authorize]
        public async Task<IActionResult> Create(int userId, [FromBody] Recipe? recipe)
        {
            //NULO
            if (recipe == null) return BadRequest();

            User userExists = await _userService.GetById(userId);
            if (userExists == null) return NotFound(new { Message = "User Not Found!" });

            recipe.User = userExists;
            recipe.UserId = userId;
            recipe.FavoritedBy = new List<UserFavoriteRecipe>();
            recipe.RateAudit = new List<RateAudit>();

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
        [Authorize(Roles = "Admin")]
        public async Task<IEnumerable<Recipe>> ToApprove()
        {
            return await _recipeService.ToApprove();
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

        [HttpGet("fav/user/{id}")]
        public async Task<IEnumerable<Recipe>> GetByFavUserId(int id)
        {
            return await _recipeService.GetFavByUserId(id);
        }


        

        [HttpPost("r/{id}/{rate}/{userId}")]
        [Authorize]
        public async Task<IActionResult> Rate(int id, float rate, int userId)
        {
            if (id == null)
                return BadRequest();

            Recipe recipe = await _recipeService.GetById(id);

            if (recipe == null) return NotFound
                    (new { Message = "Recipe Not Found!" });

            if (recipe.RateAudit != null && recipe.RateAudit.Any(x => x.RatedBy == userId))
                return NotFound(new { Message = "You already voted on this recipe!" });

            var rateAudit = new RateAudit
            {
                RatedBy = userId,
            };

            if (recipe.RateAudit == null)
            {
                recipe.RateAudit = new List<RateAudit>();
            }

            recipe.RateAudit.Add(rateAudit);
            recipe.Rate = (recipe.Rate * recipe.TotalRates + rate) / (recipe.TotalRates + 1);
            recipe.TotalRates++;

            await _recipeService.UpdateRate(recipe);
            return Ok();
        }

        [HttpPost("c/{id}")]
        [Authorize]
        public async Task<IActionResult> AddComment(int id, [FromBody] Comment? comment)
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
            await _recipeService.UpdateComment(recipe);
            return Ok(new { Message = "Commentary added!" });
        }

        // PUT api/<RecipeController>/5
        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> UpdateRecipe([FromBody] Recipe? updatedRecipe)
        {
            if (updatedRecipe == null) return BadRequest();
            Recipe recipeExists = await _recipeService.GetById(updatedRecipe.Id);
            if (recipeExists == null) return NotFound(new { Message = "Recipe Not Found!" });

            await _recipeService.Update(recipeExists, updatedRecipe);
            return Ok(new { Message = "Recipe updated!!" });
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
            string respMessage = string.Empty;
            if (userFavoriteRecipe != null)
            {
                recipe.FavoritedBy.Remove(userFavoriteRecipe);
                await _recipeService.UpdateFav(recipe);
                respMessage = "Recipe added to fav list!";
            }
            else
            {
                var userFavorite = new UserFavoriteRecipe
                {
                    RecipeId = recipeId,
                    UserId = id,
                };

                recipe.FavoritedBy.Add(userFavorite);
                await _recipeService.UpdateFav(recipe);
                respMessage = "Recipe added to fav list!";
            }

            return Ok(new { Message = respMessage });
        }

        // DELETE api/<RecipeController>/5
        [HttpDelete("del/{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _recipeService.Delete(id))
                return Ok(new { Message = "Recipe Deleted!" });

            else
                return NotFound(new { Message = "Recipe Not Found!" });
        }
    }
}
