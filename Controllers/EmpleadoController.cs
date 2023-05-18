namespace UVEATS_API_DOTNET.Business;
using Microsoft.AspNetCore.Mvc;
using UVEATS_API_DOTNET.Business;
using UVEATS_API_DOTNET.Models;
using UVEATS_API_DOTNET.Domain;

[ApiController]
[Route("[controller]")]

public class EmpleadoController : ControllerBase
{
    private readonly ILogger<EmpleadoController> _logger;
    private EmpleadoProvider _empleado;

    //CONSTRUCTOR
    public EmpleadoController(ILogger<EmpleadoController> logger, EmpleadoProvider empleadoProvider)
    {
        _logger = logger;
        _empleado = empleadoProvider;
    }

    [HttpGet(Name = "RecuperarEmpleados")]
    public ActionResult RecuperarEmpelados()
    {
        (int resultado, List<Usuario> clientesList) = _empleado.RecuperarEmpelados();
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

    [HttpPost(Name = "RegistrarEmpleado")]
    public ActionResult RegistrarEmpleado([FromBody] UsuarioDomain usuarioTemp)
    {
        (int resultado, Usuario cliente) = _empleado.ReistrarEmpleado(usuarioTemp);
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

    [HttpPut(Name = "ModificarEmpleado")]
    public ActionResult ModificarEmpleado([FromBody] UsuarioDomain usuarioTemp)
    {
        (int resultado, Usuario usuarioModificado) = _empleado.ModificarEmpleado(usuarioTemp);
        if (resultado == CodigosOperacion.EXITO)
            return new JsonResult(new
            {
                codigo = resultado,
                empleado = usuarioModificado
            });
        else if (resultado == CodigosOperacion.RECURSO_NO_ENCONTRADO)
            return new JsonResult(new
            {
                codigo = resultado,
                msg = "El usuario no fue encontrado"
            });
        else if (resultado == CodigosOperacion.CONFLICTO)
            return new JsonResult(new
            {
                codigo = resultado,
                msg = "Ocurrio un error"
            });
        else
            return new JsonResult(new
            {
                codigo = resultado,
                msg = "Error de conexion"
            });
    }


}