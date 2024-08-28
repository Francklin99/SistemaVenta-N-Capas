using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SistemaVenta.Model;

namespace SistemaVenta.DAL.Repositorios.Contrato
{
    //HEREDA TODOS LOS METODO DEL SISTEMA GENERICO PERO PARA EL MODELO ESPECIFICO EN ESTE CASO DEL MODELO VENTA
    public interface IVentaRepository: IGenericRepository<Venta>
    {
        Task<Venta> Registrar(Venta modelo);
    }
}
