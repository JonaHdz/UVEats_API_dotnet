using System;
using System.Collections.Generic;

namespace UVEATS_API_DOTNET.Models;

public partial class Pedido
{
    public int IdPedido { get; set; }

    public decimal? Total { get; set; }

    public string? EstadoPedido { get; set; }

    public int? IdUsuario { get; set; }

    public DateOnly? FechaPedido { get; set; }

    public virtual ICollection<Conversacionespedido> Conversacionespedidos { get; set; } = new List<Conversacionespedido>();

    public virtual Usuario? IdUsuarioNavigation { get; set; }

    public virtual ICollection<Productospedido> Productospedidos { get; set; } = new List<Productospedido>();
}
