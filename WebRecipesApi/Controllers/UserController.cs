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
        public async Task<IEnumerable<User>> Get(string? searchFilter = null)
        {
            return await _userService.Search(searchFilter);
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            //NULO
            if (user == null) return BadRequest();

            user.Email = user.Email.ToLower();

            User userExists = await _userService.GetByEmail(user.Email);


            if (userExists == null) return NotFound(new { Message = "User Not Found!" });

            if (!PasswordHasher.VerifyPassword(user.Password, userExists.Password)) return BadRequest(new { Message = "Password Incorret" });
            if (userExists.IsBlocked) return BadRequest(new { Message = "User Blocked" });

            userExists.Token = _jwtService.CreateJwt(userExists);

            var newAcessToken = userExists.Token;

            var newRefreshToken = _jwtService.CreateRefreshToken();
            userExists.RefreshToken = newRefreshToken;
            userExists.RefreshTokenExpiryTime = DateTime.Now.AddDays(5);
            await _context.SaveChangesAsync();
            return Ok(new TokenApiDTO()
            {
                AcessToken = newAcessToken,
                RefreshToken = newRefreshToken
            });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(TokenApiDTO tokenApiDto)
        {
            if (tokenApiDto is null) return BadRequest("Invalid Client Request");

            string acessToken = tokenApiDto.AcessToken;
            string refreshToken = tokenApiDto.RefreshToken;

            var principal = _jwtService.GetPrincipalFromExpiredToken(acessToken);
            var email = principal.Identity.Name;

            User user = await _userService.GetByEmail(email);

            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now) return BadRequest("Invalid Request");



            var newAcessToken = _jwtService.CreateJwt(user);
            var newrefreshToken = _jwtService.CreateRefreshToken();
            user.RefreshToken = newrefreshToken;
            await _context.SaveChangesAsync();
            return Ok(new TokenApiDTO()
            {
                AcessToken = newAcessToken,
                RefreshToken = newrefreshToken
            });
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            //Check if User is not null 
            if (user == null) return BadRequest();

            //Check Email Exists on DB
            if (await _userService.CheckEmailExistsAsync(user.Email)) return BadRequest(new { Message = "Email Already Exists!" });

            //Check PasswordStrenght
            string password = _userService.CheckPasswordStrength(user.Password);
            if (!string.IsNullOrEmpty(password)) return BadRequest(new { Message = password.ToString() });

            //Create User
            user.Email = user.Email.ToLower();
            await _userService.Create(user);

            // Return OK message to client
            return Ok(new { Message = "User Registered!" });
        }



        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<User> Get(int id)
        {
            return await _userService.GetById(id);
        }

        // POST api/<UserController>
        [HttpPost]
        public void Post([FromBody] User user)
        {
            _userService.Update(user, user);
        }

        // PUT api/<UserController>/5
        [HttpPut]
        public async Task<IActionResult> Put(int id, [FromBody] User? user)
        {
            //NULO
            if (user == null) return BadRequest();

            User userExists = await _userService.GetById(id);


            if (userExists == null) return NotFound(new { Message = "User Not Found!" });

            // Check Password Strenght
            if (!string.IsNullOrEmpty(user.Password) && user.Password != userExists.Password)
            {
                string password = _userService.CheckPasswordStrength(user.Password);
                if (!string.IsNullOrEmpty(password)) return BadRequest(new { Message = password.ToString() });
            }

            await _userService.Update(userExists, user);

            return Ok(new { Message = "Sucessfully changed!" });
        }


        [HttpPut("img={id}")]
        public async Task<IActionResult> SaveImage(int id, [FromForm] IFormFile imageData)
        {
            User userExists = await _userService.GetById(id);

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

        [HttpGet("img={id}")]
        public async Task<IActionResult> GetImage(int id)
        {
            User userExists = await _userService.GetById(id);
            if (userExists == null)

                return NotFound(new { Message = "User Not Found!" });

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

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.Delete(id);
            return Ok();
        }

    }
}