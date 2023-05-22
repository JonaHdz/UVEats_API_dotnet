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
            foreach (var util in pedido.productosPedido)
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

    public (int, List<PedidoDomain>) RecuperarPedidosCliente(int idUsuario)
    {
        List<PedidoDomain> pedidosActivos = new List<PedidoDomain>();
        int resultado = 0;
        try
        {
            var pedidos = _connectionModel.Pedidos.Where(a => a.IdUsuario == idUsuario && a.EstadoPedido == EstadosPedido.PEDIDO_ACTIVO).ToList();

            foreach (var util in pedidos)
            {
                PedidoDomain pedidoTemp = new PedidoDomain();
                pedidoTemp.IdPedido = util.IdPedido;
                pedidoTemp.Total = util.Total;
                pedidoTemp.EstadoPedido = util.EstadoPedido;
                pedidoTemp.IdUsuario = util.IdUsuario;
                pedidoTemp.FechaPedido = util.FechaPedido;
                pedidosActivos.Add(pedidoTemp);
            }
            var productosPedido = (from productopedidoQ in _connectionModel.Productospedidos
                                   join productoQ in _connectionModel.Productos
                                   on productopedidoQ.IdProducto equals productoQ.IdProducto
                                   select new
                                   {
                                       Producto = productoQ,
                                       Productospedido = productopedidoQ
                                   }).ToList();
            for (int i = 0; i < pedidosActivos.Count(); i++)
            {
                foreach (var util in productosPedido)
                {
                    if (util.Productospedido.IdPedido == pedidosActivos[i].IdPedido)
                    {
                        ProductosPedidoDomain  productoPedidoTemp = new ProductosPedidoDomain();
                        productoPedidoTemp.IdProductoPedido = util.Productospedido.IdProductoPedido;
                        productoPedidoTemp.IdPedido = util.Productospedido.IdPedido;
                        productoPedidoTemp.Cantidad = util.Productospedido.Cantidad;
                        productoPedidoTemp.Subtotal = util.Productospedido.Subtotal;
                        productoPedidoTemp.EstadoProducto = util.Productospedido.EstadoProducto;
                        productoPedidoTemp.NombreProdcuto = util.Producto.Nombre;
                        pedidosActivos[i].productosPedido.Add(productoPedidoTemp);
                    }
                }
            }

            resultado = CodigosOperacion.EXITO;

        }
        catch (DbUpdateException)
        {
            resultado = CodigosOperacion.ENTIDAD_NO_PROCESABLE;
        }

        return (resultado, pedidosActivos);
    }

        public (int, List<PedidoDomain>) RecuperarPedidosEmpleado()
    {
        List<PedidoDomain> pedidosActivos = new List<PedidoDomain>();
        int resultado = 0;
        try
        {
            var pedidos = _connectionModel.Pedidos.Where(a => a.EstadoPedido == EstadosPedido.PEDIDO_ACTIVO).ToList();

            foreach (var util in pedidos)
            {
                PedidoDomain pedidoTemp = new PedidoDomain();
                pedidoTemp.IdPedido = util.IdPedido;
                pedidoTemp.Total = util.Total;
                pedidoTemp.EstadoPedido = util.EstadoPedido;
                pedidoTemp.IdUsuario = util.IdUsuario;
                pedidoTemp.FechaPedido = util.FechaPedido;
                pedidosActivos.Add(pedidoTemp);
            }
            var productosPedido = (from productopedidoQ in _connectionModel.Productospedidos
                                   join productoQ in _connectionModel.Productos
                                   on productopedidoQ.IdProducto equals productoQ.IdProducto
                                   select new
                                   {
                                       Producto = productoQ,
                                       Productospedido = productopedidoQ
                                   }).ToList();
            for (int i = 0; i < pedidosActivos.Count(); i++)
            {
                foreach (var util in productosPedido)
                {
                    if (util.Productospedido.IdPedido == pedidosActivos[i].IdPedido)
                    {
                        ProductosPedidoDomain  productoPedidoTemp = new ProductosPedidoDomain();
                        productoPedidoTemp.IdProductoPedido = util.Productospedido.IdProductoPedido;
                        productoPedidoTemp.IdPedido = util.Productospedido.IdPedido;
                        productoPedidoTemp.Cantidad = util.Productospedido.Cantidad;
                        productoPedidoTemp.Subtotal = util.Productospedido.Subtotal;
                        productoPedidoTemp.EstadoProducto = util.Productospedido.EstadoProducto;
                        productoPedidoTemp.NombreProdcuto = util.Producto.Nombre;
                        pedidosActivos[i].productosPedido.Add(productoPedidoTemp);
                    }
                }
            }

            resultado = CodigosOperacion.EXITO;

        }
        catch (DbUpdateException)
        {
            resultado = CodigosOperacion.ENTIDAD_NO_PROCESABLE;
        }

        return (resultado, pedidosActivos);
    }



}
