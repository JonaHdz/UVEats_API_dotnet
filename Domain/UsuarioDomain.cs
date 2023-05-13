namespace UVEATS_API_DOTNET.Domain;

public partial class UsuarioDomain
{
    public int? IdUsuario { get; set; }

    public string? Correo { get; set; }

    public string? Contrasena { get; set; }

    public string? Firstname { get; set; }

    public string? Apellido { get; set; }

    public string? Telefono { get; set; }

    public string? Tipo { get; set; }

    public byte[]? Foto { get; set; }

}
