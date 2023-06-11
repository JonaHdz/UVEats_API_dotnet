using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UVEATS_API_DOTNET.Business;
using UVEATS_API_DOTNET.Domain;

namespace UVEATS_API_DOTNET.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]

public class ResenaController : ControllerBase
{

    private readonly ILogger<ResenaController> _logger;
    private ResenaProvider _resenas;

    //CONSTRUCTOR
    public ResenaController(ILogger<ResenaController> logger, ResenaProvider resenaProvider)
    {
        _logger = logger;
        _resenas = resenaProvider;
    }

    [HttpPost ("RecuperarResenasPorId")]
    public ActionResult RecuperarResenasPorId([FromBody]int idProducto)
    {
        int resultado = 0;
        List<ResenaDomain> resenas = new List<ResenaDomain>();
        (resultado,resenas) = _resenas.RecuperarResenasPorId(idProducto);
        return new JsonResult(new{
            codigo = resultado,
            resenasRecuperas = resenas
        });
    }

    [HttpPost ("RegistrarResena")]
    public ActionResult RegistrarReseña (ResenaRecepcionDomain nuevaResena)
    {
        int resultado = _resenas.RegistrarReseña(nuevaResena);
        return new JsonResult(new{
            codigo = resultado
        });

    }

}