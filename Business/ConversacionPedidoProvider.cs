namespace UVEATS_API_DOTNET.Business;
using UVEATS_API_DOTNET.Models;
using System.Globalization;
using UVEATS_API_DOTNET.Domain;
using Microsoft.EntityFrameworkCore;
using API_PROYECTO.Models;

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
          Console.WriteLine("CACENLANDO");
        int resultado = 0;
        try
        {
            Conversacionespedido? conversacion = _connectionModel.Conversacionespedidos.Where(a => a.IdConversacionesPedido == conversacionTemp.IdConversacionesPedido).FirstOrDefault();
          //  Console.WriteLine("Conversacionrecuperada: " + conversacion.Conversacion);
            conversacion.Conversacion = conversacionTemp.Conversacion;
            int cambios = _connectionModel.SaveChanges();
            if (cambios == 1)
                resultado = CodigosOperacion.EXITO;
            else
                resultado = CodigosOperacion.SOLICITUD_INCORRECTA;
        }
        catch (Exception e)
        {
            Console.WriteLine("EX: " + e);
            resultado = CodigosOperacion.ENTIDAD_NO_PROCESABLE;
        }
        return resultado;
    }

    public ConversacionPedidoDomain RecuperaConvesacionPedido(int idPedido)
    {
      
        Conversacionespedido conversacion = _connectionModel.Conversacionespedidos.Where(a => a.IdPedido == idPedido).FirstOrDefault();
        ConversacionPedidoDomain conversacionTemp = new ConversacionPedidoDomain();
        conversacionTemp.IdConversacionesPedido = conversacion.IdConversacionesPedido;
        conversacionTemp.IdPedido = conversacion.IdPedido;
        conversacionTemp.Conversacion = conversacion.Conversacion;
        return conversacionTemp;
        
    }
}