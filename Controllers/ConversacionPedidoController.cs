using Microsoft.AspNetCore.Mvc;
using UVEATS_API_DOTNET.Business;
using UVEATS_API_DOTNET.Domain;
using Microsoft.AspNetCore.Authorization;

namespace UVEATS_API_DOTNET.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]

public class ConversacionPedidoController : ControllerBase
{

    private readonly ILogger<ConversacionPedidoController> _logger;
    private ConversacionPedidoProvider _conversacion;

    //CONSTRUCTOR
    public ConversacionPedidoController(ILogger<ConversacionPedidoController> logger, ConversacionPedidoProvider pedidoProvider)
    {
        _logger = logger;
        _conversacion = pedidoProvider;
    }

    [HttpPost("RegistrarConversacion")]
    public ActionResult RegistrarConversacion([FromBody]ConversacionPedidoDomain nuevaConversacion )
    {
        
        int resultado = 0; 
        resultado = _conversacion.RegistrarConversacion(nuevaConversacion);
        return new JsonResult(new{
            codigo = resultado
        });
    }

[HttpPost ("RecuperarConversacionPedido")]
    public ActionResult RecuperarConversacionPedido  ([FromBody]int idPedido) 
    {
        ConversacionPedidoDomain conversacion = _conversacion.RecuperaConvesacionPedido(idPedido);
        return new JsonResult(new{
            resultado = conversacion
        });

    }
}
