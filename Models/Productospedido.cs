using System;
using System.Collections.Generic;

namespace API_PROYECTO.Models;

public partial class Productospedido
{
    public int IdProductoPedido { get; set; }

    public int? IdPedido { get; set; }

    public int? IdProducto { get; set; }

    public int? Cantidad { get; set; }

    public decimal? Subtotal { get; set; }

    public string? EstadoProducto { get; set; }

    public virtual Pedido? IdPedidoNavigation { get; set; }

    public virtual Producto? IdProductoNavigation { get; set; }
}
