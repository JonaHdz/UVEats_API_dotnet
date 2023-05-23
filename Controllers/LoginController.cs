namespace UVEATS_API_DOTNET.Controllers;
using Microsoft.AspNetCore.Mvc;
using UVEATS_API_DOTNET.Business;
using UVEATS_API_DOTNET.Models;
using UVEATS_API_DOTNET.Domain;

[ApiController]
[Route("[controller]")]

public class LoginController: ControllerBase
{
        private readonly ILogger<LoginController> _logger;
        private LoginProvider _login;

        //CONSTRUCTOR
        public LoginController(ILogger<LoginController> logger, LoginProvider loginProvider)
        {
            _logger = logger;
            _login = loginProvider;
        }

        [HttpPost (Name ="ValidarUsurioLogin")]
        public ActionResult ValidarUsuarioLogin(Domain.LoginDomain credenciales)
        {
            (int resultado, Usuario usuarioRecuperado) = _login.IniciarSesion(credenciales);
            //return new JsonResult("resultado,usuarioRecuperado");
            if(resultado == CodigosOperacion.EXITO)
            return new JsonResult(new {usuario = usuarioRecuperado});
            else if(resultado == CodigosOperacion.RECURSO_NO_ENCONTRADO)
            return new JsonResult(new{mensaje = "NO SE ENCONTRO EL USUARIO"});
            return new JsonResult(new{mensaje = "ERROR CIRTICO"});
        }
        

}