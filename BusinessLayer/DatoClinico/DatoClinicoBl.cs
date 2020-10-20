using System.Collections.Generic;
using System.Linq;
using BusinessLayer.DatoClinico.Interfaces;
using DataLayer.Area.DatoClinico;
using Model;

namespace BusinessLayer.DatoClinico
{
    public class DatoClinicoBl : IDatoClinicoBl
    {
        /// <summary>
        /// Descripción: Obtiene información de la Categoria y Enfermedad Categoria Dato.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idEnfermedad"></param>
        /// <returns></returns>
        public List<CategoriaDato> GetCategoriaByEnfermedad(int idEnfermedad, int idProyecto, string idExamen)
        {
            using (var datoClinicoDal = new DatoClinicoDal())
            {
                var categorias = datoClinicoDal.GetCategoriaByEnfermedad(idEnfermedad);

                return GetCategoriasTop(categorias).Select(cat => CreateCategoria(categorias, cat, idEnfermedad, idProyecto, idExamen)).ToList();
            }
        }
        /// <summary>
        /// Descripción: Obtiene los datos diferneciados de los nulos.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="categorias"></param>
        /// <returns></returns>
        private static IEnumerable<CategoriaDato> GetCategoriasTop(IEnumerable<CategoriaDato> categorias)
        {
            return categorias.Where(cat => cat.IdCategoriaDatoPadre == null);
        }
        /// <summary>
        /// Descripción: Obtiene datos cuyos IdCategoriaDatoPadre =  idCategoriaPadre
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="categorias"></param>
        /// <param name="idCategoriaPadre"></param>
        /// <returns></returns>
        private static IEnumerable<CategoriaDato> GetSubCategorias(IEnumerable<CategoriaDato> categorias, int idCategoriaPadre)
        {
            return categorias.Where(cat => cat.IdCategoriaDatoPadre == idCategoriaPadre);
        }
        /// <summary>
        /// Descripción: Retorna los datos en la entidad CategoriaDato
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="categorias"></param>
        /// <param name="categoria"></param>
        /// <param name="idEnfermedad"></param>
        /// <returns></returns>
        private static CategoriaDato CreateCategoria(IEnumerable<CategoriaDato> categorias, CategoriaDato categoria, int idEnfermedad, int idProyecto, string idExamen)
        {
            var idCategoriaDatoPadre = categoria.IdCategoriaDatoPadre;
            var idCategoria = categoria.IdCategoriaDato;
            var nombre = categoria.Nombre;
            var descripcion = categoria.Descripcion;
            var visible = categoria.Visible;
            var orientacion = categoria.Orientacion;
            var categoriaDatos = categorias as IList<CategoriaDato> ?? categorias.ToList();
            var subCategorias = GetSubCategorias(categoriaDatos, idCategoria).Select(cat => CreateCategoria(categoriaDatos, cat, idEnfermedad, idProyecto, idExamen)).ToList();
            var datos = GetDatos(subCategorias, idCategoria, idEnfermedad, idExamen);
            

            return new CategoriaDato
            {
                IdCategoriaDatoPadre = idCategoriaDatoPadre,
                IdCategoriaDato = idCategoria,
                Nombre = nombre,
                Descripcion = descripcion,
                Visible = visible,
                Orientacion = orientacion,
                SubCategorias = subCategorias,
                OrdenDatoClinicoList = datos != null? datos.Where(x => x.Dato.idProyecto == idProyecto || x.Dato.idProyecto == 0).ToList(): null
            };
        }
        /// <summary>
        /// Descripción: Obtiene informacion de los datos clinicos por categoria
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="subCategorias"></param>
        /// <param name="idCategoria"></param>
        /// <param name="idEnfermedad"></param>
        /// <returns></returns>
        private static List<OrdenDatoClinico> GetDatos(IEnumerable<CategoriaDato> subCategorias, int idCategoria, int idEnfermedad, string idExamen)
        {
            if (subCategorias.Any()) return null;

            using (var datoClinicoDal = new DatoClinicoDal())
            {
                var datos = datoClinicoDal.GetDatoByCategoria(idCategoria, idExamen);

                datos.ForEach(d => d.ListaDato = GetLista(d.IdListaDato));

                List<OrdenDatoClinico> ordenDatoClinicoList = new List<OrdenDatoClinico>();
                foreach (Dato dato in datos)
                {
                    var noPrceisa = (dato.Prefijo.Contains("Escala de Glasgow"));

                    OrdenDatoClinico ordenDatoClinico = new OrdenDatoClinico();
                    ordenDatoClinico.idCategoriaDato = idCategoria;
                    ordenDatoClinico.Dato = dato;
                    ordenDatoClinico.noPrecisa = noPrceisa;
                    ordenDatoClinico.Enfermedad = new Enfermedad();
                    ordenDatoClinico.Enfermedad.idEnfermedad = idEnfermedad;
                    
                    ordenDatoClinicoList.Add(ordenDatoClinico);
                }

                return ordenDatoClinicoList;
            }
        }
        /// <summary>
        /// Descripción: Retorna informacion de las listas de acuerdo al dato solicitado.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idListaDato"></param>
        /// <returns></returns>
        private static ListaDato GetLista(int? idListaDato)
        {
            if (!idListaDato.HasValue) return null;

            using (var listaDatoDal = new ListaDatoDal())
            {
                var lista = listaDatoDal.GetListaDatoById(idListaDato.Value);
                lista.Opciones = listaDatoDal.GetOpcionDatoByLista(idListaDato.Value);

                return lista;
            }
        } 
    }
}