using Framework.DAL;
using System.Collections.Generic;
using System.Data;
using Model;


namespace DataLayer
{
    public class UsuarioConvertTo : Converter
    {
        /// <summary>
        /// Descripcion: Convierte a List<menu> el datatable
        /// con informacion del menu.
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<Menu> ListMenu(DataTable dataTable)
        {
            List<Menu> lista = new List<Menu>();
            List<Menu> listaTemp = new List<Menu>();
            if (dataTable.Rows.Count == 0)
                return lista;

            int i = 0;
            int max = dataTable.Rows.Count;

            while (i < max)
            {
                var row = dataTable.Rows[i];

                Menu item = new Menu
                {
                    idMenu = GetInt(row, "idMenu"),
                    nombre = row["nombre"].ToString(),
                    URL = row["URL"].ToString(),
                    icon = row["icon"].ToString(),
                    idMenuPadre = GetInt(row, "idMenuPadre"),
                    orden = GetInt(row, "orden")
                };

                listaTemp.Add(item);

                if (item.idMenuPadre == 0)
                {
                    item.hijos = new List<Menu>();
                    lista.Add(item);
                } else
                {
                    foreach (Menu padre in listaTemp)
                    {
                        if(padre.idMenu == item.idMenuPadre)
                        {
                            if (padre.hijos == null) padre.hijos = new List<Menu>();
                            padre.hijos.Add(item);
                        }
                    }
                }
                
                i++;
            }

            return lista;
        }



        /// <summary>
        /// Descripcion: Convierte a List<menu> el datatable
        /// con informacion del usuario.
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static Usuario Usuario(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
                return null;
            
            var row = dataTable.Rows[0];
            System.Web.HttpContext.Current.Session["respuesta"] = row["respuesta"].ToString();

            return new Usuario
            {
                
                idUsuario = GetInt(row, "idUsuario"),
                login = row["login"].ToString(),
                documentoIdentidad = row["documentoIdentidad"].ToString(),
                apellidoPaterno = row["apellidoPaterno"].ToString(),
                apellidoMaterno = row["apellidoMaterno"].ToString(),
                nombres = row["nombres"].ToString(),
                iniciales = row["iniciales"].ToString(),
                codigoColegio = row["codigoColegio"].ToString(),
                RNE = row["RNE"].ToString(),
                correo = row["correo"].ToString(),
                Estado = GetInt(row, "estado"),
                fechaIngreso = GetDateTime(row, "fechaIngreso"),
                tiempoCaducidad = GetInt(row, "tiempoCaducidad"),
                estatus = GetInt(row, "estatus"),
                cargo = row["cargo"].ToString(),
                telefono = row["telefonoContacto"].ToString(),
                profesion = GetInt(row,"idProfesion"),
                tipo = GetInt(row, "idTipoUsuario"),
                firmaDigital = GetBytes(row,"firmadigital"),
                componente = GetInt(row,"idComponente"),
                idTipoUsuario = GetInt(row, "idTipoAcceso"),
                nAprobacion = GetInt(row,"idNivelAprobacion"),
                fechaCaducidad = GetDateTime(row, "fechaCaducidad")
            };
        }

        /// <summary>
        /// Descripcion: Devuelve la entidad Establecimiento obteniendo la informacion del Data Table.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static Establecimiento Establecimiento(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];

            return new Establecimiento
            {
                IdEstablecimiento = GetInt(row, "idEstablecimiento"),
                CodigoUnico = row["codigoUnico"].ToString(),
                Nombre = row["nombre"].ToString(),
                Ubigeo = row["ubigeo"].ToString(),
                Direccion = row["direccion"].ToString(),
                FechaRegistro = GetDateTime(row, "fechaRegistro")
            };
        }
        /// <summary>
        /// Descripcion: Devuelve la entidad Establecimiento obteniendo la informacion del Data Table.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<Establecimiento> ListEstablecimiento(DataTable dataTable)
        {
            List<Establecimiento> lista = new List<Establecimiento>();

            if (dataTable.Rows.Count == 0) { 
                return lista;
            }
            int i = 0;
            int max = dataTable.Rows.Count;

            while (i < max)
            {
                var row = dataTable.Rows[i];

                Establecimiento item = new Establecimiento
                {
                    IdEstablecimiento = GetInt(row, "idEstablecimiento"),
                    CodigoUnico = row["codigoUnico"].ToString(),
                    Nombre = row["nombre"].ToString(),
                    Ubigeo = row["ubigeo"].ToString(),
                    Direccion = row["direccion"].ToString(),
                    FechaRegistro = GetDateTime(row, "fechaRegistro")
                };
                lista.Add(item);

                i++;
            }

            return lista;
        }
        /// <summary>
        /// Descripcion: Convierte a List<Laboratorio> el datatable
        /// con informacion del menu.
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<Laboratorio> ListLaboratorio(DataTable dataTable)
        {
            List<Laboratorio> lista = new List<Laboratorio>();

            if (dataTable.Rows.Count == 0)
            {
                return lista;
            }
            int i = 0;
            int max = dataTable.Rows.Count;

            while (i < max)
            {
                var row = dataTable.Rows[i];

                Laboratorio item = new Laboratorio
                {
                    IdLaboratorio = GetInt(row, "idLaboratorio"),
                    CodigoUnico = row["codigoUnico"].ToString(),
                    Nombre = row["nombre"].ToString(),
                    Ubigeo = row["ubigeo"].ToString(),
                    Direccion = row["direccion"].ToString(),
                    FechaRegistro = GetDateTime(row, "fechaRegistro")
                };
                lista.Add(item);

                i++;
            }

            return lista;
        }


        //combos dependientes vivos
        /// <summary>
        /// Descripcion: Convierte a List<Institucion> el datatable
        /// con informacion del menu.
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<Institucion> ListInstitucion(DataTable dataTable) {


            List<Institucion> lista = new List<Institucion>();

            if (dataTable.Rows.Count == 0)
            {
                return lista;
            }
            int i = 0;
            int max = dataTable.Rows.Count;

            while (i < max)
            {
                var row = dataTable.Rows[i];

                Institucion item = new Institucion
                {
                    codigoInstitucion = GetInt(row, "codigoInstitucion"),
                    nombreInstitucion = row["nombreInstitucion"].ToString(),
                };
                lista.Add(item);

                i++;
            }

            return lista;


        }
        /// <summary>
        /// Descripcion: Convierte a List<DISA> el datatable
        /// con informacion del menu.
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<DISA> ListDisa(DataTable dataTable) {


            List<DISA> lista = new List<DISA>();

            if (dataTable.Rows.Count == 0)
            {
                return lista;
            }
            int i = 0;
            int max = dataTable.Rows.Count;

            while (i < max)
            {
                var row = dataTable.Rows[i];

                DISA item = new DISA
                {
                    IdDISA = row["IdDISA"].ToString(),
                    NombreDISA = row["NombreDISA"].ToString(),
                };
                lista.Add(item);

                i++;
            }

            return lista;


        }


    }
}
