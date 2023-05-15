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


        public RecipeController(RecipeService recipeService, UserService userService)
        {
            _recipeService = recipeService;
            _userService = userService;
        }


        [HttpPost("create")]
        public async Task<IActionResult> Create(int userId,[FromBody] Recipe recipe)
        {
            //NULO
            if (recipe == null) return BadRequest();

            User userExists = await _userService.GetById(userId);
            if (userExists == null) return NotFound(new { Message = "User Not Found!" });

            recipe.User = userExists;
            recipe.UserId = userId;

            await _recipeService.Create(recipe);

            return Ok();
        }

        // GET: api/<RecipeController>
        [HttpGet]
        public async Task<IEnumerable<Recipe>> Get(string? searchFilter = null)
        {
            return await _recipeService.Search(searchFilter);
        }

        // GET api/<RecipeController>/5
        [HttpGet("{id}")]
        public async Task<Recipe> Get(int id)
        {
            return await _recipeService.GetById(id);
        }

        // POST api/<RecipeController>
        [HttpPost]
        public void Post([FromBody] Recipe recipe)
        {
            _recipeService.Update(recipe);
        }

        // PUT api/<RecipeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Recipe recipe)
        {
            _recipeService.Update(recipe);
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
