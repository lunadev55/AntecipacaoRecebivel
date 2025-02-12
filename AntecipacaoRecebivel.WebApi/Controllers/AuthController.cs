using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AntecipacaoRecebivel.WebApi.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;

    public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration, ILogger<AuthController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _logger = logger;
    }

    [HttpPost("Registrar")]
    public async Task<IActionResult> Registrar([FromBody] AntecipacaoRecebivel.Domain.Entities.Auth.RegisterRequest request)
    {
        _logger.LogInformation("Requisicao para criar novo usuario: {0}", request.Username);

        var user = new IdentityUser
        {
            UserName = request.Username
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            _logger.LogInformation("Erro ao criar usuario: {0}", request.Username);
            return BadRequest(result.Errors);
        }

        _logger.LogInformation("Usuario {0} criado com sucesso", request.Username);
        return Ok(new { Message = "Usuário registrado com sucesso." });
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] AntecipacaoRecebivel.Domain.Entities.Auth.LoginRequest request)
    {
        _logger.LogInformation("Requisicao para login do Usuario: {0}", request.Username);
        var user = await _userManager.FindByNameAsync(request.Username);

        if (user == null)
        {
            _logger.LogInformation("Usuario nao encontrado.");
            return Unauthorized(new { Message = "Credenciais inválidas." });
        }            

        var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);

        if (!result.Succeeded)
        {
            _logger.LogInformation("Credenciais invalidas.");
            return Unauthorized(new { Message = "Credenciais inválidas." });
        }

        _logger.LogInformation("Login bem sucedido!.");
        var token = GeraTokenJwt(user);
        return Ok(new { Token = token });
    }

    private string GeraTokenJwt(IdentityUser user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),            
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
