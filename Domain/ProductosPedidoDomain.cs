namespace UVEATS_API_DOTNET.Domain;

public class ProductosPedidoDomain
{
    public int? IdProductoPedido { get; set; }

    public int? IdPedido { get; set; }

    public int? IdProducto { get; set; }

    public int? Cantidad { get; set; }

    public decimal? Subtotal { get; set; }

    public string? EstadoProducto { get; set; }

    public string? NombreProducto {get; set;}

}