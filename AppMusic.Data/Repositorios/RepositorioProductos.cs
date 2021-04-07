using AppMusic.ApiWeb;
using AppMusic.Data.Contratos;
using AppMusic.Domain;
using AppMusic.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppMusic.Data.Repositorios
{
    public class RepositorioProductos : IProductosRepositorio
    {
        private readonly TiendaDbContext _context;
        private readonly ILogger<RepositorioProductos> _logger;
        public RepositorioProductos(TiendaDbContext context, ILogger<RepositorioProductos> logger)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<bool> Actualizar(Producto producto)
        {
            /*Obtener la referencia al producto*/
            var productoBd = await ObtenerProductoAsync(producto.Id);
            productoBd.Nombre = producto.Nombre;
            productoBd.Precio = producto.Precio;


            //_context.Productos.Attach(producto);
            //_context.Entry(producto).State = EntityState.Modified;
            try
            {
                return await _context.SaveChangesAsync() > 0 ? true : false;
            }
            catch (Exception e)
            {

                _logger.LogError($"Error en {nameof(Actualizar)}:{e.Message}");
            }
            return false;
        }

        public async Task<Producto> Agregar(Producto producto)
        {
            //como vamos a usar dto no va estar ni el status ni la fecha del producto
            //para eso se lo asignamos desde el server
            producto.Estatus = EstatusProducto.Activo;
            producto.FechaRegistro = DateTime.UtcNow;

            _context.Productos.Add(producto);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Error en {nameof(Agregar)}:{e.Message}");
                return null;
            }
            return producto;
        }

        public async Task<bool> Eliminar(int id)
        {
            var producto = await _context.Productos.SingleOrDefaultAsync(x => x.Id == id);
            producto.Estatus = EstatusProducto.Inactivo;
            _context.Entry(producto).State = EntityState.Modified;
            try
            {
                return (await _context.SaveChangesAsync() > 0 ? true : false);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error en {nameof(Eliminar)}:{e.Message}");
                ;
            }
            return false;
        }

        public async  Task<Producto> ObtenerProductoAsync(int id)
        {
            return await _context.Productos.SingleOrDefaultAsync(x => x.Id == id && x.Estatus == EstatusProducto.Activo);
        }

        public async Task<List<Producto>> ObtenerProductosAsync()
        {
            return await _context.Productos
                .Where(x=> x.Estatus == EstatusProducto.Activo)
                .OrderBy(x => x.Nombre).ToListAsync();
        }
    }
}
