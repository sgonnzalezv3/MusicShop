using AppMusic.Domain;
using AutoMapper;
using JMusik.Data.Contratos;
using JMusik.Dtos;
using JMusik.WebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppMusic.ApiWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SesionController : ControllerBase
    {
        private IUsuariosRepositorio _usuariosRepositorio;
        private IMapper _mapper;
        private TokenService _tokenService;
        public SesionController(IUsuariosRepositorio usuariosRepositorio, IMapper mapper, TokenService tokenService )
        {
            _usuariosRepositorio = usuariosRepositorio;
            _mapper = mapper;
            _tokenService = tokenService;
        }
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<string>> PostLogin(LoginModelDto usuarioLogin)
        {
            //mapear los datos de envio a un usuario
            var datosLoginUsuario = _mapper.Map<Usuario>(usuarioLogin);
            //checar si todo coincide
            var resultadoValidacion = await _usuariosRepositorio.ValidarDatosLogin(datosLoginUsuario);
            if (!resultadoValidacion.resultado)
            {
                return BadRequest("Usuario/Contraseña Invalidos.");
            }
            //si todo es exitoso, retorna el token
            return _tokenService.GenerarToken(resultadoValidacion.usuario);
        }
    }
}
