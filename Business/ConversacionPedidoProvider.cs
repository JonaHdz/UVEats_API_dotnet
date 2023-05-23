namespace UVEATS_API_DOTNET.Business;
using UVEATS_API_DOTNET.Models;
using System.Globalization;
using UVEATS_API_DOTNET.Domain;
using Microsoft.EntityFrameworkCore;

public class ConversacionPedidoProvider
{

    private UveatsContext _connectionModel;

    //Constructor del DataContext
    public ConversacionPedidoProvider(UveatsContext connectionModel)
    {
        string Culture = "es-MX";
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Culture);
        _connectionModel = connectionModel;
    }

    public int RegistrarConversacion(ConversacionPedidoDomain conversacionTemp)
    {
        int resultado = 0;
        try
        {
            Conversacionespedido? conversacion = _connectionModel.Conversacionespedidos.Where(a => a.IdPedido == conversacionTemp.IdPedido).FirstOrDefault();
            conversacion.Conversacion += conversacionTemp.Conversacion;
            int cambios = _connectionModel.SaveChanges();
            if (cambios == 1)
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

    //falta metodo para recuperar conversaciones
}