namespace UVEATS_API_DOTNET.Business;
using UVEATS_API_DOTNET.Models;
using System.Globalization;
using UVEATS_API_DOTNET.Domain;

public class LoginProvider {

    private UveatsContext _connectionModel;
    
    //Constructor del DataContext
    public LoginProvider(UveatsContext connectionModel)
    {
        string Culture = "es-MX";
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Culture);
        _connectionModel = connectionModel;
    }

    //METODOS DEL DAO
    public List<Usuario> ObtenerUsuarios(){
        List<Usuario> usuarios = _connectionModel.Usuarios.ToList();
        return usuarios;
    }

    public (int ,Usuario) IniciarSesion(Domain.LoginDomain credenciales)
    {
        int operacion = 0;
         Usuario usuario = _connectionModel.Usuarios.Where(a => a.Correo.Equals(credenciales.correo) && a.Contrasena.Equals(credenciales.contrasena)).FirstOrDefault();
        //Usuario usuario = _connectionModel.Usuarios.FirstOrDefault();
        if(usuario != null)
            operacion = CodigosOperacion.EXITO;
        else
            operacion = CodigosOperacion.RECURSO_NO_ENCONTRADO;

        return(operacion,usuario);
    }
}