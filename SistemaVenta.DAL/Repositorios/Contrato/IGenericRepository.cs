using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using SistemaVenta.Model;

namespace SistemaVenta.DAL.Repositorios.Contrato
{
    //AQUI VAMOS A TENEMOS TODOS LOS METODO PARA INTERACTUAR CON NUESTROS MODELOS

    /*Esta interfaz define un repositorio genérico para interactuar con cualquier 
     * modelo de datos en la capa de acceso a datos (DAL). El repositorio es genérico 
     * porque puede manejar cualquier tipo de entidad (TModel), lo que permite reutilizar
     * el código para diferentes modelos sin tener que definir un repositorio específico para cada uno.*/
    public interface IGenericRepository<TModel> where TModel : class
    {
        //Este método se utiliza para obtener un único modelo de la base de datos
        //que cumpla con la condición especificada en el filtro. Devuelve el primer
        //modelo que coincide con el filtro o null si no se encuentra ningún registro.
        Task<TModel> Obtener(Expression<Func<TModel, bool>> filtro);






        //Este método se utiliza para crear un nuevo registro en la base de datos basado
        //en el modelo proporcionado. Devuelve el modelo creado después de guardarlo en
        //la base de datos.
        Task<TModel> Crear(TModel modelo);





        //Este método se utiliza para actualizar un registro existente en la base de datos
        //con los nuevos valores del modelo proporcionado. Devuelve true si la actualización
        //se realizó correctamente o false si falló.
        Task<bool> Editar(TModel modelo);




        //Este método se utiliza para eliminar un registro de la base de datos basado en el modelo
        //proporcionado. Devuelve true si la eliminación fue exitosa o false si falló.
        Task<bool> Eliminar(TModel modelo);



        // Este método se utiliza para consultar varios registros de la base de datos que cumplan
        // con la condición especificada en el filtro. Si no se proporciona ningún filtro, devuelve
        // todos los registros. Devuelve un conjunto de datos en formato IQueryable, lo que permite
        // aplicar operaciones adicionales como filtrado, ordenación o paginación antes de ejecutar
        // la consulta en la base de datos.
        Task<IQueryable<TModel>> Consultar(Expression<Func<TModel,bool>>filtro=null);
    }
}
