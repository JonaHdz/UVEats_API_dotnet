namespace UVEATS_API_DOTNET.Business;
using UVEATS_API_DOTNET.Models;
using System.Globalization;
using UVEATS_API_DOTNET.Domain;
using Microsoft.EntityFrameworkCore;

public class ProductoProvider
{
    private UveatsContext _connectionModel;

    //Constructor del DataContext
    public ProductoProvider(UveatsContext connectionModel)
    {
        string Culture = "es-MX";
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Culture);
        _connectionModel = connectionModel;
    }

    public (int, List<ProductoDomain>) RecuperarProductos() //CU08 VISUALIZAR MENU
    {
        int resultado = 0;
        List<ProductoDomain> productos = new List<ProductoDomain>();
        try
        {
            var productosList = _connectionModel.Productos.Where(a => a.EstadoProducto.Equals(EstadosProducto.PRODUCTO_DISPONIBLE)).ToList();
            Console.WriteLine("PRODUCTOS COUNT-----------------------" + productosList.Count());
            foreach (var util in productosList)
            {
                ProductoDomain productoTemp = new ProductoDomain();
                productoTemp.IdProducto = util.IdProducto;
                productoTemp.Nombre = util.Nombre;
                productoTemp.Descripcion = util.Descripcion;
                productoTemp.Precio = util.Precio;
                productoTemp.IdCategoria = util.IdCategoria;

                productoTemp.EstadoProducto = util.EstadoProducto;
                productoTemp.FotoProducto = util.FotoProducto;
                productos.Add(productoTemp);
            }

            var categoriasList = _connectionModel.Categorias.ToList();
            foreach (var util in categoriasList)
            {
                for (int i = 0; i < productos.Count(); i++)
                {
                    if (productos[i].IdCategoria == util.IdCategoria)
                        productos[i].Categoria = util.Categoria1;
                }
            }
            resultado = CodigosOperacion.EXITO;
        }
        catch (DbUpdateException)
        {
            resultado = CodigosOperacion.ENTIDAD_NO_PROCESABLE;
        }
        return (resultado, productos);
    }

    public int RegistrarProducto(ProductoDomain producto)
    {
        int resultado = 0;
        Producto productoTemp = new Producto();
        productoTemp.Nombre = producto.Nombre;
        productoTemp.Descripcion = producto.Descripcion;
        productoTemp.Precio = producto.Precio;
        productoTemp.IdCategoria = producto.IdCategoria;
        productoTemp.EstadoProducto = producto.EstadoProducto;
        productoTemp.FotoProducto = producto.FotoProducto;
        try
        {
            _connectionModel.Productos.Add(productoTemp);
            int cambios = _connectionModel.SaveChanges();
            if (cambios > 0)
                resultado = CodigosOperacion.EXITO;
            else
                resultado = CodigosOperacion.SOLICITUD_INCORRECTA;
        }
        catch (DbUpdateException)
        {
            resultado = CodigosOperacion.ENTIDAD_NO_PROCESABLE;
        }
        return resultado;
    }

    public int ModificarProducto(ProductoDomain productoModificado)
    {
        int resultado = 0;
        try
        {
            var producto = _connectionModel.Productos.Where(a => a.IdProducto == productoModificado.IdProducto).FirstOrDefault();
            if (producto != null)
            {
                producto.Nombre = productoModificado.Nombre;
                producto.Descripcion = productoModificado.Descripcion;
                producto.Precio = productoModificado.Precio;
                producto.IdCategoria = productoModificado.IdCategoria;
                producto.EstadoProducto = productoModificado.EstadoProducto;
                producto.FotoProducto = productoModificado.FotoProducto;
                int cambios = _connectionModel.SaveChanges();
                if (cambios == 1)
                    resultado = CodigosOperacion.EXITO;
                else
                    resultado = CodigosOperacion.SOLICITUD_INCORRECTA;
            }
            else
                resultado = CodigosOperacion.RECURSO_NO_ENCONTRADO;
        }
        catch (DbUpdateException)
        {
            resultado = CodigosOperacion.ENTIDAD_NO_PROCESABLE;
        }
        return resultado;
    }


}