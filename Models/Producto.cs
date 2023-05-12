using System;
using System.Collections.Generic;

namespace API_PROYECTO.Models;

public partial class Producto
{
    public int IdProducto { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public decimal? Precio { get; set; }

    public int? IdCategoria { get; set; }

    public string? EstadoProducto { get; set; }

    public byte[]? FotoProducto { get; set; }

    public virtual Categoria? IdCategoriaNavigation { get; set; }

    public virtual ICollection<Productospedido> Productospedidos { get; set; } = new List<Productospedido>();

    public virtual ICollection<Resena> Resenas { get; set; } = new List<Resena>();
}
