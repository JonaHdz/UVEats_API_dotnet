using Microsoft.AspNetCore.Mvc;
using UVEATS_API_DOTNET.Business;
using UVEATS_API_DOTNET.Domain;

namespace UVEATS_API_DOTNET.Controllers;

[ApiController]
[Route("[controller]")]

public class ProductoController : ControllerBase
{

    private readonly ILogger<ProductoController> _logger;
    private ProductoProvider _productos;

    //CONSTRUCTOR
    public ProductoController(ILogger<ProductoController> logger, ProductoProvider productoProvider)
    {
        _logger = logger;
        _productos = productoProvider;
    }

    [HttpGet("RecuperarProductos")]
    public ActionResult RecuperarProductos()
    {
        int resultado = 0;
        List<ProductoDomain> productos = new List<ProductoDomain>();
        (resultado, productos) = _productos.RecuperarProductos();
        if (resultado == CodigosOperacion.ENTIDAD_NO_PROCESABLE)
            return new JsonResult(new
            {
                codigo = resultado,
                msg = "no se pudo recuperar la informacion de pedidos",
                productosRecuperados = productos
            });
        else
            return new JsonResult(new
            {
                codigo = resultado,
                msg = "lista recuperada",
                productosRecuperados = productos
            });
    }

    [HttpPost ("RegistrarProducto")]
    public ActionResult RegistrarProducto([FromBody] ProductoDomain nuevoProducto)
    {
        int resultado = _productos.RegistrarProducto(nuevoProducto);
        return new JsonResult(new{
            codigo = resultado
        });
    }

    [HttpPut ("ModificarProducto")]
    public ActionResult ModificarProducto(ProductoDomain productoModificado)
    {
        int resultado = _productos.ModificarProducto(productoModificado);
        return new JsonResult(new{
            codigo = resultado
        });
    }
}