namespace UVEATS_API_DOTNET.Business;
using UVEATS_API_DOTNET.Models;
using System.Globalization;
using UVEATS_API_DOTNET.Domain;
using Microsoft.EntityFrameworkCore;
using API_PROYECTO.Models;

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

    public int CancelarProductoPedido(int idProductoPedido)
    {
        int resultado = 0;
        try{
            Productospedido producto = _connectionModel.Productospedidos.Where(a => a.IdProductoPedido == idProductoPedido).FirstOrDefault();
            producto.EstadoProducto = EstadosPedido.PEDIDO_CANCELADO;
            int cambio = _connectionModel.SaveChanges();
            if(cambio ==1)
            resultado = CodigosOperacion.EXITO;
            else
            resultado = CodigosOperacion.ENTIDAD_NO_PROCESABLE;
            Console.WriteLine("cambio: " + cambio);
        }catch(Exception)
        {
            resultado = CodigosOperacion.ERROR_CONEXION;
        }
        return resultado;
    }

    

        public int CancelarPedido(int idPedido)
    {
        int resultado = 0;
        try{
            Pedido pedido = _connectionModel.Pedidos.Where(a => a.IdPedido == idPedido).FirstOrDefault();
            pedido.EstadoPedido = EstadosPedido.PEDIDO_CANCELADO;
            int cambio = _connectionModel.SaveChanges();
            if(cambio ==1)
            resultado = CodigosOperacion.EXITO;
            else
            resultado = CodigosOperacion.ENTIDAD_NO_PROCESABLE;
        }catch(Exception)
        {
            resultado = CodigosOperacion.ERROR_CONEXION;
        }
        return resultado;
    }

            public int RecogerPedido(int idPedido)
    {
        int resultado = 0;
        try{
            Pedido pedido = _connectionModel.Pedidos.Where(a => a.IdPedido == idPedido).FirstOrDefault();
            pedido.EstadoPedido = EstadosPedido.PEDIDO_ENTREGADO;
            int cambio = _connectionModel.SaveChanges();
            if(cambio ==1)
            resultado = CodigosOperacion.EXITO;
            else
            resultado = CodigosOperacion.ENTIDAD_NO_PROCESABLE;
        }catch(Exception)
        {
            resultado = CodigosOperacion.ERROR_CONEXION;
        }
        return resultado;
    }

    public int RegistrarPedido(PedidoDomain pedido)
    {
        //1) registrar pedido 2)registrar conversacion  3)registrar productos pedido
        int resultado = 0;
        try
        {
            Console.WriteLine("INICIO: " );
            Pedido pedidoTemp = new Pedido();
            pedidoTemp.Total = pedido.Total;
            pedidoTemp.EstadoPedido = "Activo";
            pedidoTemp.IdUsuario = pedido.IdUsuario;
           // pedidoTemp.FechaPedido = DateOnly.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
           pedidoTemp.FechaPedido = DateOnly.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            _connectionModel.Pedidos.Add(pedidoTemp);
            int cambioPedido = _connectionModel.SaveChanges();
            List<Productospedido> productosList = new List<Productospedido>();
            foreach (var util in pedido.ProductosPedido)
            {
                Console.WriteLine("ID DE PRODUCTO: " + util.Subtotal);
                Productospedido productoPedido = new Productospedido();
                productoPedido.IdPedido = pedidoTemp.IdPedido;
                productoPedido.IdProducto = util.IdProducto;
                productoPedido.Cantidad = util.Cantidad;
                productoPedido.Subtotal = util.Subtotal;
                productoPedido.EstadoProducto = EstadosPedido.PEDIDO_ACTIVO;
                productosList.Add(productoPedido);
            }
            Console.WriteLine("ELEMENTOS DE LSITA: " + productosList.Count);
            _connectionModel.Productospedidos.AddRange(productosList);
            int cambioProductos = _connectionModel.SaveChanges();
            Conversacionespedido conversacionespedido = new Conversacionespedido();
            conversacionespedido.IdConversacionesPedido = pedidoTemp.IdPedido;
            conversacionespedido.IdPedido = pedidoTemp.IdPedido;
            conversacionespedido.Conversacion = "";
            _connectionModel.Conversacionespedidos.Add(conversacionespedido);
            Console.WriteLine("cambioPRodcutos "+ cambioProductos);
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


     public (int, List<PedidoDomain>) RecuperarHistorialCliente(int idUsuario)
    {
        List<PedidoDomain> pedidosActivos = new List<PedidoDomain>();
        int resultado = 0;
        try
        {

            var pedidos = _connectionModel.Pedidos.Where(a => a.IdUsuario == idUsuario).ToList();

            foreach (var util in pedidos)
            {
                PedidoDomain pedidoTemp = new PedidoDomain();
                pedidoTemp.IdPedido = util.IdPedido;
                pedidoTemp.Total = util.Total;
                pedidoTemp.EstadoPedido = util.EstadoPedido;
                pedidoTemp.IdUsuario = util.IdUsuario;
                string ?fecha = util.FechaPedido.ToString();
                pedidoTemp.FechaPedido = DateTime.Parse(fecha);
                pedidoTemp.ProductosPedido = new List<ProductosPedidoDomain>();
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
                        ProductosPedidoDomain productoPedidoTemp = new ProductosPedidoDomain();
                        productoPedidoTemp.IdProductoPedido = util.Productospedido.IdProductoPedido;
                        productoPedidoTemp.IdPedido = util.Productospedido.IdPedido;
                        productoPedidoTemp.Cantidad = util.Productospedido.Cantidad;
                        productoPedidoTemp.IdProducto = util.Productospedido.IdProducto;
                        productoPedidoTemp.Subtotal = util.Productospedido.Subtotal;
                        productoPedidoTemp.EstadoProducto = util.Productospedido.EstadoProducto;
                        productoPedidoTemp.NombreProducto = util.Producto.Nombre;
                        pedidosActivos[i].ProductosPedido.Add(productoPedidoTemp);
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

    //recupera los pedidos del dia para que el cliente pueda mandar mensajes
    public (int, List<PedidoDomain>) RecuperarPedidosCliente(int idUsuario)
    {
        List<PedidoDomain> pedidosActivos = new List<PedidoDomain>();
        int resultado = 0;
        try
        {
            DateOnly hoy = DateOnly.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
            Console.WriteLine("FECHA: " + hoy);
            var pedidos = _connectionModel.Pedidos.Where(a => a.FechaPedido == hoy && a.IdUsuario == idUsuario).ToList();

            foreach (var util in pedidos)
            {
                PedidoDomain pedidoTemp = new PedidoDomain();
                pedidoTemp.IdPedido = util.IdPedido;
                pedidoTemp.Total = util.Total;
                pedidoTemp.EstadoPedido = util.EstadoPedido;
                pedidoTemp.IdUsuario = util.IdUsuario;
                string ?fecha = util.FechaPedido.ToString();
                pedidoTemp.FechaPedido = DateTime.Parse(fecha);
                pedidoTemp.ProductosPedido = new List<ProductosPedidoDomain>();
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
                        ProductosPedidoDomain productoPedidoTemp = new ProductosPedidoDomain();
                        productoPedidoTemp.IdProductoPedido = util.Productospedido.IdProductoPedido;
                        productoPedidoTemp.IdPedido = util.Productospedido.IdPedido;
                        productoPedidoTemp.Cantidad = util.Productospedido.Cantidad;
                        productoPedidoTemp.IdProducto = util.Productospedido.IdProducto;
                        productoPedidoTemp.Subtotal = util.Productospedido.Subtotal;
                        productoPedidoTemp.EstadoProducto = util.Productospedido.EstadoProducto;
                        productoPedidoTemp.NombreProducto = util.Producto.Nombre;
                        pedidosActivos[i].ProductosPedido.Add(productoPedidoTemp);
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
            var pedidos = _connectionModel.Pedidos.Where(a => a.EstadoPedido == "Activo").ToList();

            foreach (var util in pedidos)
            {
                
                PedidoDomain pedidoTemp = new PedidoDomain();
                pedidoTemp.IdPedido = util.IdPedido;
                pedidoTemp.Total = util.Total;
                pedidoTemp.EstadoPedido = util.EstadoPedido;
                pedidoTemp.IdUsuario = util.IdUsuario;
               
                string? fecha = util.FechaPedido.ToString();
                pedidoTemp.FechaPedido = DateTime.Parse(fecha);
                pedidoTemp.ProductosPedido = new List<ProductosPedidoDomain>();
                pedidosActivos.Add(pedidoTemp);
            }
            for(int i =0; i<pedidos.Count;i++)
            {
                Usuario usuarioTemp = _connectionModel.Usuarios.Where(a => a.IdUsuario == pedidosActivos[i].IdUsuario).FirstOrDefault();
                pedidosActivos[i].NombreUsuario = usuarioTemp.Nombre + " " + usuarioTemp.Apellido;
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
                        ProductosPedidoDomain productoPedidoTemp = new ProductosPedidoDomain();
                        productoPedidoTemp.IdProductoPedido = util.Productospedido.IdProductoPedido;
                        productoPedidoTemp.IdPedido = util.Productospedido.IdPedido;
                        productoPedidoTemp.Cantidad = util.Productospedido.Cantidad;
                        productoPedidoTemp.IdProducto = util.Productospedido.IdProducto;
                        productoPedidoTemp.Subtotal = util.Productospedido.Subtotal;
                        productoPedidoTemp.EstadoProducto = util.Productospedido.EstadoProducto;
                        productoPedidoTemp.NombreProducto = util.Producto.Nombre;
                        pedidosActivos[i].ProductosPedido.Add(productoPedidoTemp);
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
