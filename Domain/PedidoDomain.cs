namespace UVEATS_API_DOTNET.Domain;

public class PedidoDomain
{
    public decimal? Total { get; set; }
    public int IdPedido { get; set; }

    

    public string? EstadoPedido { get; set; }

    public int? IdUsuario { get; set; }

    public DateTime? FechaPedido { get; set; }

    public List<ProductosPedidoDomain>  ? ProductosPedido {get; set;}

    

}