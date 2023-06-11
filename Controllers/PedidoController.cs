using Microsoft.AspNetCore.Mvc;
using UVEATS_API_DOTNET.Business;
using UVEATS_API_DOTNET.Domain;

namespace UVEATS_API_DOTNET.Controllers;

[ApiController]
[Route("[controller]")]

public class PedidoController : ControllerBase
{

    private readonly ILogger<PedidoController> _logger;
    private PedidoProvider _Pedido;

    //CONSTRUCTOR
    public PedidoController(ILogger<PedidoController> logger, PedidoProvider pedidoProvider)
    {
        _logger = logger;
        _Pedido = pedidoProvider;
    }

    [HttpPost("RegistrarPedidoCliente")]
    public ActionResult RegistrarPedido([FromBody] PedidoDomain pedido)
    {
        int resultado = _Pedido.RegistrarPedido(pedido);
        if (resultado == CodigosOperacion.ENTIDAD_NO_PROCESABLE)
        {
            return new JsonResult(new
            {
                codigo = resultado,

            });
        }
        else if (resultado == CodigosOperacion.SOLICITUD_INCORRECTA)
        {
            return new JsonResult(new
            {
                codigo = resultado,

            });
        }
        else
        {
            return new JsonResult(new
            {
                codigo = resultado,
                msg = "Pedido registrado con exito"

            });
        }
    }

    [HttpPost("RecuperarPedidosCliente")]
    public ActionResult RecuperarPedidosCliente([FromBody] int idCliente)
    {
        int resultado = 0;
        List<PedidoDomain> pedidos = new List<PedidoDomain>();
        (resultado, pedidos) = _Pedido.RecuperarPedidosCliente(idCliente);
        if (resultado == CodigosOperacion.ENTIDAD_NO_PROCESABLE)
        {
            return new JsonResult(new
            {
                codigo = resultado,
                msg = "operacion fallida"
            });
        }
        else
        {
            return new JsonResult(new
            {
                codigo = resultado,
                msg = "Pedidos recuperados",
                pedidosRecuperados = pedidos

            });
        }
    }

    [HttpGet("RecuperarPedidosEmpleado")]
    public ActionResult RecuperarPedidosEmpleado()
    {
        int resultado = 0;
        List<PedidoDomain> pedidos = new List<PedidoDomain>();
        (resultado, pedidos) = _Pedido.RecuperarPedidosEmpleado();
            return new JsonResult(new
            {
                codigo = resultado,
                pedidosRecuperados = pedidos
            });
        
    }

}