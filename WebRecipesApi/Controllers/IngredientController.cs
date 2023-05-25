using Microsoft.AspNetCore.Mvc;
using WebRecipesApi.Domain;
using WebRecipesApi.BusinessLogic;

namespace WebRecipesApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly IngredientService _ingredientService;

        public IngredientController(IngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }

        // GET: api/<IngredientController>
        [HttpGet]
        public async Task<IEnumerable<Ingredient>> Get(string? searchFilter = null)
        {
            return await _ingredientService.Search(searchFilter);
        }

        // GET api/<IngredientController>/5
        [HttpGet("{id}")]
        public async Task<Ingredient> Get(int id)
        {
            return await _ingredientService.GetById(id);
        }

        // POST api/<IngredientController>
        [HttpPost]
        public async void Post([FromBody] Ingredient ingredient)
        {
            await _ingredientService.Update(ingredient);
        }

        // PUT api/<IngredientController>/5
        [HttpPut("{id}")]
        public async void Put(int id, [FromBody] Ingredient ingredient)
        {
           await  _ingredientService.Update(ingredient);
        }

        // DELETE api/<IngredientController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _ingredientService.Delete(id);
            return Ok();
        }
    }
}
