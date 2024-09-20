using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using RestWithASPNET.Business;
using RestWithASPNET.Data.VO;
using Microsoft.AspNetCore.Authorization;

namespace RestWithASPNET.Controllers
{
    [ApiVersion("1")]
    [Route("api/[controller]/v{version:apiVersion}")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private ILoginBusiness _loginBusiness;

        public AuthController(ILoginBusiness loginBusiness)
        {
            _loginBusiness = loginBusiness;
        }

        [HttpPost] // anotation
        [Route("signin")]
        public IActionResult Signin([FromBody] UserVO user)
        {
            if (user == null) return BadRequest("Invalid client request");
            var token = _loginBusiness.ValidateCredentials(user);
            if (token == null) return Unauthorized();
            return Ok(token);
        }

        [HttpPost] // anotation
        [Route("refresh")]
        public IActionResult Refresh([FromBody] TokenVO tokenVO)
        {
            if (tokenVO is null) return BadRequest("Invalid client request");
            var token = _loginBusiness.ValidateCredentials(tokenVO);
            if (token == null) return BadRequest("Invalid client request");
            return Ok(token);
        }

        [HttpGet] // anotation
        [Route("revoke")]
        [Authorize("Bearer")]
        public IActionResult Revoke()
        {
            var username = User.Identity.Name;
            var result = _loginBusiness.RevokeToken(username);
            
            if (!result) return BadRequest("Invalid client request");
            return NoContent();
        }
    }
}