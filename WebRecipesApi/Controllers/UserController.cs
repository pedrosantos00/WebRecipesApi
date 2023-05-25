using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebRecipesApi.Domain;
using WebRecipesApi.BusinessLogic;
using WebRecipesApi.ModelDTO;
using WebRecipesApi.DAL;
using static System.Net.Mime.MediaTypeNames;
using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;

namespace WebRecipesApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly JWTService _jwtService;
        private readonly WebRecipesDbContext _context;

        public UserController(UserService userService, JWTService jwtService, WebRecipesDbContext context)
        {
            _userService = userService;
            _jwtService = jwtService;
            _context = context;
        }

        // GET: api/<UserController>
        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<User>> Get(string? searchFilter = null)
        {
            return await _userService.Search(searchFilter);
        }

        // POST: /user/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            // Check if user object is null
            if (user == null)
                return BadRequest();

            user.Email = user.Email.ToLower();

            User userExists = await _userService.GetByEmail(user.Email);

            // Check if user exists
            if (userExists == null)
                return NotFound(new { Message = "User Not Found!" });

            // Verify password
            if (!PasswordHasher.VerifyPassword(user.Password, userExists.Password))
                return BadRequest(new { Message = "Incorrect Password" });

            // Check if user is blocked
            if (userExists.IsBlocked)
                return BadRequest(new { Message = "User Blocked" });

            // Generate access token and refresh token
            userExists.Token = _jwtService.CreateJwt(userExists);
            var newAccessToken = userExists.Token;
            var newRefreshToken = _jwtService.CreateRefreshToken();
            userExists.RefreshToken = newRefreshToken;
            userExists.RefreshTokenExpiryTime = DateTime.Now.AddDays(14);
            await _context.SaveChangesAsync();

            // Return tokens to the client
            return Ok(new TokenApiDTO()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }

        // POST: /user/refresh
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(TokenApiDTO? tokenApiDto)
        {
            if (tokenApiDto is null)
                return BadRequest("Invalid Client Request");

            string accessToken = tokenApiDto.AccessToken;
            string refreshToken = tokenApiDto.RefreshToken;

            var principal = _jwtService.GetPrincipalFromExpiredToken(accessToken);
            var email = principal.Identity.Name;

            User user = await _userService.GetByEmail(email);

            // Check if user exists and if the provided refresh token is valid
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return BadRequest("Invalid Request");

            // Generate new access token and refresh token
            var newAccessToken = _jwtService.CreateJwt(user);
            var newRefreshToken = _jwtService.CreateRefreshToken();
            user.RefreshToken = newRefreshToken;
            await _context.SaveChangesAsync();

            // Return new tokens to the client
            return Ok(new TokenApiDTO()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }

        // POST: /user/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            // Check if user object is null
            if (user == null)
                return BadRequest();

            // Check if email already exists
            if (await _userService.CheckEmailExistsAsync(user.Email))
                return BadRequest(new { Message = "Email Already Exists!" });

            // Check password strength
            string password = _userService.CheckPasswordStrength(user.Password);
            if (!string.IsNullOrEmpty(password))
                return BadRequest(new { Message = password.ToString() });

            // Create the user
            user.Email = user.Email.ToLower();
            await _userService.Create(user);

            // Return OK message to the client
            return Ok(new { Message = "User Registered!" });
        }

        // GET: /user/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<User> Get(int id)
        {
            return await _userService.GetById(id);
        }

        // POST: /user
        [HttpPost]
        [Authorize]
        public void Post([FromBody] User user)
        {
            _userService.Update(user, user);
        }

        // PUT: /user?id={id}
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Put(int id, [FromBody] User? user)
        {
            // Check if user object is null
            if (user == null)
                return BadRequest();

            User userExists = await _userService.GetById(id);

            // Check if user exists
            if (userExists == null)
                return NotFound(new { Message = "User Not Found!" });

            // Check password strength
            if (!string.IsNullOrEmpty(user.Password) && user.Password != userExists.Password)
            {
                string password = _userService.CheckPasswordStrength(user.Password);
                if (!string.IsNullOrEmpty(password))
                    return BadRequest(new { Message = password.ToString() });
            }

            await _userService.Update(userExists, user);

            return Ok(new { Message = "Successfully changed!" });
        }

        // PUT: /user/img={id}
        [HttpPut("img={id}")]
        [Authorize]
        public async Task<IActionResult> SaveImage(int id, [FromForm] IFormFile imageData)
        {
            User userExists = await _userService.GetById(id);

            // Check if user exists
            if (userExists == null)
                return NotFound(new { Message = "User Not Found!" });

            using (var memoryStream = new MemoryStream())
            {
                await imageData.CopyToAsync(memoryStream);
                userExists.ProfilePicture = memoryStream.ToArray();
            }

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Successfully changed!" });
        }

        // GET: /user/img={id}
        [HttpGet("img={id}")]
        [Authorize]
        public async Task<IActionResult> GetImage(int id)
        {
            User userExists = await _userService.GetById(id);

            // Check if user exists
            if (userExists == null)
                return NotFound(new { Message = "User Not Found!" });

            var imageStream = new MemoryStream(userExists.ProfilePicture);
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(imageStream)
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            return new FileStreamResult(imageStream, "image/jpeg");
        }

        // DELETE: /user/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _userService.Delete(id))
                return Ok(new { Message = "User Deleted!" });
            else
                return NotFound(new { Message = "User Not Found!" });
        }
    }
}
