using AppMusic.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppMusic.Data.Contratos
{
    //Interfaz que es contrato que obliga a usar el repositorio
    public interface IProductosRepositorio
    {
        Task<List<Producto>> ObtenerProductosAsync();
        Task<Producto> ObtenerProductoAsync(int id);
        Task<Producto> Agregar(Producto producto);
        Task<bool> Actualizar(Producto producto);
        Task<bool> Eliminar(int id);

    }
}
