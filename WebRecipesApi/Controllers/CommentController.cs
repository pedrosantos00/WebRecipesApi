using Microsoft.AspNetCore.Mvc;
using WebRecipesApi.Domain;
using WebRecipesApi.BusinessLogic;

namespace WebCommentsApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly CommentService _commentService;

        public CommentController(CommentService commentService)
        {
            _commentService = commentService;
        }

        // GET: api/<CommentController>
        [HttpGet]
        public async Task<IEnumerable<Comment>> Get(string? searchFilter = null)
        {
            return await _commentService.Search(searchFilter);
        }

        // GET api/<CommentController>/5
        [HttpGet("{id}")]
        public async Task<Comment> Get(int id)
        {
            return await _commentService.GetById(id);
        }

        // POST api/<CommentController>
        [HttpPost]
        public void Post([FromBody] Comment comment)
        {
            _commentService.Update(comment);
        }

        // PUT api/<CommentController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Comment comment)
        {
            _commentService.Update(comment);
        }

        // DELETE api/<CommentController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _commentService.Delete(id);
            return Ok();
        }
    }
}
