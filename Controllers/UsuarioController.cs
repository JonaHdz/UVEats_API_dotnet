namespace UVEATS_API_DOTNET.Business;
using Microsoft.AspNetCore.Mvc;
using UVEATS_API_DOTNET.Business;
using UVEATS_API_DOTNET.Models;
using UVEATS_API_DOTNET.Domain;

[ApiController]
[Route("[controller]")]

public class UsuarioController : ControllerBase
{
    private readonly ILogger<UsuarioController> _logger;
    private UsuarioProvider _usuario;

    //CONSTRUCTOR
    public UsuarioController(ILogger<UsuarioController> logger, UsuarioProvider usuarioProvider)
    {
        _logger = logger;
        _usuario = usuarioProvider;
    }

    [HttpGet(Name = "RecuperarClientes")]
    public ActionResult RecuperarCliente()
    {
        (int resultado, List<Usuario> clientesList) = _usuario.RecuperarClientes();
        if (resultado == CodigosOperacion.EXITO)
            return new JsonResult(new
            {
                codigo = resultado,
                lista = clientesList
            });
        else if (resultado == CodigosOperacion.ERROR_CONEXION)
            return new JsonResult(new
            {
                codigo = resultado,
                msg = "Error de conexion"
            });
        else
            return new JsonResult(new
            {
                codigo = resultado,
                msg = "Ocurrio un problema"
            });
    }

   

    [HttpPost(Name = "RegistrarCliente")]
    public ActionResult RegistrarCliente([FromBody] UsuarioDomain usuarioTemp)
    {
        (int resultado, Usuario cliente) = _usuario.ReistrarCliente(usuarioTemp);
        if (resultado == CodigosOperacion.EXITO)
            return new JsonResult(new
            {
                codigo = resultado,
                Usuario = cliente
            });
        else if (resultado == CodigosOperacion.SOLICITUD_INCORRECTA)
            return new JsonResult(new
            {
                codigo = resultado,
                msg = "No se puedo realizar la opearacion"
            });
        else
            return new JsonResult(new
            {
                codigo = resultado,
                msg = "Ocurrio un problema"
            });
    }

}