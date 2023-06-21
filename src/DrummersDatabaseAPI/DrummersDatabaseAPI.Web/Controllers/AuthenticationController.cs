using DrummersDatabaseAPI.Web.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DrummersDatabaseAPI.Web.Controllers
{
    /// <summary>
    /// controller for getting token
    /// https://app.pluralsight.com/course-player?clipId=efe6c29c-8d59-4b02-94fc-20e3485d33ba
    /// </summary>
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthenticationController> _logger;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="configuration"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public AuthenticationController(IConfiguration configuration, ILogger<AuthenticationController> logger)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger;
        }

        /// <summary>
        /// gives token to client if authorized
        /// </summary>
        /// <param name="request"></param>
        /// <returns>token as string</returns>
        [HttpPost("GetToken")]
        public ActionResult<string> GetToken(AuthenticationRequestBody request)
        {
            try
            {
                // validate user
                var user = GetValidUser(request);
                if (user is null)
                {
                    return Unauthorized("Invalid credentials.");
                }

                // create a token, encode our guid key
                var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"] ?? string.Empty));
                var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                // claims to be used in token
                var claimsForToken = new List<Claim>();
                // custom policy - will generate claim. client must provide proper resources of "all".
                claimsForToken.Add(new Claim("sub", user.UserId.ToString()));
                claimsForToken.Add(new Claim("userName", request.UserName));
                claimsForToken.Add(new Claim("resources", request.Resources ?? ""));

                // here's our token
                // https://www.jwt.io/
                var jwtSecurityToken = new JwtSecurityToken(
                                                            _configuration["Authentication:Issuer"],
                                                            _configuration["Authentication:Audience"],
                                                            claimsForToken,
                                                            DateTime.UtcNow,
                                                            DateTime.UtcNow.AddMinutes(30),
                                                            signingCredentials);

                var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

                return Ok(tokenToReturn);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetToken: {ex}");
                throw;
            }
        }

        /// <summary>
        /// returns the valid user based on data from request
        /// </summary>
        /// <param name="request"></param>
        /// <returns>ApiUser</returns>
        private ApiUser? GetValidUser(AuthenticationRequestBody request)
        {
            var validUsers = _configuration.GetSection("Authentication:ValidUsers").Get<List<ApiUser>>() ?? new List<ApiUser>();
            ApiUser user = validUsers.Where(u => u.UserName.Equals(request.UserName) && u.Password.Equals(request.Password)).FirstOrDefault();
            return user;
        }
    }
}
