using SistemaVenta.DAL.DBContext;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace SistemaVenta.DAL.Repositorios
{
    public class VentaRepository : GenericRepository<Venta>, IVentaRepository
    {
        private readonly DbventaContext _dbventaContext;

        public VentaRepository(DbventaContext dbContext) : base(dbContext)
        {
            _dbventaContext = dbContext;
        }

        public async Task<Venta> Registrar(Venta modelo)
        {
            Venta ventagenerada=new Venta();

            using(var trasaction = _dbventaContext.Database.BeginTransaction())
            {
                try
                {
                    foreach(DetalleVenta dv in modelo.DetalleVentas){
                        Producto producto_encontrado = _dbventaContext.Productos.Where(p => p.IdProducto== dv.IdProducto).First();
                    }
                    await _dbventaContext.SaveChangesAsync();

                    NumeroDocumento correlativo = _dbventaContext.NumeroDocumentos.First();
                    correlativo.UltimoNumero=correlativo.UltimoNumero+1;
                    correlativo.FechaRegistro=DateTime.Now;

                    _dbventaContext.NumeroDocumentos.Update(correlativo);
                    await _dbventaContext.SaveChangesAsync();

                    int CantidadDigitos = 4;
                    string ceros = string.Concat(Enumerable.Repeat("0", CantidadDigitos));
                    string numeroVenta = ceros+correlativo.UltimoNumero.ToString();
                    //00001
                    numeroVenta=numeroVenta.Substring(numeroVenta.Length-CantidadDigitos, CantidadDigitos);
                    modelo.NumeroDocumento = numeroVenta;

                    await _dbventaContext.AddAsync(modelo);
                    await _dbventaContext.SaveChangesAsync();

                    ventagenerada=modelo;
                    trasaction.Commit();
                }
                catch
                {
                    trasaction.Rollback();//en caso falle vuelva los datos a como era antes
                    throw;
                }
                return ventagenerada;
            }
        }
    }
}