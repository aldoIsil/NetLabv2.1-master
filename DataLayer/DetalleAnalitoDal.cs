using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Model;
using System;
using DataLayer.DalConverter;

namespace DataLayer
{
    public class DetalleAnalitoDal : DaoBase
    {
        public DetalleAnalitoDal(IDalSettings settings) : base(settings)
        {
        }

        public DetalleAnalitoDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Obtiene informacion de los componenetes por examen.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrdenExamen"></param>
        /// <param name="genero"></param>
        /// <returns></returns>
        public List<ResultAnalito> GetAnalitoByOrdenExamenAndGenero(string idOrdenExamen, int genero, int? idPlataforma = null)
        {
            var objCommand = GetSqlCommand("pNLS_MostrarAnalitosPorExamen");
            InputParameterAdd.Varchar(objCommand, "idOrdenExamen", idOrdenExamen);
            InputParameterAdd.Int(objCommand, "GeneroPaciente", genero);
            var dataTable = Execute(objCommand);

            return (from DataRow row in dataTable.Rows
                    select new ResultAnalito
                    {
                        IdOrden = Converter.GetGuid(row, "IdOrden"),
                        IdExamen = Converter.GetGuid(row, "IdExamen"),
                        Analito = Converter.GetString(row, "Analito"),
                        IdOrdenResultadoAnalito = Converter.GetGuid(row, "idOrdenResultadoAnalito"),
                        Examen = Converter.GetString(row, "Examen"),
                        Resultado = Converter.GetString(row, "Resultado"),
                        Unidad = Converter.GetString(row, "Unidad"),
                        ValorReferencia = row.IsNull("ValorReferencia") ? "-" : Converter.GetString(row, "ValorReferencia"),
                        ValorInferior = row.IsNull("valorInferior") ? -1 : Converter.GetDecimal(row, "valorInferior"),
                        ValorSuperior = row.IsNull("valorSuperior") ? -1 : Converter.GetDecimal(row, "valorSuperior"),
                        Observacion = row.IsNull("Observacion") ? "": Converter.GetString(row, "Observacion"),
                        tipo = int.Parse(row["tipo"].ToString()),
                        estatusE = int.Parse(row["estatusE"].ToString()),
                        IdAnalito = Converter.GetGuid(row, "IdAnalito"),
                        codigoOpcion = Converter.GetString(row, "codigoOpcion"),
                        Metodos = GetAnalitosbyIdAnalito(Converter.GetGuid(row, "IdAnalito"), Converter.GetGuid(row, "idOrdenResultadoAnalito"), Converter.GetGuid(row, "idOrdenExamen"), idPlataforma),
                        IdOrdenExamen = Converter.GetGuid(row, "idOrdenExamen"),
                        idSecuencia = Converter.GetInt(row,"idSecuen")
                    }).ToList();
        }
        /// <summary>
        /// Descripción: Obtiene informacion para la validacion de los componenetes de un a prueba.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017.
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// Se realizaron las modificaciones para el registro de los datos configurados por componente. Juan Muga
        /// Se adicionó los números de celular del paciente y solicitante para envío de sms. Marcos Mejía
        /// </summary>
        /// <param name="idOrdenExamen"></param>
        /// <returns></returns>
        public List<ResultAnalito> GetAnalitoByOrdenExamen(string idOrdenExamen)
        {
            var objCommand = GetSqlCommand("pNLS_ValidarAnalitos");
            InputParameterAdd.Varchar(objCommand, "idOrdenExamen", idOrdenExamen);
            var dataTable = Execute(objCommand);

            return (from DataRow row in dataTable.Rows
                    select new ResultAnalito
                    {
                        IdOrden = Converter.GetGuid(row, "IdOrden"),
                        IdExamen = Converter.GetGuid(row, "IdExamen"),
                        Analito = Converter.GetString(row, "Analito"),
                        Examen = Converter.GetString(row, "Examen"),
                        Resultado = Converter.GetString(row, "Resultado"),
                        Unidad = Converter.GetString(row, "Unidad"),
                        ValorReferencia = Converter.GetString(row, "ValorReferencia"),
                        Observacion = Converter.GetString(row, "Observacion"),
                        tipo = int.Parse(row["tipo"].ToString()),
                        estatusE = int.Parse(row["estatusE"].ToString()),
                        IdAnalito = Converter.GetGuid(row, "IdAnalito"),
                        Metodos = GetAnalitosbyIdAnalito(Converter.GetGuid(row, "IdAnalito"), null),
                        ListJsResultados = GetAnalitosbyIdAnalitoResultado(new SerializarResultado().DeserializarResultados(Converter.GetString(row, "Resultado"))),
                        paciente = new Paciente
                        {
                            Celular1 = Converter.GetString(row, "celular1"),
                            NroDocumento = Converter.GetString(row, "nroDocumento")
                        },
                        solicitante = new OrdenSolicitante
                        {
                            telefonoContacto = Converter.GetString(row, "telefono")
                        },
                        codigoOpcion = Converter.GetString(row, "codigoOrden"),
                        Enfermedad = Converter.GetString(row, "Enfermedad"),
                        idSecuencia = dataTable.Rows.Count
                    }).ToList();
        }

        /// <summary>
        /// Descripción: Obtiene las opciones ingresados por cada analito.  
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idAnalito"></param>
        /// <returns></returns>
        public List<AnalitoOpcionResultado> GetAnalitosbyIdAnalito(Guid idAnalito, Guid? _IdOrdenResultadoAnalito, Guid? idOrdenExamen = null, int? idPlataforma=null)
        {
            var objCommand = GetSqlCommand("pNLS_OpcionesByAnalito");
            InputParameterAdd.Guid(objCommand, "idAnalito", idAnalito);
            InputParameterAdd.Int(objCommand, "idPlataforma", idPlataforma);
            var dataTable = Execute(objCommand);

            return (from DataRow row in dataTable.Rows
                    select new AnalitoOpcionResultado
                    {
                        IdAnalito = Converter.GetGuid(row, "IdAnalito"),
                        IdAnalitoOpcionResultado = int.Parse(row["idAnalitoOpcionResultado"].ToString()),
                        IdAnalitoOpcionParent = row["idOpcionParent"].ToString(),
                        Tipo = int.Parse(row["tipo"].ToString()),
                        Glosa = row["glosa"].ToString(),
                        Orden = int.Parse(row["ordenOpcR"].ToString()),
                        IdOrdenResultadoAnalito = _IdOrdenResultadoAnalito,
                        IdOrdenExamen = idOrdenExamen
                    }).ToList();
        }
        /// <summary>
        /// Descripción: Obtiene las opciones ingresados por cada analito, retorna configuracion de resultados con el formato correcto.  
        /// Author: Juan Muga.
        /// Fecha Creacion: 01/01/2017
        /// </summary>
        /// <param name="LstJsResultado"></param>
        /// <returns></returns>
        public List<AnalitoOpcionResultado> GetAnalitosbyIdAnalitoResultado(List<AnalitoValues> LstJsResultado)
        {
            var objCommand = GetSqlCommand("pNLS_OpcionesByAnalito");
            List<AnalitoOpcionResultado> lista = new List<AnalitoOpcionResultado>();
            var LsttodoRestultado = new List<AnalitoOpcionResultado>();
            foreach (var item in LstJsResultado)
            {
                InputParameterAdd.Guid(objCommand, "idAnalito", item.AnalitoId);
                if (LsttodoRestultado == null || LsttodoRestultado.Count == 0)
                {
                    LsttodoRestultado = Execute(objCommand).AsEnumerable().Select(dr => new AnalitoOpcionResultado()
                    {
                        IdAnalito = Converter.GetGuid(dr, "idAnalito"),
                        IdAnalitoOpcionResultado = Converter.GetInt(dr, "IdAnalitoOpcionResultado"),
                        Glosa = Converter.GetString(dr, "glosa"),
                        Orden = Converter.GetInt(dr, "ordenOpcR"),
                        Estado = Converter.GetInt(dr, "estado"),
                        IdAnalitoOpcionParent = Converter.GetString(dr, "idOpcionParent").ToString().TrimEnd(),
                        Tipo = Converter.GetInt(dr, "tipo"),
                        idAnalitoCabeceraVariable = Converter.GetInt(dr, "idAnalitoCabeceraVariable")
                    }).ToList();
                }

                switch (item.Tipo)
                {
                    case "checkbox":
                    case "CHECKBOX":
                        foreach (var datos in LsttodoRestultado.Where(x => x.IdAnalitoOpcionResultado.ToString().Trim() == item.Id))
                        {
                            var Lstcheck = LsttodoRestultado.Where(r => r.IdAnalitoOpcionResultado.ToString() == datos.IdAnalitoOpcionParent.ToString()).ToList();
                            if (Lstcheck != null)
                            {
                                if (Lstcheck.Count > 0)
                                {
                                        datos.GlosaParent = Lstcheck.ToList().FirstOrDefault().Glosa;
                                  
                                }
                            }
                            lista.Add(datos);
                        }
                        break;
                    case "combo":
                    case "COMBO":
                        foreach (var datos in LsttodoRestultado.Where(x => x.IdAnalitoOpcionResultado.ToString().Trim() == item.Value))
                        {
                            var Lstcombo = LsttodoRestultado.Where(r => r.IdAnalitoOpcionResultado.ToString() == item.Id.ToString()).ToList();
                            if (Lstcombo != null)
                            {
                                if (Lstcombo.Count > 0)
                                {
                                    datos.GlosaParent = Lstcombo.ToList().FirstOrDefault().Glosa;
                                }
                                else
                                {
                                    datos.GlosaParent = datos.Glosa;
                                }
                            }
                            lista.Add(datos);
                        }
                        break;
                    case "inputdate":
                    case "INPUTDATE":
                        foreach (var datos in LsttodoRestultado)
                        {
                            var Lstdate = LsttodoRestultado.Where(r => r.IdAnalitoOpcionParent.ToString() == datos.IdAnalitoOpcionResultado.ToString()).ToList();
                            if (Lstdate != null)
                            {
                                if (Lstdate.Count > 0)
                                {
                                    datos.GlosaParent = Lstdate.ToList().FirstOrDefault().Glosa;
                                }
                            }
                            datos.Glosa = item.Value;
                            lista.Add(datos);
                        }
                        break;
                    case "inputtext":
                    case "INPUTTEXT":
                    case "inputnumber":
                    case "INPUTNUMBER":
                    case "textarea":
                    case "TEXTAREA":

                        if (LsttodoRestultado.Count == 1)
                        {
                            foreach (var datos in LsttodoRestultado)
                            {
                                datos.Glosa = item.Value;
                                datos.GlosaParent = datos.Glosa;
                                lista.Add(datos);
                            }
                        }
                        else
                        {
                            foreach (var datos in LsttodoRestultado.Where(x => x.IdAnalitoOpcionResultado.ToString().Trim() == item.Id))
                            {
                                var Lsttext = LsttodoRestultado.Where(r => r.IdAnalitoOpcionParent.ToString() == datos.IdAnalitoOpcionResultado.ToString()).ToList();
                                if (Lsttext != null)
                                {
                                    if (Lsttext.Count > 0)
                                    {
                                        datos.GlosaParent = Lsttext.ToList().FirstOrDefault().Glosa;
                                    }
                                }
                                datos.GlosaParent = datos.Glosa;
                                datos.Glosa = item.Value;
                                lista.Add(datos);
                            }
                        }
                        break;
                    default:
                        break;
                }

            }

            return lista;
        }
    }
}