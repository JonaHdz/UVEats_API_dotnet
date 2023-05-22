namespace UVEATS_API_DOTNET.Business;
using UVEATS_API_DOTNET.Models;
using System.Globalization;
using UVEATS_API_DOTNET.Domain;
using Microsoft.EntityFrameworkCore;

public class UsuarioProvider
{
    private UveatsContext _connectionModel;

    //Constructor del DataContext
    public UsuarioProvider(UveatsContext connectionModel)
    {
        string Culture = "es-MX";
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Culture);
        _connectionModel = connectionModel;
    }

    public (int, Usuario) ReistrarCliente(UsuarioDomain nuevoUsuario) //CU06 REGISTRAR CLIENTE
    {
        Usuario usuario = new Usuario();
        usuario.Firstname = nuevoUsuario.Firstname;
        usuario.Apellido = nuevoUsuario.Apellido;
        usuario.Contrasena = nuevoUsuario.Contrasena;
        usuario.Correo = nuevoUsuario.Correo;
        usuario.Foto = nuevoUsuario.Foto;
        usuario.Telefono = nuevoUsuario.Telefono;
        usuario.Tipo = Roles.ROL_CLIENTE;
        int resultado = 0;
        try
        {
            _connectionModel.Usuarios.Add(usuario);
            int cambios = _connectionModel.SaveChanges();
            if (cambios == 1)
                resultado = CodigosOperacion.EXITO;
            else
                resultado = CodigosOperacion.SOLICITUD_INCORRECTA;
        }
        catch (DbUpdateException)
        {
            resultado = CodigosOperacion.ENTIDAD_NO_PROCESABLE;
        }
        return (resultado, usuario);
    }

    public int EliminarUsuario(int id)
    {
        int resultado = 0;
        try
        {
            Usuario usuario = _connectionModel.Usuarios.FirstOrDefault();
            if (usuario != null && usuario.IdUsuario == id)
            {
                _connectionModel.Usuarios.Remove(usuario);
                int cambios = _connectionModel.SaveChanges();
                if (cambios > 0)
                    resultado = CodigosOperacion.EXITO;
                else
                    resultado = CodigosOperacion.CONFLICTO;

            }
            else
                resultado = CodigosOperacion.RECURSO_NO_ENCONTRADO;
        }
        catch (DbUpdateException)
        {
            resultado = CodigosOperacion.ENTIDAD_NO_PROCESABLE;
        }
        return resultado;
    }

    public (int, Usuario) ModificarUsuario(UsuarioDomain usuarioTemp) // CU07 MODIFICAR INFORMACION CLIENTE
    {
        int resultado = 0;
        Usuario usuario = new Usuario();
        try
        {
            usuario = _connectionModel.Usuarios.FirstOrDefault(a => a.IdUsuario == usuarioTemp.IdUsuario);
            if (usuario != null && usuario.IdUsuario == usuarioTemp.IdUsuario)
            {
                usuario.Firstname = usuarioTemp.Firstname;
                usuario.Apellido = usuarioTemp.Apellido;
                usuario.Contrasena = usuarioTemp.Contrasena;
                // usuario.Correo = usuarioTemp.Correo;
                usuario.Foto = usuarioTemp.Foto;
                usuario.Telefono = usuarioTemp.Telefono;
                //  usuario.Tipo = usuarioTemp.Tipo;
                int cambios = _connectionModel.SaveChanges();
                if (cambios > 0)
                    resultado = CodigosOperacion.EXITO;
                else
                    resultado = CodigosOperacion.CONFLICTO;
            }
        }
        catch (DbUpdateException)
        {
            resultado = CodigosOperacion.ENTIDAD_NO_PROCESABLE;
        }

        return (resultado, usuario);

    }

    public (int, List<Usuario>) RecuperarClientes()
    {

        int resultado = 0;
        List<Usuario> UsuariosList = new List<Usuario>();
        try
        {
            UsuariosList = _connectionModel.Usuarios.Where(a => a.Tipo == Roles.ROL_CLIENTE).ToList();
            resultado = CodigosOperacion.EXITO;

        }
        catch (DbUpdateException)
        {
            resultado = CodigosOperacion.ERROR_CONEXION;
        }
        return (resultado, UsuariosList);
    }






}
