using AutoMapper;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.BLL.Servicios
{
    public class DashBoardService:IDashBoardService
    {
        private readonly IVentaRepository _ventaRepository;
        private readonly IGenericRepository<Producto> _productoRepository;
        private readonly IMapper _mapper;

        public DashBoardService(IVentaRepository ventaRepository, IGenericRepository<Producto> productoRepository, IMapper mapper)
        {
            _ventaRepository=ventaRepository;
            _productoRepository=productoRepository;
            _mapper=mapper;
        }

        private IQueryable<Venta> retornarVentas(IQueryable<Venta> tablaVenta,int restarCantidadDias)
        {
            DateTime? ultimaFecha=tablaVenta.OrderByDescending(v=>v.FechaRegistro).Select(v=>v.FechaRegistro).First();

            ultimaFecha=ultimaFecha.Value.AddDays(restarCantidadDias);

            return tablaVenta.Where(v=>v.FechaRegistro.Value.Date>=ultimaFecha.Value.Date);
        }

        private async Task<int> TotalVentasUltimaSemana()
        {
            int total = 0;
            IQueryable<Venta> _ventaQuery = await _ventaRepository.Consultar();

            if(_ventaQuery.Count() > 0)
            {
                var tablaVenta = retornarVentas(_ventaQuery, -7);
                total = tablaVenta.Count();
            }
            return total;
        }

        private async Task<string> TotalIngresosUltimaSemana()
        {
            decimal resultado = 0;
            IQueryable<Venta> _ventaQuery=await _ventaRepository.Consultar();

            if (_ventaQuery.Count()>0)
            {
                var tablaVenta = retornarVentas(_ventaQuery, -7);

                resultado=tablaVenta.Select(v => v.Total).Sum(v => v.Value);
            }
            return Convert.ToString(resultado,new CultureInfo("es-PE"));
        }

        private async Task<int> TotalProductos()
        {
            IQueryable<Producto> _productoQuery=await _productoRepository.Consultar();  

            int total=_productoQuery.Count();

            return total;
        }

        private async Task<Dictionary<string,int>> VentasUltimaSemana()
        {
            Dictionary<string,int> result=new Dictionary<string, int>();
            IQueryable<Venta> _ventaQuey=await _ventaRepository.Consultar();

            if (_ventaQuey.Count()>0)
            {
                var tablaventa=retornarVentas(_ventaQuey, -7);

                result=tablaventa
                    .GroupBy(v => v.FechaRegistro.Value.Date).OrderBy(g => g.Key)
                    .Select(dv => new { fecha = dv.Key.ToString("dd/MM/yyyy"), total = dv.Count() })
                    .ToDictionary(keySelector: f => f.fecha, elementSelector: r => r.total);
            }
            return result;
        }


        //RECIEN AHORA IMPLEMENTAMOS INTERFAZ

        public async Task<DashBoardDTO> Resumen()
        {
            DashBoardDTO dashBoardDTO = new DashBoardDTO();

            try
            {
                dashBoardDTO.TotalVentas=await TotalVentasUltimaSemana();
                dashBoardDTO.TotalIngresos=await TotalIngresosUltimaSemana();
                dashBoardDTO.TotalProductos=await TotalProductos();

                List<VentaSemanaDTO> listaVentaSemana=new List<VentaSemanaDTO>();

                foreach(KeyValuePair<string,int> item in await VentasUltimaSemana())
                {
                    listaVentaSemana.Add(new VentaSemanaDTO()
                    {
                        Fecha=item.Key,
                        Total=item.Value,
                    });
                }
                dashBoardDTO.VentasUltimaSemana=listaVentaSemana;
            }
            catch
            {

            }
            return dashBoardDTO;
        }


    }
}
