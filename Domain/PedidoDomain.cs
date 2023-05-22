namespace UVEATS_API_DOTNET.Domain;

public class PedidoDomain
{
    public int IdPedido { get; set; }

    public decimal? Total { get; set; }

    public string? EstadoPedido { get; set; }

    public int? IdUsuario { get; set; }

    public DateOnly? FechaPedido { get; set; }

    public List<ProductosPedidoDomain> productosPedido = new List<ProductosPedidoDomain>();


}