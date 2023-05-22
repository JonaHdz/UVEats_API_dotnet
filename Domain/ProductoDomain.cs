namespace UVEATS_API_DOTNET.Domain;

public class ProductoDomain
{
     public int IdProducto { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public decimal? Precio { get; set; }

    public int? IdCategoria { get; set; }

    public string? Categoria {get; set;}

    public string? EstadoProducto { get; set; }

    public byte[]? FotoProducto { get; set; }
}