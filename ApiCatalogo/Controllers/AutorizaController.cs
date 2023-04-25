using ApiCatalogo.DTOs;
using ApiCatalogo.Migrations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;

namespace ApiCatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorizaController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager; 
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _config; 

        public AutorizaController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = configuration;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return "AutorizaController :: Acessado em : " + DateTime.Now.ToString(); 
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody] UsuarioDTO model)
        {
            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if(!result.Succeeded)
            {
                return BadRequest(result.Errors); 
            }

            await _signInManager.SignInAsync(user, false);
            return Ok(GeraToken(model)); 
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UsuarioDTO userInfo)
        {
            var result = await _signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password, 
                isPersistent: false, lockoutOnFailure: false);

            if(result.Succeeded)
            {
                return Ok(GeraToken(userInfo));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Login inválido!"); 
                return BadRequest(ModelState);
            }
        }

        private UsuarioToken GeraToken(UsuarioDTO userInfo)
        {
            //define declarações do usuário
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
                new Claim("meuPet", "pipoca"), 
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            //gera uma chave com base em um algoritmo simetrico
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:key"])); 

            //gera uma assinatura digital do token usando o algoritmo hmac e a chave privada
            var credenciais = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //Tempo de expiração do token
            var expipacao = _config["TokenConfiguration:ExpireHours"]; 
            var expiration = DateTime.UtcNow.AddHours(double.Parse(expipacao));

            //Classe que representa o token e gera o token
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _config["TokenConfiguration:Issuer"],
                audience: _config["TokenConfiguration:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credenciais);

            //retorna os dados com o token e informações
            return new UsuarioToken()
            {
                Authenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
                Message = "Token JWT OK"
            }; 
        }
    }
}
