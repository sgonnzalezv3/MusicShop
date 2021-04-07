using AppMusic.Data.Contratos;
using AppMusic.Data.Repositorios;
using AppMusic.Domain;
using JMusik.Data.Contratos;
using JMusik.Data.Repositorios;
using JMusik.WebApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMusic.ApiWeb.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            //agregando el servicio de CORS 
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                        //.AllowCredentials();
                    });
            });

            //implementacion del jwt

        }
        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration )
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            string secretKey = jwtSettings.GetValue<string>("SecretKey");
            int minutes = jwtSettings.GetValue<int>("MinutesToExpiration");
            string issuer = jwtSettings.GetValue<string>("Issuer");
            string audience = jwtSettings.GetValue<string>("Audience");
            var key = Encoding.ASCII.GetBytes(secretKey);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                //debe ser true en produccion, requeire htttps
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(minutes)
                };
            });
        }
        //dependencias implementacion
        public static void ConfigureDependencies(this IServiceCollection services)
        {
            /*
    Cada vez que se solicite un IProductoRepositorio nos va devolver una implementacion
    de ProductosRepositorio
 */
            //Scoped significa que el servicio sera creado y liberado por cada peticion del cliente
            services.AddScoped<IProductosRepositorio, RepositorioProductos>();
            services.AddScoped<IRepositorioGenerico<Perfil>, RepositorioPerfiles>();
            services.AddScoped<IOrdenesRepositorio, RepositorioOrdenes>();
            services.AddScoped<IUsuariosRepositorio, RepositorioUsuarios>();
            //increptacion de los campos IPasswordHasher
            services.AddScoped<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();

            //Singleton significa que solo existira una instancia para todo el ciclo de vida 
            services.AddSingleton<TokenService>();
        }
    }
}
