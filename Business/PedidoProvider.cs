namespace UVEATS_API_DOTNET.Business;
using UVEATS_API_DOTNET.Models;
using System.Globalization;
using UVEATS_API_DOTNET.Domain;
using Microsoft.EntityFrameworkCore;

public class PedidoProvider
{

    private UveatsContext _connectionModel;

    //Constructor del DataContext
    public PedidoProvider(UveatsContext connectionModel)
    {
        string Culture = "es-MX";
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Culture);
        _connectionModel = connectionModel;
    }

    public int RegistrarPedido(PedidoDomain pedido)
    {
        //1) registrar pedido 2)registrar conversacion  3)registrar productos pedido
        int resultado = 0;
        try
        {
            Pedido pedidoTemp = new Pedido();
            pedidoTemp.Total = pedido.Total;
            pedidoTemp.EstadoPedido = EstadosPedido.PEDIDO_ACTIVO;
            pedidoTemp.IdUsuario = pedido.IdUsuario;
            pedidoTemp.FechaPedido = pedido.FechaPedido;
            _connectionModel.Pedidos.Add(pedidoTemp);
            int cambioPedido = _connectionModel.SaveChanges();

            List<Productospedido> productosList = new List<Productospedido>();
            foreach (var util in pedido.productos)
            {
                Productospedido productoPedido = new Productospedido();
                productoPedido.IdPedido = pedidoTemp.IdPedido;
                productoPedido.IdProducto = util.IdProducto;
                productoPedido.Cantidad = util.Cantidad;
                productoPedido.Subtotal = util.Subtotal;
                productoPedido.EstadoProducto = EstadosPedido.PEDIDO_ACTIVO;
                productosList.Add(productoPedido);
            }
            _connectionModel.AddRange(productosList);
            int cambioProductos = _connectionModel.SaveChanges();

            Conversacionespedido conversacionespedido = new Conversacionespedido();
            conversacionespedido.IdPedido = pedidoTemp.IdPedido;
            int cambioConversacion = _connectionModel.SaveChanges();

            if (cambioConversacion == 1 && cambioPedido == 1 && cambioProductos == 1)
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

    public void RecuperarHistorialPedidos(int idUsuario)
    {

    }



}
