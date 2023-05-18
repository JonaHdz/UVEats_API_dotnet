/*
/   CLASE PARA COLOCAR TODOS LOS CODIGOS QUE PUEDAN NECESITAR
/   EJ:EXITO,FALLO,EXCEPCION,ERROR,ETC
*/
namespace UVEATS_API_DOTNET.Domain;
public class CodigosOperacion
{
    public static int EXITO = 200;
    public static int RECURSO_NO_ENCONTRADO = 404;
    public static int ERROR_CONEXION = 502;
    public static int ENTIDAD_NO_PROCESABLE = 422;
    public static int SOLICITUD_INCORRECTA = 400; 

//puede ser usada cuando no se puede eliminar un recurso
    public static int CONFLICTO = 409;
}