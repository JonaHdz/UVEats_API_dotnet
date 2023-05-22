namespace UVEATS_API_DOTNET.Business;
using UVEATS_API_DOTNET.Models;
using System.Globalization;
using UVEATS_API_DOTNET.Domain;
using Microsoft.EntityFrameworkCore;

public class ResenaProvider
{
    private UveatsContext _connectionModel;

    //Constructor del DataContext
    public ResenaProvider(UveatsContext connectionModel)
    {
        string Culture = "es-MX";
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Culture);
        _connectionModel = connectionModel;
    }
    public (int, List<ResenaDomain>) RecuperarResenasPorId(int idProducto)//CU03 visualizar reseñas
    {
        int resultado = 0;
        List<ResenaDomain> resenas = new List<ResenaDomain>( );
        try
        {
            var resenasList =_connectionModel.Resenas.Where( a => a.IdProducto == idProducto).ToList();

            foreach(var util in resenasList)
            {
                ResenaDomain resenaTemp = new ResenaDomain();
                resenaTemp.Resena1 = util.Resena1;
                resenaTemp.IdProducto = util.IdProducto;
                resenaTemp.IdUsuario = util.IdUsuario;
                resenas.Add(resenaTemp);
            }
        }
        catch (DbUpdateException)
        {
            resultado = CodigosOperacion.ENTIDAD_NO_PROCESABLE;
        }
        return (resultado, resenas);
    }

    public int RegistrarReseña(ResenaDomain resena) //CU11 REALIZAR RESEÑA DE PRODUCTOS
    {
        int resultado = 0;
        Resena resenaTemp = new Resena();
        resenaTemp.IdProducto = resena.IdProducto;
        resenaTemp.IdUsuario = resena.IdUsuario;
        resenaTemp.Resena1 = resena.Resena1;

        try
        {
            _connectionModel.Resenas.Add(resenaTemp);
            int cambios = _connectionModel.SaveChanges();
            if (cambios > 0)
                resultado = CodigosOperacion.EXITO;
            else
                resultado = CodigosOperacion.ENTIDAD_NO_PROCESABLE;
        }
        catch (DbUpdateException)
        {
            resultado = CodigosOperacion.ENTIDAD_NO_PROCESABLE;
        }
        return resultado;
    }


}