namespace UVEATS_API_DOTNET.Business;
using Microsoft.AspNetCore.Mvc;
using UVEATS_API_DOTNET.Business;
using API_PROYECTO.Models;
using UVEATS_API_DOTNET.Domain;
using Microsoft.AspNetCore.Authorization;


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


    [HttpGet("RecuperarClientes")]
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




    [HttpPost("RegistrarCliente")]
    public ActionResult RegistrarCliente([FromBody] UsuarioDomain usuarioTemp)
    {
        (int resultado, Usuario cliente) = _usuario.ReistrarCliente(usuarioTemp);
        if (resultado == CodigosOperacion.EXITO)
            return new JsonResult(new
            {
                codigo = resultado,
            });
        else if (resultado == CodigosOperacion.SOLICITUD_INCORRECTA)
            return new JsonResult(new
            {
                codigo = resultado,

            });
        else
            return new JsonResult(new
            {
                codigo = resultado,

            });
    }

    [HttpPost("RecuperarClienteId")]
    public ActionResult RecuperaClientePorId([FromBody] int idClinte)
    {
        (int resultado, UsuarioDomain cliente) = _usuario.RecuperaClintePorId(idClinte);
        return new JsonResult(new
        {
            codigo = resultado,
            cliente = cliente
        });
    }

    [HttpPost("ValidarCorreo")]
    public ActionResult ValidarCorreo([FromBody] string correo)
    {
        int resultado = _usuario.ValidarCorreo(correo);
        return new JsonResult(new
        {

            codigo = resultado
        });


    }

    [HttpPatch("ActualizarUsuario")]
    public ActionResult ActualizarUsuario(UsuarioDomain usuarioTemp)
    {
        int resultado = _usuario.ModificarUsuario(usuarioTemp);
        return new JsonResult(new { codigo = resultado });

    }

}