namespace UVEATS_API_DOTNET.Business;
using UVEATS_API_DOTNET.Models;
using System.Globalization;
using UVEATS_API_DOTNET.Domain;
using Microsoft.EntityFrameworkCore;
using API_PROYECTO.Models;

public class EmpleadoProvider
{

    private UveatsContext _connectionModel;

    //Constructor del DataContext
    public EmpleadoProvider(UveatsContext connectionModel)
    {
        string Culture = "es-MX";
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Culture);
        _connectionModel = connectionModel;
    }

    public (int, Usuario) ReistrarEmpleado(UsuarioDomain nuevoUsuario) //CU12 AGREGAR EMPLEADO
    {
        Usuario usuario = new Usuario();
        usuario.Nombre = nuevoUsuario.Nombre;
        usuario.Apellido = nuevoUsuario.Apellido;
        usuario.Contrasena = nuevoUsuario.Contrasena;
        usuario.Correo = nuevoUsuario.Correo;
        usuario.Foto = nuevoUsuario.FotoBytes;
        usuario.Telefono = nuevoUsuario.Telefono;
        usuario.Tipo = Roles.ROL_EMPLEADO;
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
        catch (Exception)
        {
            resultado = CodigosOperacion.ENTIDAD_NO_PROCESABLE;
        }
        return (resultado, usuario);
    }

    public int EliminarEmpleado(int idEmpleado) //CU14 ELIMINAR EMPLEADO
    {
        int resultado = 0;
        try
        {
            Usuario? empleado = _connectionModel.Usuarios.Where(a => a.IdUsuario == idEmpleado).FirstOrDefault();
            _connectionModel.Usuarios.Remove(empleado);
            _connectionModel.SaveChanges();
            resultado = CodigosOperacion.EXITO;
        }
        catch (DbUpdateException)
        {
            resultado = CodigosOperacion.ENTIDAD_NO_PROCESABLE;
        }
        return resultado;
    }

    public (int, Usuario) ModificarEmpleado(UsuarioDomain usuarioTemp) //CU13 MODIFICAR EMPLEADO
    {
        int resultado = 0;
        Usuario? empleadoSeleccionado = new Usuario();
        try
        {
            empleadoSeleccionado = _connectionModel.Usuarios.FirstOrDefault(a => a.IdUsuario == usuarioTemp.IdUsuario);
            if (empleadoSeleccionado != null && empleadoSeleccionado.IdUsuario == usuarioTemp.IdUsuario)
            {
                empleadoSeleccionado.Nombre = usuarioTemp.Nombre;
                empleadoSeleccionado.Apellido = usuarioTemp.Apellido;
                empleadoSeleccionado.Contrasena = usuarioTemp.Contrasena;
                empleadoSeleccionado.Correo = usuarioTemp.Correo;
               empleadoSeleccionado.Foto = usuarioTemp.FotoBytes;
                empleadoSeleccionado.Telefono = usuarioTemp.Telefono;
                int cambios = _connectionModel.SaveChanges();
                if (cambios == 1)
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
        return (resultado, empleadoSeleccionado);
    }


    public (int, List<UsuarioDomain>) RecuperarEmpelados()
    {

        int resultado = 0;
        List<Usuario> UsuariosList = new List<Usuario>();
        List<UsuarioDomain> empleados = new List<UsuarioDomain>();
        try
        {
            UsuariosList = _connectionModel.Usuarios.Where(a => a.Tipo.Equals( Roles.ROL_EMPLEADO) || a.Tipo.Equals(Roles.ROL_ADMIN)).ToList();
            
            foreach(var util in UsuariosList)
            {
                UsuarioDomain nuevoEmpleado = new UsuarioDomain();
                nuevoEmpleado.Nombre = util.Nombre;
                nuevoEmpleado.Apellido = util.Apellido;
                nuevoEmpleado.Contrasena = util.Contrasena;
                nuevoEmpleado.Tipo = util.Tipo;
                nuevoEmpleado.Correo = util.Correo;
                nuevoEmpleado.Telefono = util.Telefono;
                nuevoEmpleado.IdUsuario = util.IdUsuario;
                if(util.Foto != null)
                nuevoEmpleado.Foto = Convert.ToBase64String(util.Foto);
                empleados.Add(nuevoEmpleado);
            }
            resultado = CodigosOperacion.EXITO;

        }
        catch (DbUpdateException)
        {
            resultado = CodigosOperacion.ERROR_CONEXION;
        }
        return (resultado, empleados);
    }


}