using Microsoft.AspNetCore.Mvc;
using UVEATS_API_DOTNET.Business;
using UVEATS_API_DOTNET.Domain;
using Microsoft.AspNetCore.Authorization;

namespace UVEATS_API_DOTNET.Controllers;

[Authorize]
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

    [HttpPost("RecuperarPedidosClienteRecientes")]
    public ActionResult RecuperarPedidosClienteRecientes([FromBody] int idCliente)
    {
        int resultado = 0;
        List<PedidoDomain> pedidos = new List<PedidoDomain>();
        (resultado, pedidos) = _Pedido.RecuperarPedidosCliente(idCliente);     
            return new JsonResult(new
            {
                codigo = resultado,
                pedidosRecuperados = pedidos
            });
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
    
    [HttpPut("CambiarEstadoProductoPedido")]
    public ActionResult CambiarEstadoProductoPedido([FromBody]int id)
    {
        
        Console.WriteLine("-" + id);
        int resultado = 0;
        resultado = _Pedido.CancelarProductoPedido(id);
        return new JsonResult(new{
            codigo = resultado
        });
    }

    [HttpPut("CancelarPedido")]
    public ActionResult CancelarPedido ([FromBody] int idPedido){
        int resultado = 0;
        resultado = _Pedido.CancelarPedido(idPedido);
        return new JsonResult(new {
            codigo = resultado
        });
    }

        [HttpPut("RecogerPedido")]
    public ActionResult RecogerPedido ([FromBody] int idPedido){
        int resultado = 0;
        resultado = _Pedido.RecogerPedido(idPedido);
        return new JsonResult(new {
            codigo = resultado
        });
    }

    [HttpPost("recuperarHistorialPedidosCliente")]
    public ActionResult recuperarHistorialPedidosCliente([FromBody]int idCliente)
    {
        int resultado = 0;
        List<PedidoDomain> pedidos = new List<PedidoDomain>();
        (resultado, pedidos) = _Pedido.RecuperarHistorialCliente(idCliente);     
            return new JsonResult(new
            {
                codigo = resultado,
                pedidosRecuperados = pedidos
            });
    }

}