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
        private readonly RecipeService _recipeService;
        private readonly UserService _userService;

        public CommentController(RecipeService recipeService, UserService userService, CommentService commentService)
        {
            _recipeService = recipeService;
            _userService = userService;
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
        [HttpPost("c/{id}")]
        public async Task<IActionResult> Create(int id, [FromBody] Comment? comment)
        {
            if (id == null) return BadRequest();
            Recipe recipe = await _recipeService.GetById(id);
            if (recipe == null) return NotFound(new { Message = "Recipe Not Found!" });

            User userExists = await _userService.GetById(comment.UserId);
            if (userExists == null) return NotFound(new { Message = "User Not Found!" });

            comment.CreatedDate = DateTime.Now;
            comment.RecipeId = id;
            comment.Name = userExists.FullName;

            await _commentService.Create(comment);
            return Ok();

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
