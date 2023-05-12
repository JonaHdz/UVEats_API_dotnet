using System;
using System.Collections.Generic;

namespace API_PROYECTO.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string? Correo { get; set; }

    public string? Contrasena { get; set; }

    public string? Firstname { get; set; }

    public string? Apellido { get; set; }

    public string? Telefono { get; set; }

    public string? Tipo { get; set; }

    public byte[]? Foto { get; set; }

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    public virtual ICollection<Resena> Resenas { get; set; } = new List<Resena>();
}
