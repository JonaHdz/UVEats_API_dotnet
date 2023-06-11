using System;
using System.Collections.Generic;

namespace API_PROYECTO.Models;

public partial class Resena
{
    public int IdResena { get; set; }

    public int? IdProducto { get; set; }

    public int? IdUsuario { get; set; }

    public string? Resena1 { get; set; }

    public DateTime? Fecha { get; set; }

    public virtual Producto? IdProductoNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
