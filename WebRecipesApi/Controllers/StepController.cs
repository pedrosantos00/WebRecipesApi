using Microsoft.AspNetCore.Mvc;
using WebRecipesApi.Domain;
using WebRecipesApi.BusinessLogic;
namespace WebRecipesApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StepController : ControllerBase
    {
        private readonly StepService _stepService;

        public StepController(StepService stepService)
        {
            _stepService = stepService;
        }

        // GET: api/<StepController>
        [HttpGet]
        public async Task<IEnumerable<Step>> Get(string? searchFilter = null)
        {
            return await _stepService.Search(searchFilter);
        }

        // GET api/<StepController>/5
        [HttpGet("{id}")]
        public async Task<Step> Get(int id)
        {
            return await _stepService.GetById(id);
        }

        // POST api/<StepController>
        [HttpPost]
        public void Post([FromBody] Step step)
        {
            _stepService.Update(step);
        }

        // PUT api/<StepController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Step step)
        {
            _stepService.Update(step);
        }

        // DELETE api/<StepController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _stepService.Delete(id);
            return Ok();
        }
    }
}
