 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//importamos Adicional los siguiente
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Model;

namespace SistemaVenta.BLL.Servicios
{
    public class UsuarioService:IUsuarioService
    {
        private readonly IGenericRepository<Usuario> _usuarioRepositorio;
        private readonly IMapper _mapper;

        public UsuarioService(IGenericRepository<Usuario> genericRepository, IMapper mapper)
        {
            _usuarioRepositorio = genericRepository;
            _mapper = mapper;
        }

        public async Task<List<UsuarioDTO>> Lista()
        {
            try
            {
                var queryUsuario = await _usuarioRepositorio.Consultar();
                var listaUsuarios = queryUsuario.Include(rol => rol.IdRolNavigation).ToList();

                return _mapper.Map<List<UsuarioDTO>>(listaUsuarios);
            }
            catch
            {
                throw;
            }
        }

        public async Task<SesionDTO> ValidarCredenciales(string correo, string clave)
        {
            try
            {
                var queryUsuario = await _usuarioRepositorio.Consultar(u =>
                u.Correo==correo &&
                u.Clave==clave
                );
                if (queryUsuario.FirstOrDefault()==null)
                    throw new TaskCanceledException("Usuario No existe");

                Usuario devolverUsuario=queryUsuario.Include(rol=>rol.IdRolNavigation).First();
                return _mapper.Map<SesionDTO>(devolverUsuario);
                
            }
            catch 
            {
                throw;
            }
        }
        public async Task<UsuarioDTO> crear(UsuarioDTO modelo)
        {
            var UsuarioCreado = await _usuarioRepositorio.Crear(_mapper.Map<Usuario>(modelo));

            if (UsuarioCreado.IdUsuario==0)
                throw new TaskCanceledException("No se pudo crear");

            var query=await _usuarioRepositorio.Consultar(u=>u.IdUsuario==UsuarioCreado.IdUsuario);
            UsuarioCreado=query.Include(rol=>rol.IdRolNavigation).First();

            return _mapper.Map<UsuarioDTO>(UsuarioCreado);
        }

        public async Task<bool> Editar(UsuarioDTO modelo)
        {
            try
            {
                var UsuarioModelo=_mapper.Map<Usuario>(modelo);

                var UsuarioEncontrado = await _usuarioRepositorio.Obtener(u => u.IdUsuario==UsuarioModelo.IdUsuario);

                if (UsuarioEncontrado==null)
                    throw new TaskCanceledException("El Usuario no existe");

                UsuarioEncontrado.NombreCompleto=UsuarioModelo.NombreCompleto;
                UsuarioEncontrado.Correo=UsuarioModelo.Correo;
                UsuarioEncontrado.IdRol=UsuarioModelo.IdRol;
                UsuarioEncontrado.Clave=UsuarioModelo.Clave;
                UsuarioEncontrado.EsActivo=UsuarioModelo.EsActivo;

                bool respuesta=await _usuarioRepositorio.Editar(UsuarioEncontrado);

                if (!respuesta)
                    throw new TaskCanceledException("No se puede Editar");

                return respuesta;
            }
            catch 
            { 
                throw;
            }

        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var UsuarioEncontrado=await _usuarioRepositorio.Obtener(u=>u.IdUsuario==id);

                if (UsuarioEncontrado==null)
                    throw new TaskCanceledException("El usuario no existe");

                bool respuesta=await _usuarioRepositorio.Eliminar(UsuarioEncontrado);

                if (!respuesta)
                    throw new TaskCanceledException("No se pudo eliminar");

                return respuesta;
            }
            catch 
            {
                throw;
            }
        }

       
    }
}
