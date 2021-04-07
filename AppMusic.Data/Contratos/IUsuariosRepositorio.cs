using AppMusic.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JMusik.Data.Contratos
{
    public interface IUsuariosRepositorio : IRepositorioGenerico<Usuario>
    {
        Task<bool> CambiarContrasena(Usuario usuario);
        Task<bool> CambiarPerfil(Usuario usuario);
        Task<bool> ValidarContrasena(Usuario usuario);

        //tupla para devolver un resultado, y en caso de exito el usuario
        Task<(bool resultado, Usuario usuario)> ValidarDatosLogin(Usuario datosLoginUsuario);
    }
}
