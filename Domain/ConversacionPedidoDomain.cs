namespace UVEATS_API_DOTNET.Domain;

public class ConversacionPedidoDomain
{
    public int IdConversacionesPedido { get; set; }

    public int? IdPedido { get; set; }//llave foranea,solo se usa al crear en la tabla

    public string? Conversacion { get; set; }
}