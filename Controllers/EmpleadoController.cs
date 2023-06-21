namespace UVEATS_API_DOTNET.Business;
using Microsoft.AspNetCore.Mvc;
using UVEATS_API_DOTNET.Business;
using UVEATS_API_DOTNET.Models;
using UVEATS_API_DOTNET.Domain;
using API_PROYECTO.Models;

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
        (int resultado, List<UsuarioDomain> clientesList) = _empleado.RecuperarEmpelados();
            return new JsonResult(new
            {
                codigo = resultado,
                lista = clientesList
            });
    }

    [HttpPost(Name = "RegistrarEmpleado")]
    public ActionResult RegistrarEmpleado([FromBody] UsuarioDomain usuarioTemp)
    {
        usuarioTemp.FotoBytes = Convert.FromBase64String(usuarioTemp.Foto);
        Console.WriteLine("RGISTRANDO EMPELADO");
       (int resultado, Usuario cliente) = _empleado.ReistrarEmpleado(usuarioTemp);
      
            return new JsonResult(new
            {
                codigo = resultado,
                Usuario = cliente
            });
            
    }

    [HttpPut(Name = "ModificarEmpleado")]
    public ActionResult ModificarEmpleado([FromBody] UsuarioDomain usuarioTemp)
    {
        if(usuarioTemp.Foto != null)
        {
        usuarioTemp.FotoBytes = Convert.FromBase64String(usuarioTemp.Foto);
            Console.WriteLine("ID MODIFICACION + " + usuarioTemp.IdUsuario);
        }
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

    [HttpPost ("EliminarEmpleado")]
    public ActionResult EliminarEmpleado( [FromBody]int id)
    {
        Console.WriteLine("ID ELIMINACION  " + id);
        int resultado = _empleado.EliminarEmpleado(id);
        return new JsonResult(new {
            codigo  = 200
        });
    }


}