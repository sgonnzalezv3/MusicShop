using AppMusic.Domain;
using AppMusic.Dtos;
using AutoMapper;
using JMusik.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppMusic.ApiWeb.Profiles
{
    public class AppMusicProfile : Profile
    {
        public AppMusicProfile()
        {
            CreateMap<Producto, ProductoDto>().ReverseMap();
            CreateMap<Perfil, PerfilDto>().ReverseMap();
            CreateMap<Orden, OrdenDto>()
                .ForMember(x => x.Usuario, p => p.MapFrom(y => y.Usuario.Username))
                .ReverseMap()
                .ForMember(u => u.Usuario, p => p.Ignore());

            CreateMap<DetalleOrden, DetalleOrdenDto>()
                .ForMember(x => x.Producto, p => p.MapFrom(y => y.Producto.Nombre))
                .ReverseMap()
                .ForMember(u => u.Producto, p => p.Ignore());

            CreateMap<Usuario, UsuarioRegistroDto>()
            .ForMember(u => u.Perfil, p => p.MapFrom(m => m.Perfil.Nombre))
            .ReverseMap()
            .ForMember(u => u.Perfil, p => p.Ignore());

            CreateMap<Usuario, UsuarioActualizacionDto>()
                .ReverseMap();

            CreateMap<Usuario, UsuarioListaDto>()
                .ForMember(u => u.Perfil, p => p.MapFrom(m => m.Perfil.Nombre))
                .ForMember(u => u.NombreCompleto, p => p.MapFrom(m => string.Format("{0} {1}",
                        m.Nombre, m.Apellidos)))
                .ReverseMap();

            CreateMap<Usuario, LoginModelDto>().ReverseMap();

            CreateMap<Usuario, PerfilUsuarioDto>().ReverseMap();

        }
    }
}
