using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SistemaVenta.DAL.DBContext;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.Model;

namespace SistemaVenta.DAL.Repositorios
{
    //GenericRepository<TModelo> es una clase genérica que implementa una interfaz genérica IGenericRepository<TModelo>,
    //y está diseñada para trabajar con cualquier clase (TModelo).
    public class GenericRepository<TModelo> : IGenericRepository<TModelo> where TModelo : class
    {
        //Primero declaramos nuestro DBContext para interactura con la base de datos
        private readonly DbventaContext _dbContext; //readonly es como asignar una constante solo una vez puede ser asignado y no puede cambiar

        //creamos un contructor
        public GenericRepository(DbventaContext dbContext)
        {
            _dbContext = dbContext;
        }

        //Luego al final las implementaciones de la interfaz
        public async Task<TModelo> Obtener(Expression<Func<TModelo, bool>> filtro)
        {
            try
            {
                TModelo modelo = await _dbContext.Set<TModelo>().FirstOrDefaultAsync(filtro); 
                return modelo;
            }
            catch
            {
                throw;
            }
        }
        public async Task<TModelo> Crear(TModelo modelo)
        {
            try
            {
                _dbContext.Set<TModelo>().Add(modelo);
                await _dbContext.SaveChangesAsync();
                return modelo;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> Editar(TModelo modelo)
        {
            try
            {
                _dbContext.Set<TModelo>().Update(modelo);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> Eliminar(TModelo modelo)
        {
            try
            {
                _dbContext.Set<TModelo>().Remove(modelo);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }
        public async Task<IQueryable<TModelo>> Consultar(Expression<Func<TModelo, bool>> filtro = null)
        {
            try
            {
                IQueryable<TModelo> queryModelo = filtro==null ? _dbContext.Set<TModelo>() : _dbContext.Set<TModelo>().Where(filtro);
                return queryModelo;// oara retornar esto tenemos que declara asincrono
            }
            catch
            {
                throw;
            }
        }
       

        
    }
}
