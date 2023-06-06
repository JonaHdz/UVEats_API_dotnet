namespace UVEATS_API_DOTNET.Controllers;
using Microsoft.AspNetCore.Mvc;
using UVEATS_API_DOTNET.Business;
using UVEATS_API_DOTNET.Models;
using UVEATS_API_DOTNET.Domain;
//clases para el token
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

[ApiController]
[Route("[controller]")]

public class LoginController : ControllerBase
{
    private readonly ILogger<LoginController> _logger;
    private LoginProvider _login;
    private IConfiguration config;

    //CONSTRUCTOR
    public LoginController(ILogger<LoginController> logger, LoginProvider loginProvider, IConfiguration config)
    {
        _logger = logger;
        _login = loginProvider;
        this.config = config;
    }

    [HttpPost("ValidarUsuarioLogin")]
    public ActionResult ValidarUsuarioLogin([FromBody] Domain.LoginDomain credenciales)
    {
        Console.WriteLine("COREO Recibido" + credenciales.correo);
        (int resultado, Usuario usuarioRecuperado) = _login.IniciarSesion(credenciales);
        //return new JsonResult("resultado,usuarioRecuperado");
        if (resultado == CodigosOperacion.EXITO)
        {
            string jwtToken = generarToken(usuarioRecuperado);
            return new JsonResult(new
            {
                codigo = resultado,
                usuario = usuarioRecuperado,
                token = jwtToken
            });
        }
        return new JsonResult(new { codigo = resultado });
    }


    [ApiExplorerSettings(IgnoreApi = true)]
    public string generarToken(Usuario usuario)
    {
        Console.WriteLine("CORREO en token:   " + usuario.Firstname+"   "  +usuario.Correo);
        string token="";
        try
        {
            var claims = new[]
        {
            new Claim(ClaimTypes.Name, "usuario.Firstname"),
            new Claim(ClaimTypes.Email, usuario.Correo)

        };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("JWT:Key").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var securityToken = new JwtSecurityToken(
                                claims: claims,
                                expires: DateTime.Now.AddMinutes(60),
                                signingCredentials: creds
                );
             token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            
        }
        catch (Exception e)
        {
            Console.WriteLine( e.StackTrace);
        }
return token;


    }


    }