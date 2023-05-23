using Microsoft.AspNetCore.Mvc;
using UVEATS_API_DOTNET.Business;
using UVEATS_API_DOTNET.Domain;

namespace UVEATS_API_DOTNET.Controllers;

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
        int resultado = _conversacion.RegistrarConversacion(nuevaConversacion);
        return new JsonResult(new{
            codigo = resultado
        });
    }
}
