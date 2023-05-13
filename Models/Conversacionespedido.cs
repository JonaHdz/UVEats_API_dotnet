using System;
using System.Collections.Generic;

namespace UVEATS_API_DOTNET.Models;

public partial class Conversacionespedido
{
    public int IdConversacionesPedido { get; set; }

    public int? IdPedido { get; set; }

    public string? Conversacion { get; set; }

    public virtual Pedido? IdPedidoNavigation { get; set; }
}
