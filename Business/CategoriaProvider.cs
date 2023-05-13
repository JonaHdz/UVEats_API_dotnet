namespace UVEATS_API_DOTNET.Business;
using UVEATS_API_DOTNET.Models;
using System.Globalization;
using UVEATS_API_DOTNET.Domain;
using Microsoft.EntityFrameworkCore;

public class CategoriaProvider
{

    private UveatsContext _connectionModel;

    //Constructor del DataContext
    public CategoriaProvider(UveatsContext connectionModel)
    {
        string Culture = "es-MX";
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Culture);
        _connectionModel = connectionModel;
    }

    //METODOS DEL DAO
    public (int, List<Categoria>) ObtenerCategorias()
    {
        List<Categoria> categorias = new List<Categoria>();
        int resultado = 0;
        try
        {
            categorias = _connectionModel.Categorias.ToList();
            resultado = CodigosOperacion.EXITO;
        }
        catch (Exception)
        {
            resultado = CodigosOperacion.ERROR_CONEXION;
        }
        return (resultado, categorias);
    }

    public int RegistrarCategoria(Domain.CategoriaDomain nuevaCategoria)
    {
        int resultado = 0;
        try
        {
            Categoria categoria = new Categoria();
            categoria.Categoria1 = nuevaCategoria.Categoria;
            _connectionModel.Categorias.Add(categoria);
            int cambios = _connectionModel.SaveChanges();
            if (cambios == 1)
                resultado = CodigosOperacion.EXITO;
            else
                resultado = CodigosOperacion.ENTIDAD_NO_PROCESABLE;
        }
        catch (DbUpdateException)
        {
            resultado = CodigosOperacion.SOLICITUD_INCORRECTA;
        }
        return resultado;
    }

    public int BorrarCategoria(int idCategoria)
    {
        int resultado = 0;
        Categoria categoriaSeleccionda;
        try
        {
            categoriaSeleccionda = _connectionModel.Categorias.FirstOrDefault(a => a.IdCategoria == idCategoria);
            if (categoriaSeleccionda.IdCategoria == idCategoria)
            {
                _connectionModel.Categorias.Remove(categoriaSeleccionda);
                int cambios = _connectionModel.SaveChanges();
                if (cambios == 1)
                    resultado = CodigosOperacion.EXITO;
                else
                    resultado = CodigosOperacion.ENTIDAD_NO_PROCESABLE;
            }
        }
        catch (DbUpdateException)
        {
            resultado = CodigosOperacion.SOLICITUD_INCORRECTA;
        }


        return resultado;
    }

    public int ModificarCategoria(CategoriaDomain categoria)
    {
        int resultado = 0;
        Categoria categoriaSeleccionada = new Categoria();
        try
        {
            categoriaSeleccionada = _connectionModel.Categorias.FirstOrDefault(a => a.IdCategoria == categoria.IdCategoria);
            if (categoriaSeleccionada != null && categoriaSeleccionada.IdCategoria == categoria.IdCategoria)
            {
                categoriaSeleccionada.Categoria1 = categoria.Categoria;
                _connectionModel.SaveChanges();
                resultado = CodigosOperacion.EXITO;
            }else
                resultado = CodigosOperacion.RECURSO_NO_ENCONTRADO;
        }
        catch (DbUpdateException)
        {
            resultado = CodigosOperacion.ENTIDAD_NO_PROCESABLE;
        }
        return resultado;
    }


}