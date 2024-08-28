using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SistemaVenta.DAL.DBContext;
using SistemaVenta.DAL.Repositorios.Contrato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//importamos los siguiente
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.DAL.Repositorios;
//importamos Utility
using SistemaVenta.Utility;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.BLL.Servicios;

namespace SistemaVenta.IOC
{
    public static class Dependencia
    {
        public static void InyectarDependencias(this IServiceCollection service,IConfiguration configuration)
        {
            service.AddDbContext<DbventaContext>(option =>
            {
                option.UseSqlServer(configuration.GetConnectionString("CadenaSQL"));
            });

            //agregamos las interfaces
            service.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            service.AddScoped<IVentaRepository,VentaRepository>();

            //Inyectamos las dependencias para trabajr con Mapeos de clases

            service.AddAutoMapper(typeof(AutoMapperProfile));

            //Agregamos las interfaces con sus implementaciones de servicios de cada modelo de la capa BLL
            service.AddScoped<IRolService,RolService>();
            service.AddScoped<IUsuarioService,UsuarioService>();
            service.AddScoped<ICategoriaService,CategoriaService>();
            service.AddScoped<IProductoService,ProductoService>();
            service.AddScoped<IVentaService,VentaService>();
            service.AddScoped<IDashBoardService, DashBoardService>();
            service.AddScoped<IMenuService, MenuService>();

        }
    }
}
