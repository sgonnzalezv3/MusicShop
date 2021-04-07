using AppMusic.Data.Contratos;
using AppMusic.Domain;
using AppMusic.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppMusic.ApiWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly IProductosRepositorio _repositorio;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductosController> _logger;
        public ProductosController(IProductosRepositorio repositorio, IMapper mapper, ILogger<ProductosController> logger ) 
        {
            _logger = logger;
            _repositorio = repositorio;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> Get()
        {
            try
            {
                var productos =  await _repositorio.ObtenerProductosAsync();
                return _mapper.Map<List<ProductoDto>>(productos);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error en {nameof(Get)}: ${e.Message}"); 
                return BadRequest();
            };
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoDto>> Get(int id)
        {
            var producto = await _repositorio.ObtenerProductoAsync(id);
            if(producto == null)
            {
                return NotFound();
            }
            return _mapper.Map<ProductoDto>(producto);
        }
        [HttpPost]
        public async Task<ActionResult<ProductoDto>> Post(ProductoDto productoDto)
        {
            try
            {
                var producto = _mapper.Map<Producto>(productoDto);
                var nuevoProducto =  await _repositorio.Agregar(producto);
                if(nuevoProducto == null)
                {
                    return BadRequest();
                }
                var nuevoProductoDto = _mapper.Map<ProductoDto>(nuevoProducto);
                return CreatedAtAction(nameof(Post), new { id = nuevoProductoDto.Id }, nuevoProductoDto);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error en {nameof(Post)}: ${e.Message}");

                return BadRequest();
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductoDto>> Put(int id ,[FromBody]ProductoDto productoDto )
        {
            if (productoDto == null)
                return NotFound();

            var producto = _mapper.Map<Producto>(productoDto);
            var result = await _repositorio.Actualizar(producto);
            if (!result)
                return BadRequest();
            return productoDto;
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var resultado =await _repositorio.Eliminar(id);
                if (!resultado)
                {
                    return BadRequest();
                }
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError($"Error en {nameof(Delete)}: ${e.Message}");


                return BadRequest();
            }
        }
    }
}
