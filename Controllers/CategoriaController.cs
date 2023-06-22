namespace UVEATS_API_DOTNET.Business;
using Microsoft.AspNetCore.Mvc;
using UVEATS_API_DOTNET.Business;
using UVEATS_API_DOTNET.Models;
using UVEATS_API_DOTNET.Domain;
using Microsoft.AspNetCore.Authorization;

//[Authorize]
[ApiController]
[Route("[controller]")] 

public class CategoriaController: ControllerBase
{
        private readonly ILogger<CategoriaController> _logger;
        private CategoriaProvider _categoria;

        //CONSTRUCTOR
        public CategoriaController(ILogger<CategoriaController> logger, CategoriaProvider categoriaProvider)
        {
            _logger = logger;
            _categoria = categoriaProvider;
        }

        [HttpGet (Name ="RecuperarCategorias")]
        public ActionResult RecuperarCategorias()
        {
            (int resultado, List<CategoriaDomain> categoriasRecuperadas) = _categoria.ObtenerCategorias();
           //if(resultado == CodigosOperacion.ERROR_CONEXION)
             //   return new JsonResult(new{codigo = resultado ,categoriasRecuperadas});
            return new JsonResult(new{
                codigo = resultado, categoriasRecuperadas
                });
        }

        [HttpPost(Name = "RegistrarCategoria")]
        public ActionResult RegistrarCategoria([FromBody] CategoriaDomain nuevaCategoria)
        {
            
            int resultado = _categoria.RegistrarCategoria(nuevaCategoria);
            if(resultado == CodigosOperacion.EXITO)
                return new JsonResult(new {codigo = resultado, msg = "Categoría registrada exitosamente"});
            else
                return new JsonResult(new {codigo = resultado, msg = "Error al realizar registro"});
        }

        [HttpDelete (Name = "BorrarCategoria")]
        public ActionResult BorrarCategoria([FromBody] int idCategoria)
        {
            int resultado = _categoria.BorrarCategoria(idCategoria);
            if(resultado == CodigosOperacion.EXITO)
                return new JsonResult(new {codigo = resultado, msg = "categoria eliminada con exito"});
            else
                return new JsonResult(new {codigo = resultado, msg = "Error al elimnar categoria"});
        }

        [HttpPut (Name = "ModificarCategoria")]

        public ActionResult ModificarCategoria([FromBody] CategoriaDomain categoria)
        {
            int resultado = _categoria.ModificarCategoria(categoria);
            if(resultado == CodigosOperacion.EXITO)
            return new JsonResult( new {codigo = resultado, msg = "Categoria modificada exitosamente"});
            else if(resultado == CodigosOperacion.RECURSO_NO_ENCONTRADO)
                return new JsonResult( new {codigo = resultado, msg = "No se encontro el elemento para poder realizar la operacion" });
            else
                return new JsonResult(new {codigo = resultado, msg = "Error al realiar operacion"});
        }
        
        

}