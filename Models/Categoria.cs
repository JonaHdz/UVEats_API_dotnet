using System;
using System.Collections.Generic;

namespace API_PROYECTO.Models;

public partial class Categoria
{
    public int IdCategoria { get; set; }

    public string? Categoria1 { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
