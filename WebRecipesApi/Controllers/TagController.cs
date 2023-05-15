using Microsoft.AspNetCore.Mvc;
using WebRecipesApi.Domain;
using WebRecipesApi.BusinessLogic;

namespace WebRecipesApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly TagService _tagService;

        public TagController(TagService tagService)
        {
            _tagService = tagService;
        }

        // GET: api/<TagController>
        [HttpGet]
        public async Task<IEnumerable<Tag>> Get(string? searchFilter = null)
        {
            return await _tagService.Search(searchFilter);
        }

        // GET api/<TagController>/5
        [HttpGet("{id}")]
        public async Task<Tag> Get(int id)
        {
            return await _tagService.GetById(id);
        }

        // POST api/<TagController>
        [HttpPost]
        public void Post([FromBody] Tag tag)
        {
            _tagService.Update(tag);
        }

        // PUT api/<TagController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Tag tag)
        {
            _tagService.Update(tag);
        }

        // DELETE api/<TagController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _tagService.Delete(id);
            return Ok();
        }
    }
}