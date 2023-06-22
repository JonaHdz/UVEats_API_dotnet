using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UVEATS_API_DOTNET.Business;
using UVEATS_API_DOTNET.Domain;

namespace UVEATS_API_DOTNET.Controllers;
 [Authorize]
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
            return new JsonResult(new
            {
                codigo = resultado,
               
                productosRecuperados = productos
            });
    }

    [HttpGet("RecuperarProductosEmpleado")]
    public ActionResult RecuperarProductosEmpleado()
    {

        int resultado = 0;
        List<ProductoDomain> productos = new List<ProductoDomain>();
        (resultado, productos) = _productos.RecuperarProductosEmpleado();
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
        if(nuevoProducto.FotoProductoString != null)
            nuevoProducto.FotoProducto = Convert.FromBase64String(nuevoProducto.FotoProductoString);
        int resultado = _productos.RegistrarProducto(nuevoProducto);
        return new JsonResult(new{
            codigo = resultado
        });
    }

    [HttpPut ("ModificarProducto")]
    public ActionResult ModificarProducto(ProductoDomain productoModificado)
    {
        if(productoModificado.FotoProductoString != null)
        productoModificado.FotoProducto = Convert.FromBase64String(productoModificado.FotoProductoString);
        int resultado = _productos.ModificarProducto(productoModificado);
        return new JsonResult(new{
            codigo = resultado
        });
    }
}