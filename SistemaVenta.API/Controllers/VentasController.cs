using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVenta.BLL.Servicios;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DTO;

namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly IVentaService _ventaService;

        public VentasController(IVentaService ventaService)
        {
            _ventaService=ventaService;
        }

        [HttpPost]
        [Route("Registrar")]
        public async Task<IActionResult> Registrar([FromBody] VentaDTO venta)
        {
            var rsp = new Utilidad.Response<VentaDTO>();

            try
            {
                rsp.status = true;
                rsp.value=await _ventaService.Registrar(venta);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }


        [HttpGet]
        [Route("Historial")]
        public async Task<IActionResult> Historial(string buscarPor,string? numeroVenta,string?fechaInicio,string?fechaFin)
        {
            var rsp = new Utilidad.Response<List<VentaDTO>>();
            numeroVenta=numeroVenta is null ? "" : numeroVenta;
            fechaInicio=fechaInicio is null?"":fechaInicio;
            fechaFin=fechaFin is null?"":fechaFin;

            try
            {
                rsp.status = true;
                rsp.value=await _ventaService.Historial(buscarPor,numeroVenta,fechaInicio,fechaFin);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }

        [HttpGet]
        [Route("Reporte")]
        public async Task<IActionResult> Reporte(string? fechaInicio, string? fechaFin)
        {
            var rsp = new Utilidad.Response<List<ReporteDTO>>();
         

            try
            {
                rsp.status = true;
                rsp.value=await _ventaService.reporte(fechaInicio, fechaFin);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }
    }
}
