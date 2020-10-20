using ExcelDataReader;
using Model;
using Model.DTO;
using Model.ExcelDTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.DatoClinico;

namespace BusinessLayer.ProcesoDesatendido
{
    public class ExcelImportacion
    {
        public void ImportarExcel(string file, int idUsuarioRegistro)
        {
            try
            {
                Dictionary<string, string> xcel = new Dictionary<string, string>();
                ExcelImportHelpers helper = new ExcelImportHelpers();
                var hojas = helper.ObtenerListaHojas(file);
                List<ExcelEstablecimiento> establecimientos = new List<ExcelEstablecimiento>();
                List<Paciente> pacientes = new List<Paciente>();
                List<OrdenSolicitante> solicitantes = new List<OrdenSolicitante>();
                List<ExcelOrden> ordenes = new List<ExcelOrden>();
                List<ExcelOrdenMuestra> ordenmuestras = new List<ExcelOrdenMuestra>();
                List<ExcelOrdenExamen> ordenexamenes = new List<ExcelOrdenExamen>();
                List<ExcelOrdenExamenMuestra> ordenexamenmuestras = new List<ExcelOrdenExamenMuestra>();
                List<ExcelVIH> vihDatos = new List<ExcelVIH>();
                PacienteBl pacienteBL = new PacienteBl();
                MaterialBl materialBL = new MaterialBl();
                EnfermedadBl enfermedadBL = new EnfermedadBl();
                OrdenBl ordenBL = new OrdenBl();
                foreach (var hoja in hojas)
                {
                    //var columnas = helper.ObtenerListaColumnas(file, hoja);
                    var datos = helper.ObtenerDatosPorHoja(file, hoja);
                    switch (hoja)
                    {
                        case "Paciente":
                            pacientes = this.ImportarPacientes(file, hoja, datos.Item1, datos.Item2);
                            break;
                        case "Solicitante":
                            solicitantes = this.ImportarSolicitantes(file, hoja, datos.Item1, datos.Item2);
                            break;
                        case "Orden":
                            ordenes = this.ImportarOrdenes(file, hoja, datos.Item1, datos.Item2);
                            break;
                        case "ordenmuestra":
                            ordenmuestras = this.ImportarOrdenMuestras(file, hoja, datos.Item1, datos.Item2);
                            break;
                        case "ordenexamen":
                            ordenexamenes = this.ImportarOrdenExamenes(file, hoja, datos.Item1, datos.Item2);
                            break;
                        case "ordenexamenmuestra":
                            ordenexamenmuestras = this.ImportarOrdenExamenMuestras(file, hoja, datos.Item1, datos.Item2);
                            break;
                        case "origeneess":
                            establecimientos = this.ImportarEstablecimientos(file, hoja, datos.Item1, datos.Item2);
                            break;
                        case "vih":
                            vihDatos = this.ImportarVIH(file, hoja, datos.Item1, datos.Item2);
                            break;
                    }
                }

                foreach (var item in ordenes)
                {
                    var orden = new Orden();

                    orden.Proyecto = new Proyecto
                    {
                        IdProyecto = 1
                    };

                    orden.IdUsuarioRegistro = idUsuarioRegistro;

                    var establec = new Establecimiento() { IdEstablecimiento = int.Parse(item.EESSOrigen) };
                    //orden.establecimiento = establec;
                    var paciente = pacienteBL.GetPacienteByDocumento(item.Dni);
                    if (paciente == null)
                    {
                        var nuevoPacienteId = pacienteBL.InsertPaciente(pacientes.FirstOrDefault(x => x.NroDocumento == item.Dni));
                        orden.idPaciente = nuevoPacienteId;
                    }
                    else
                    {
                        orden.idPaciente = paciente.IdPaciente;
                    }

                    orden.Paciente = new Paciente { IdPaciente = orden.idPaciente };

                    orden.idEstablecimiento = int.Parse(item.EESSOrigen);
                    orden.solicitante = item.Solicitante;

                    //ORDENEXAMEN
                    orden.ordenExamenList = new List<OrdenExamen>();
                    orden.enfermedadList = new List<Enfermedad>();
                    foreach (var oe in ordenexamenes.Where(x => x.CodigoOrden == item.CodigoOrden))
                    {
                        orden.ordenExamenList.Add(new OrdenExamen
                        {
                            idEnfermedad = oe.IdEnfermedad,
                            Enfermedad = new Enfermedad { idEnfermedad = oe.IdEnfermedad },
                            idExamen = oe.IdExamen,
                            Examen = new Examen { idExamen = oe.IdExamen },
                            IdTipoMuestra = oe.IdTipoMuestra
                        });
                    }

                    //ORDENMUESTRA
                    orden.ordenMuestraList = new List<OrdenMuestra>();
                    orden.ordenMaterialList = new List<OrdenMaterial>();
                    foreach (var om in ordenmuestras.Where(x => x.CodigoOrden == item.CodigoOrden))
                    {
                        orden.ordenMuestraList.Add(new OrdenMuestra
                        {
                            idTipoMuestra = om.IdTipoMuestra,
                            TipoMuestra = new TipoMuestra { idTipoMuestra = om.IdTipoMuestra },
                            fechaColeccion = om.FechaObten,
                            horaColeccion = om.HoraObten,
                            idProyecto = om.CodigoMuestra,
                            MuestraCodificacion = new MuestraCodificacion()
                        });

                        //ORDENMATERIAL
                        var materiales = materialBL.GetMaterialesByIdTipoMuestra(om.IdTipoMuestra);
                        var material = materiales.FirstOrDefault(x => x.estado == 1);
                        if (material != null)
                        {
                            orden.ordenMaterialList.Add(new OrdenMaterial
                            {
                                cantidad = 1,
                                idMaterial = material.idMaterial,
                                Material = new Material { idMaterial = material.idMaterial },
                                fechaEnvio = DateTime.Today,
                                horaEnvio = DateTime.Now,
                                volumenMuestraColectada = material != null ? material.volumen : 1
                            });
                        }
                    }

                    //ORDENDATOCLINICO
                    orden.ordenDatoClinicoList = new List<OrdenDatoClinico>();
                    var preOrdenDatoClinico = new OrdenDatoClinico
                    {
                        FechaRegistro = DateTime.Now,
                        Enfermedad = new Enfermedad { idEnfermedad = 1005635 }
                    };

                    List<CategoriaDato> categoriaDatoList = new List<CategoriaDato>();
                    vihDatos.ForEach(x =>
                    {
                        #region Agregar OrdenDatoClinico
                        orden.ordenDatoClinicoList.Add(new OrdenDatoClinico
                        {
                            idCategoriaDato = 85,
                            FechaRegistro = DateTime.Now,
                            Enfermedad = new Enfermedad { idEnfermedad = 1005635 },
                            ValorString = x.antec_sexual,
                            noPrecisa = x.antec_sexual == "66",//no precisa
                            Dato = new Dato { IdDato = 190 }
                        });
                        orden.ordenDatoClinicoList.Add(new OrdenDatoClinico
                        {
                            idCategoriaDato = 90,
                            FechaRegistro = DateTime.Now,
                            Enfermedad = new Enfermedad { idEnfermedad = 1005635 },
                            ValorString = x.asoc_sida,
                            noPrecisa = x.asoc_sida == "73",
                            Dato = new Dato { IdDato = 206 }
                        });
                        orden.ordenDatoClinicoList.Add(new OrdenDatoClinico
                        {
                            idCategoriaDato = 89,
                            FechaRegistro = DateTime.Now,
                            Enfermedad = new Enfermedad { idEnfermedad = 1005635 },
                            ValorString = x.estadio,
                            Dato = new Dato { IdDato = 203 }
                        });
                        orden.ordenDatoClinicoList.Add(new OrdenDatoClinico
                        {
                            idCategoriaDato = 80,
                            FechaRegistro = DateTime.Now,
                            Enfermedad = new Enfermedad { idEnfermedad = 1005635 },
                            ValorString = x.etnia,
                            Dato = new Dato { IdDato = 185 }
                        });
                        orden.ordenDatoClinicoList.Add(new OrdenDatoClinico
                        {
                            idCategoriaDato = 90,
                            FechaRegistro = DateTime.Now,
                            Enfermedad = new Enfermedad { idEnfermedad = 1005635 },
                            ValorString = x.fecha_def.ToString(),
                            noPrecisa = x.fecha_def_nop,
                            Dato = new Dato { IdDato = 205 }
                        });
                        orden.ordenDatoClinicoList.Add(new OrdenDatoClinico
                        {
                            idCategoriaDato = 89,
                            FechaRegistro = DateTime.Now,
                            Enfermedad = new Enfermedad { idEnfermedad = 1005635 },
                            ValorString = x.fecha_ini_trat.ToString(),
                            noPrecisa = x.fecha_ini_trat_nop,
                            Dato = new Dato { IdDato = 202 }
                        });
                        orden.ordenDatoClinicoList.Add(new OrdenDatoClinico
                        {
                            idCategoriaDato = 152,
                            FechaRegistro = DateTime.Now,
                            Enfermedad = new Enfermedad { idEnfermedad = 1005635 },
                            ValorString = x.fecha_ini_sint.ToString(),
                            noPrecisa = x.fecha_ini_sint_nop,
                            Dato = new Dato { IdDato = 424 }
                        });
                        orden.ordenDatoClinicoList.Add(new OrdenDatoClinico
                        {
                            idCategoriaDato = 87,
                            FechaRegistro = DateTime.Now,
                            Enfermedad = new Enfermedad { idEnfermedad = 1005635 },
                            ValorString = x.fecha_conf1.ToString(),
                            Dato = new Dato { IdDato = 198 }
                        });
                        orden.ordenDatoClinicoList.Add(new OrdenDatoClinico
                        {
                            idCategoriaDato = 87,
                            FechaRegistro = DateTime.Now,
                            Enfermedad = new Enfermedad { idEnfermedad = 1005635 },
                            ValorString = x.fecha_conf2.ToString(),
                            noPrecisa = x.fecha_conf2_nop,
                            Dato = new Dato { IdDato = 200 }
                        });
                        orden.ordenDatoClinicoList.Add(new OrdenDatoClinico
                        {
                            idCategoriaDato = 87,
                            FechaRegistro = DateTime.Now,
                            Enfermedad = new Enfermedad { idEnfermedad = 1005635 },
                            ValorString = x.fecha_tamiz1.ToString(),
                            noPrecisa = x.fecha_tamiz1_nop,
                            Dato = new Dato { IdDato = 194 }
                        });
                        orden.ordenDatoClinicoList.Add(new OrdenDatoClinico
                        {
                            idCategoriaDato = 87,
                            FechaRegistro = DateTime.Now,
                            Enfermedad = new Enfermedad { idEnfermedad = 1005635 },
                            ValorString = x.fecha_tamiz2.ToString(),
                            noPrecisa = x.fecha_tamiz2_nop,
                            Dato = new Dato { IdDato = 196 }
                        });
                        orden.ordenDatoClinicoList.Add(new OrdenDatoClinico
                        {
                            idCategoriaDato = 81,
                            FechaRegistro = DateTime.Now,
                            Enfermedad = new Enfermedad { idEnfermedad = 1005635 },
                            ValorString = x.grado_instrucc,
                            Dato = new Dato { IdDato = 186 }
                        });
                        orden.ordenDatoClinicoList.Add(new OrdenDatoClinico
                        {
                            idCategoriaDato = 84,
                            FechaRegistro = DateTime.Now,
                            Enfermedad = new Enfermedad { idEnfermedad = 1005635 },
                            ValorString = x.ident_genero,
                            Dato = new Dato { IdDato = 189 }
                        });
                        orden.ordenDatoClinicoList.Add(new OrdenDatoClinico
                        {
                            idCategoriaDato = 89,
                            FechaRegistro = DateTime.Now,
                            Enfermedad = new Enfermedad { idEnfermedad = 1005635 },
                            ValorString = x.infecc_tbc,
                            Dato = new Dato { IdDato = 204 }
                        });
                        orden.ordenDatoClinicoList.Add(new OrdenDatoClinico
                        {
                            idCategoriaDato = 72,
                            FechaRegistro = DateTime.Now,
                            Enfermedad = new Enfermedad { idEnfermedad = 1005635 },
                            ValorString = x.motivonoti,
                            Dato = new Dato { IdDato = 183 }
                        });
                        orden.ordenDatoClinicoList.Add(new OrdenDatoClinico
                        {
                            idCategoriaDato = 90,
                            FechaRegistro = DateTime.Now,
                            Enfermedad = new Enfermedad { idEnfermedad = 1005635 },
                            ValorString = x.otra_causa,
                            noPrecisa = x.otra_causa_nop,
                            Dato = new Dato { IdDato = 207 }
                        });
                        orden.ordenDatoClinicoList.Add(new OrdenDatoClinico
                        {
                            idCategoriaDato = 87,
                            FechaRegistro = DateTime.Now,
                            Enfermedad = new Enfermedad { idEnfermedad = 1005635 },
                            ValorString = x.prueba_conf1,
                            Dato = new Dato { IdDato = 199 }
                        });
                        orden.ordenDatoClinicoList.Add(new OrdenDatoClinico
                        {
                            idCategoriaDato = 87,
                            FechaRegistro = DateTime.Now,
                            Enfermedad = new Enfermedad { idEnfermedad = 1005635 },
                            ValorString = x.prueba_conf2,
                            Dato = new Dato { IdDato = 201 }
                        });
                        orden.ordenDatoClinicoList.Add(new OrdenDatoClinico
                        {
                            idCategoriaDato = 87,
                            FechaRegistro = DateTime.Now,
                            Enfermedad = new Enfermedad { idEnfermedad = 1005635 },
                            ValorString = x.prueba_tamiz1,
                            Dato = new Dato { IdDato = 195 }
                        });
                        orden.ordenDatoClinicoList.Add(new OrdenDatoClinico
                        {
                            idCategoriaDato = 87,
                            FechaRegistro = DateTime.Now,
                            Enfermedad = new Enfermedad { idEnfermedad = 1005635 },
                            ValorString = x.prueba_tamiz2,
                            Dato = new Dato { IdDato = 197 }
                        });
                        orden.ordenDatoClinicoList.Add(new OrdenDatoClinico
                        {
                            idCategoriaDato = 83,
                            FechaRegistro = DateTime.Now,
                            Enfermedad = new Enfermedad { idEnfermedad = 1005635 },
                            ValorString = x.sexo_nac,
                            Dato = new Dato { IdDato = 188 }
                        });
                        orden.ordenDatoClinicoList.Add(new OrdenDatoClinico
                        {
                            idCategoriaDato = 86,
                            FechaRegistro = DateTime.Now,
                            Enfermedad = new Enfermedad { idEnfermedad = 1005635 },
                            ValorString = x.via_transm,
                            Dato = new Dato { IdDato = 191 }
                        });

                        #endregion
                        #region CategoriaDato
                        categoriaDatoList.Add(new CategoriaDato
                        {
                            IdCategoriaDato = 85
                        });
                        categoriaDatoList.Add(new CategoriaDato
                        {
                            IdCategoriaDato = 90
                        });
                        categoriaDatoList.Add(new CategoriaDato
                        {
                            IdCategoriaDato = 89
                        });
                        categoriaDatoList.Add(new CategoriaDato
                        {
                            IdCategoriaDato = 80
                        });
                        categoriaDatoList.Add(new CategoriaDato
                        {
                            IdCategoriaDato = 90
                        });
                        categoriaDatoList.Add(new CategoriaDato
                        {
                            IdCategoriaDato = 152
                        });
                        categoriaDatoList.Add(new CategoriaDato
                        {
                            IdCategoriaDato = 87
                        });
                        categoriaDatoList.Add(new CategoriaDato
                        {
                            IdCategoriaDato = 81
                        });
                        categoriaDatoList.Add(new CategoriaDato
                        {
                            IdCategoriaDato = 84
                        });
                        categoriaDatoList.Add(new CategoriaDato
                        {
                            IdCategoriaDato = 72
                        });
                        categoriaDatoList.Add(new CategoriaDato
                        {
                            IdCategoriaDato = 83
                        });
                        categoriaDatoList.Add(new CategoriaDato
                        {
                            IdCategoriaDato = 86
                        });

                        #endregion
                    });

                    orden.enfermedadList.Add(new Enfermedad
                    {
                        idEnfermedad = 1005635,//VIH
                        categoriaDatoList = categoriaDatoList
                    });
                    ordenBL.InsertOrden(orden, Enums.TipoRegistroOrden.SOLO_ORDEN);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<Paciente> ImportarPacientes(string file, string hoja, List<Dictionary<string, string>> datos, List<string> ordenColumnas)
        {
            List<Paciente> pacientes = new List<Paciente>();
            foreach (var rowItem in datos)
            {
                Paciente paciente = new Paciente();
                for (int i = 0; i < ordenColumnas.Count; i++)
                {
                    var key = ordenColumnas[i];
                    string svalue = string.Empty;

                    if (rowItem.Count > i)
                    {
                        //int keyIndex = ordenColumnas.IndexOf(key);
                        //svalue = eDictionary.ContainsKey(key) ? (!string.IsNullOrEmpty(rowItem[keyIndex]) ? rowItem[keyIndex] : sdefault) : sdefault;
                        svalue = !string.IsNullOrEmpty(rowItem[key]) ? rowItem[key] : string.Empty;

                        switch ((PacienteColumnas)Enum.Parse(typeof(PacienteColumnas), key.ToLower()))
                        {
                            //case PacienteColumnas.apellidopaterno:
                            //    paciente.ApellidoPaterno = svalue;
                            //    break;
                            //case PacienteColumnas.apellidomaterno:
                            //    paciente.ApellidoMaterno = svalue;
                            //    break;
                            //case PacienteColumnas.nombres:
                            //    paciente.Nombres = svalue;
                            //    break;
                            //case PacienteColumnas.tipodocumento:
                            //    paciente.TipoDocumento = !string.IsNullOrWhiteSpace(svalue) ? Convert.ToInt32(svalue) : 0;
                            //    break;
                            case PacienteColumnas.Nro_documento:
                                paciente.NroDocumento = svalue;
                                break;
                            //case PacienteColumnas.cel1:
                            //    paciente.Celular1 = svalue;
                            //    break;
                            //case PacienteColumnas.cel2:
                            //    paciente.Celular2 = svalue;
                            //    break;
                            //case PacienteColumnas.correo:
                            //    paciente.correoElectronico = svalue;
                            //    break;
                            //case PacienteColumnas.direccion:
                            //    paciente.DireccionActual = svalue;
                            //    break;
                            //case PacienteColumnas.fechanacimiento:
                            //    paciente.FechaNacimiento = !string.IsNullOrWhiteSpace(svalue) ? Convert.ToDateTime(svalue) : (DateTime?)null;
                            //    break;
                            //case PacienteColumnas.genero:
                            //    if (string.IsNullOrWhiteSpace(svalue))
                            //    {
                            //        paciente.Genero = (int?)null;
                            //    }
                            //    else
                            //    {
                            //        if (svalue.ToLower() == "masculino")
                            //        {
                            //            paciente.Genero = 1;
                            //        }
                            //        else if (svalue.ToLower() == "femenino")
                            //        {
                            //            paciente.Genero = 2;
                            //        }
                            //    }
                            //    break;
                            //case PacienteColumnas.otroseguro:
                            //    paciente.otroSeguroSalud = svalue;
                            //    break;
                            //case PacienteColumnas.profesion:
                            //    paciente.ocupacion = svalue;
                            //    break;
                            //case PacienteColumnas.seguro:
                            //    paciente.tipoSeguroSalud = !string.IsNullOrWhiteSpace(svalue) ? Convert.ToInt32(svalue) : (int?)null;
                            //    break;
                            //case PacienteColumnas.telefono:
                            //    paciente.TelefonoFijo = svalue;
                            //    break;
                            //case PacienteColumnas.ubigeo:
                            //    paciente.UbigeoActual = new Ubigeo { Id = svalue };
                            //    paciente.UbigeoReniec = new Ubigeo { Id = svalue };
                            //    break;
                        }
                    }
                }

                pacientes.Add(paciente);
            }

            return pacientes;
        }

        public List<OrdenSolicitante> ImportarSolicitantes(string file, string hoja, List<Dictionary<string, string>> datos, List<string> ordenColumnas)
        {
            List<OrdenSolicitante> entidades = new List<OrdenSolicitante>();
            foreach (var rowItem in datos)
            {
                OrdenSolicitante entidad = new OrdenSolicitante();
                for (int i = 0; i < ordenColumnas.Count; i++)
                {
                    var key = ordenColumnas[i];
                    string svalue = string.Empty;

                    if (rowItem.Count > i)
                    {
                        //int keyIndex = ordenColumnas.IndexOf(key);
                        //svalue = eDictionary.ContainsKey(key) ? (!string.IsNullOrEmpty(rowItem[keyIndex]) ? rowItem[keyIndex] : sdefault) : sdefault;
                        svalue = !string.IsNullOrEmpty(rowItem[key]) ? rowItem[key] : string.Empty;

                        switch ((SolicitanteColumnas)Enum.Parse(typeof(SolicitanteColumnas), key.ToLower()))
                        {
                            case SolicitanteColumnas.apepat:
                                entidad.apellidoPaterno = svalue;
                                break;
                            case SolicitanteColumnas.apemat:
                                entidad.apellidoMaterno = svalue;
                                break;
                            case SolicitanteColumnas.nombres:
                                entidad.Nombres = svalue;
                                break;
                            case SolicitanteColumnas.dni:
                                entidad.Dni = svalue;
                                break;
                            case SolicitanteColumnas.codigoprof:
                                entidad.codigoColegio = !string.IsNullOrWhiteSpace(svalue) ? Convert.ToInt32(svalue) : 0;
                                break;
                            case SolicitanteColumnas.profesion:
                                entidad.idProfesion = !string.IsNullOrWhiteSpace(svalue) ? Convert.ToInt32(svalue) : 0;
                                break;
                            case SolicitanteColumnas.correo:
                                entidad.correo = svalue;
                                break;
                            case SolicitanteColumnas.centrotrabajo:

                                break;
                            case SolicitanteColumnas.telefono:
                                entidad.telefonoContacto = svalue;
                                break;
                        }
                    }
                }

                entidades.Add(entidad);
            }

            return entidades;
        }

        public List<ExcelOrden> ImportarOrdenes(string file, string hoja, List<Dictionary<string, string>> datos, List<string> ordenColumnas)
        {
            List<ExcelOrden> entidades = new List<ExcelOrden>();
            foreach (var rowItem in datos)
            {
                ExcelOrden entidad = new ExcelOrden();
                for (int i = 0; i < ordenColumnas.Count; i++)
                {
                    var key = ordenColumnas[i];
                    string svalue = string.Empty;

                    if (rowItem.Count > i)
                    {
                        //int keyIndex = ordenColumnas.IndexOf(key);
                        //svalue = eDictionary.ContainsKey(key) ? (!string.IsNullOrEmpty(rowItem[keyIndex]) ? rowItem[keyIndex] : sdefault) : sdefault;
                        svalue = !string.IsNullOrEmpty(rowItem[key]) ? rowItem[key] : string.Empty;
                        switch ((OrdenColumnas)Enum.Parse(typeof(OrdenColumnas), key.ToLower()))
                        {
                            case OrdenColumnas.codigodeorden:
                                entidad.CodigoOrden = svalue;
                                break;
                            case OrdenColumnas.dni:
                                entidad.Dni = svalue;
                                break;
                            case OrdenColumnas.motivo:
                                entidad.Motivo = svalue;
                                break;
                            case OrdenColumnas.eessorigen:
                                entidad.EESSOrigen = svalue;
                                break;
                            case OrdenColumnas.fechasolicitud:
                                entidad.FechaSolicitud = svalue;
                                break;
                            case OrdenColumnas.solicitante:
                                entidad.Solicitante = svalue;
                                break;
                            case OrdenColumnas.observaciones:
                                entidad.Observaciones = svalue;
                                break;
                        }
                    }
                }

                entidades.Add(entidad);
            }

            return entidades;
        }

        public List<ExcelOrdenMuestra> ImportarOrdenMuestras(string file, string hoja, List<Dictionary<string, string>> datos, List<string> ordenColumnas)
        {
            List<ExcelOrdenMuestra> entidades = new List<ExcelOrdenMuestra>();
            foreach (var rowItem in datos)
            {
                ExcelOrdenMuestra entidad = new ExcelOrdenMuestra();
                for (int i = 0; i < ordenColumnas.Count; i++)
                {
                    var key = ordenColumnas[i];
                    string svalue = string.Empty;

                    if (rowItem.Count > i)
                    {
                        //int keyIndex = ordenColumnas.IndexOf(key);
                        //svalue = eDictionary.ContainsKey(key) ? (!string.IsNullOrEmpty(rowItem[keyIndex]) ? rowItem[keyIndex] : sdefault) : sdefault;
                        svalue = !string.IsNullOrEmpty(rowItem[key]) ? rowItem[key] : string.Empty;
                        switch ((OrdenMuestraColumnas)Enum.Parse(typeof(OrdenMuestraColumnas), key.ToLower()))
                        {
                            case OrdenMuestraColumnas.codigodeorden:
                                entidad.CodigoOrden = svalue;
                                break;
                            case OrdenMuestraColumnas.item:
                                entidad.Item = svalue;
                                break;
                            case OrdenMuestraColumnas.idtipomuestra:
                                entidad.IdTipoMuestra = !string.IsNullOrWhiteSpace(svalue) ? Convert.ToInt32(svalue) : 0;
                                break;
                            case OrdenMuestraColumnas.fechaobten:
                                //convertir fecha y hora
                                //var fecha = svalue.Split('/');
                                //entidad.FechaObten = new DateTime(Convert.ToInt32(fecha[2]), Convert.ToInt32(fecha[1]),
                                //    Convert.ToInt32(fecha[0]));

                                entidad.FechaObten = !string.IsNullOrWhiteSpace(svalue) ? Convert.ToDateTime(svalue) : DateTime.Today;
                                break;
                            case OrdenMuestraColumnas.horaobten:
                                //var hora = svalue.Split(':');
                                //entidad.HoraObten = new DateTime(entidad.FechaObten.Year, entidad.FechaObten.Month,
                                //    entidad.FechaObten.Day, Convert.ToInt32(hora[0]), Convert.ToInt32(hora[1]), 0);
                                if (!string.IsNullOrWhiteSpace(svalue))
                                {
                                    var fechahora = Convert.ToDateTime(svalue);
                                    entidad.HoraObten = new DateTime(entidad.FechaObten.Year, entidad.FechaObten.Month, entidad.FechaObten.Day, fechahora.Hour, fechahora.Minute, fechahora.Second);
                                }
                                else
                                {
                                    entidad.HoraObten = DateTime.Now;
                                }
                                break;
                            case OrdenMuestraColumnas.codigomuestra:
                                entidad.CodigoMuestra = !string.IsNullOrWhiteSpace(svalue) ? Convert.ToInt32(svalue) : 0;
                                break;
                        }
                    }
                }

                entidades.Add(entidad);
            }

            return entidades;
        }

        public List<ExcelOrdenExamen> ImportarOrdenExamenes(string file, string hoja, List<Dictionary<string, string>> datos, List<string> ordenColumnas)
        {
            List<ExcelOrdenExamen> entidades = new List<ExcelOrdenExamen>();
            foreach (var rowItem in datos)
            {
                ExcelOrdenExamen entidad = new ExcelOrdenExamen();
                for (int i = 0; i < ordenColumnas.Count; i++)
                {
                    var key = ordenColumnas[i];
                    string svalue = string.Empty;

                    if (rowItem.Count > i)
                    {
                        //int keyIndex = ordenColumnas.IndexOf(key);
                        //svalue = eDictionary.ContainsKey(key) ? (!string.IsNullOrEmpty(rowItem[keyIndex]) ? rowItem[keyIndex] : sdefault) : sdefault;
                        svalue = !string.IsNullOrEmpty(rowItem[key]) ? rowItem[key] : string.Empty;
                        switch ((OrdenExamenColumnas)Enum.Parse(typeof(OrdenExamenColumnas), key.ToLower()))
                        {
                            case OrdenExamenColumnas.codigodeorden:
                                entidad.CodigoOrden = svalue;
                                break;
                            case OrdenExamenColumnas.item:
                                entidad.Item = svalue;
                                break;
                            case OrdenExamenColumnas.idtipomuestra:
                                entidad.IdTipoMuestra = !string.IsNullOrWhiteSpace(svalue) ? Convert.ToInt32(svalue) : 0;
                                break;
                            case OrdenExamenColumnas.idenfermedad:
                                entidad.IdEnfermedad = !string.IsNullOrWhiteSpace(svalue) ? Convert.ToInt32(svalue) : 0;
                                break;
                            case OrdenExamenColumnas.idexamen:
                                if (!string.IsNullOrWhiteSpace(svalue))
                                {
                                    Guid idexamen = new Guid();
                                    Guid.TryParse(svalue, out idexamen);
                                    entidad.IdExamen = idexamen;
                                }
                                else
                                {
                                    //no deberia suceder, reportar este errir
                                }

                                break;
                        }
                    }
                }

                entidades.Add(entidad);
            }

            return entidades;
        }

        public List<ExcelOrdenExamenMuestra> ImportarOrdenExamenMuestras(string file, string hoja, List<Dictionary<string, string>> datos, List<string> ordenColumnas)
        {
            List<ExcelOrdenExamenMuestra> entidades = new List<ExcelOrdenExamenMuestra>();

            foreach (var rowItem in datos)
            {
                ExcelOrdenExamenMuestra entidad = new ExcelOrdenExamenMuestra();
                for (int i = 0; i < ordenColumnas.Count; i++)
                {
                    var key = ordenColumnas[i];
                    string svalue = string.Empty;

                    if (rowItem.Count > i)
                    {
                        svalue = !string.IsNullOrEmpty(rowItem[key]) ? rowItem[key] : string.Empty;
                        switch ((OrdenExamenMuestraColumnas)Enum.Parse(typeof(OrdenExamenMuestraColumnas), key.ToLower()))
                        {
                            case OrdenExamenMuestraColumnas.codigodeorden:
                                entidad.CodigoOrden = svalue;
                                break;
                            case OrdenExamenMuestraColumnas.item:
                                entidad.Item = svalue;
                                break;
                            case OrdenExamenMuestraColumnas.idtipomuestra:
                                entidad.IdTipoMuestra = !string.IsNullOrWhiteSpace(svalue) ? Convert.ToInt32(svalue) : 0;
                                break;
                            case OrdenExamenMuestraColumnas.estabdestino:
                                entidad.EstabDestino = !string.IsNullOrWhiteSpace(svalue) ? Convert.ToInt32(svalue) : 0;
                                break;
                            case OrdenExamenMuestraColumnas.numeromuestra:
                                entidad.NumeroMuestra = !string.IsNullOrWhiteSpace(svalue) ? Convert.ToInt32(svalue) : 0;
                                break;
                            case OrdenExamenMuestraColumnas.idexamen:
                                if (!string.IsNullOrWhiteSpace(svalue))
                                {
                                    Guid idexamen = new Guid();
                                    Guid.TryParse(svalue, out idexamen);
                                    entidad.IdExamen = idexamen;
                                }
                                break;
                        }
                    }
                }

                entidades.Add(entidad);
            }

            return entidades;
        }

        public List<ExcelEstablecimiento> ImportarEstablecimientos(string file, string hoja, List<Dictionary<string, string>> datos, List<string> ordenColumnas)
        {
            List<ExcelEstablecimiento> entidades = new List<ExcelEstablecimiento>();
            foreach (var rowItem in datos)
            {
                ExcelEstablecimiento entidad = new ExcelEstablecimiento();
                for (int i = 0; i < ordenColumnas.Count; i++)
                {
                    var key = ordenColumnas[i];
                    string svalue = string.Empty;

                    if (rowItem.Count > i)
                    {
                        svalue = !string.IsNullOrEmpty(rowItem[key]) ? rowItem[key] : string.Empty;
                        switch ((EstablecimientoColumnas)Enum.Parse(typeof(EstablecimientoColumnas), key.ToLower()))
                        {
                            case EstablecimientoColumnas.establecimiento:
                                entidad.Establecimiento = svalue;
                                break;
                            case EstablecimientoColumnas.codigo:
                                entidad.Codigo = svalue;
                                break;
                            case EstablecimientoColumnas.año:
                                entidad.Anio = !string.IsNullOrWhiteSpace(svalue) ? Convert.ToInt32(svalue) : 0;
                                break;
                            case EstablecimientoColumnas.orden:
                                entidad.Orden = !string.IsNullOrWhiteSpace(svalue) ? Convert.ToInt32(svalue) : 0;
                                break;
                        }
                    }
                }

                entidades.Add(entidad);
            }

            return entidades;
        }

        public List<ExcelVIH> ImportarVIH(string file, string hoja, List<Dictionary<string, string>> datos, List<string> ordenColumnas)
        {
            List<ExcelVIH> entidades = new List<ExcelVIH>();
            foreach (var rowItem in datos)
            {
                var entidad = new ExcelVIH();
                for (int i = 0; i < ordenColumnas.Count; i++)
                {
                    var key = ordenColumnas[i];
                    string svalue = string.Empty;

                    if (rowItem.Count > i)
                    {
                        svalue = !string.IsNullOrEmpty(rowItem[key]) ? rowItem[key] : string.Empty;
                        switch ((VIHColumnas)Enum.Parse(typeof(VIHColumnas), key.ToLower()))
                        {
                            case VIHColumnas.codigodeorden:
                                entidad.codigoorden = svalue;
                                break;
                            case VIHColumnas.motivonoti:
                                entidad.motivonoti = svalue;
                                break;
                            case VIHColumnas.etnia:
                                entidad.etnia = svalue;
                                break;
                            case VIHColumnas.grado_instrucc:
                                entidad.grado_instrucc = svalue;
                                break;
                            case VIHColumnas.condic_espec:
                                entidad.condic_espec = svalue;
                                break;
                            case VIHColumnas.sexo_nac:
                                entidad.sexo_nac = svalue;
                                break;
                            case VIHColumnas.ident_genero:
                                entidad.ident_genero = svalue;
                                break;
                            case VIHColumnas.antec_sexual:
                                entidad.antec_sexual = svalue;
                                break;
                            case VIHColumnas.via_transm:
                                entidad.via_transm = svalue;
                                break;
                            case VIHColumnas.fecha_tamiz1:
                                entidad.fecha_tamiz1 = !string.IsNullOrWhiteSpace(svalue) ? Convert.ToDateTime(svalue) : (DateTime?)null;
                                break;
                            case VIHColumnas.fecha_tamiz1_nop:
                                entidad.fecha_tamiz1_nop = !string.IsNullOrWhiteSpace(svalue) && svalue == "VERDADERO";
                                break;
                            case VIHColumnas.prueba_tamiz1:
                                break;
                            case VIHColumnas.fecha_tamiz2:
                                entidad.fecha_tamiz2 = !string.IsNullOrWhiteSpace(svalue) ? Convert.ToDateTime(svalue) : (DateTime?)null;
                                break;
                            case VIHColumnas.fecha_tamiz2_nop:
                                entidad.fecha_tamiz2_nop = !string.IsNullOrWhiteSpace(svalue) && svalue == "VERDADERO";
                                break;
                            case VIHColumnas.prueba_tamiz2:
                                break;
                            case VIHColumnas.fecha_conf1:
                                entidad.fecha_conf1 = !string.IsNullOrWhiteSpace(svalue) ? Convert.ToDateTime(svalue) : (DateTime?)null;
                                break;
                            case VIHColumnas.prueba_tamiz2_nop:
                                entidad.prueba_tamiz2_nop = !string.IsNullOrWhiteSpace(svalue) && svalue == "VERDADERO";
                                break;
                            case VIHColumnas.prueba_conf1:
                                break;
                            case VIHColumnas.fecha_conf2:
                                entidad.fecha_conf2 = !string.IsNullOrWhiteSpace(svalue) ? Convert.ToDateTime(svalue) : (DateTime?)null;
                                break;
                            case VIHColumnas.fecha_conf2_nop:
                                entidad.fecha_conf2_nop = !string.IsNullOrWhiteSpace(svalue) && svalue == "VERDADERO";
                                break;
                            case VIHColumnas.prueba_conf2:
                                break;
                            case VIHColumnas.fecha_ini_trat:
                                entidad.fecha_ini_trat = !string.IsNullOrWhiteSpace(svalue) ? Convert.ToDateTime(svalue) : (DateTime?)null;
                                break;
                            case VIHColumnas.fecha_ini_trat_nop:
                                entidad.fecha_ini_trat_nop = !string.IsNullOrWhiteSpace(svalue) && svalue == "VERDADERO";
                                break;
                            case VIHColumnas.estadio:
                                break;
                            case VIHColumnas.infecc_tbc:
                                break;
                            case VIHColumnas.fecha_def:
                                entidad.fecha_def = !string.IsNullOrWhiteSpace(svalue) ? Convert.ToDateTime(svalue) : (DateTime?)null;
                                break;
                            case VIHColumnas.fecha_def_nop:
                                entidad.fecha_def_nop = !string.IsNullOrWhiteSpace(svalue) && svalue == "VERDADERO";
                                break;
                            case VIHColumnas.asoc_sida:
                                break;
                            case VIHColumnas.otra_causa:
                                entidad.otra_causa = svalue;
                                break;
                            case VIHColumnas.otra_causa_nop:
                                entidad.otra_causa_nop = !string.IsNullOrWhiteSpace(svalue) && svalue == "VERDADERO";
                                break;
                            case VIHColumnas.fecha_ini_sint:
                                entidad.fecha_ini_sint = !string.IsNullOrWhiteSpace(svalue) ? Convert.ToDateTime(svalue) : (DateTime?)null;
                                break;
                            case VIHColumnas.fecha_ini_sint_nop:
                                entidad.fecha_ini_sint_nop = !string.IsNullOrWhiteSpace(svalue) && svalue == "VERDADERO";
                                break;
                            default:
                                break;
                        }
                    }
                }

                entidades.Add(entidad);
            }

            return entidades;
        }

        public void RegistrarPacientesExcel(string file, int idUsuarioRegistro)
        {
            ExcelImportHelpers helper = new ExcelImportHelpers();
            List<Paciente> pacientes = new List<Paciente>();
            var datos = helper.ObtenerDatosPorHoja(file, "nuevospacientes");
            foreach (var rowItem in datos.Item1)
            {
                var paciente = new Paciente();
                for (int i = 0; i < datos.Item2.Count; i++)
                {
                    var key = datos.Item2[i];
                    string svalue = string.Empty;
                    if (rowItem.Count > i)
                    {
                        if (!string.IsNullOrWhiteSpace(key))
                        {
                            key = key.Replace(" ", string.Empty).ToLower();
                            svalue = !string.IsNullOrEmpty(rowItem[key]) ? rowItem[key] : string.Empty;
                            PacienteColumnas pacientecolumna;
                            if (Enum.TryParse<PacienteColumnas>(key, true, out pacientecolumna))
                            {
                                switch (pacientecolumna)//((PacienteColumnas)Enum.Parse(typeof(PacienteColumnas), key.ToLower()))
                                {
                                    case PacienteColumnas.Nro_documento:
                                        if (svalue.Length == 8)
                                        {
                                            paciente.NroDocumento = svalue;
                                        }
                                        else
                                        {
                                            paciente.NroDocumento = svalue.PadLeft(12, '0');
                                        }
                                        break;
                                    case PacienteColumnas.fechaobtencion:
                                        if (!string.IsNullOrWhiteSpace(svalue))
                                        {
                                            DateTime dt;
                                            var parsed = DateTime.TryParse(svalue, out dt);
                                            if (parsed) paciente.FechaObtencion = dt;
                                        }
                                        break;
                                    case PacienteColumnas.fecharecepcionenrom:
                                        if (!string.IsNullOrWhiteSpace(svalue))
                                        {
                                            DateTime dt;
                                            var parsed = DateTime.TryParse(svalue, out dt);
                                            if (parsed) paciente.FechaRecepcionRom = dt;
                                        }
                                        break;
                                    case PacienteColumnas.renipresestablecimientoorigen:
                                        if (!string.IsNullOrWhiteSpace(svalue))
                                        {
                                            paciente.EstablecimientoOrigen = "991";
                                        }
                                        break;
                                    case PacienteColumnas.renipresestablecimientodeenvio:
                                        if (!string.IsNullOrWhiteSpace(svalue))
                                        {
                                            paciente.EstablecimientoOrigen = "991";
                                        }
                                        break;
                                    //case PacienteColumnas.apellidopaterno:
                                    //    paciente.ApellidoPaterno = svalue;
                                    //    break;
                                    //case PacienteColumnas.apellidomaterno:
                                    //    paciente.ApellidoMaterno = svalue;
                                    //    break;
                                    //case PacienteColumnas.nombres:
                                    //    paciente.Nombres = svalue;
                                    //    break;
                                    //case PacienteColumnas.tipodocumento:
                                    //    paciente.tipoDocumen = svalue;
                                    //    if (!string.IsNullOrWhiteSpace(svalue) && svalue.Trim().ToLower() == "dni")
                                    //    {
                                    //        paciente.TipoDocumento = 1;
                                    //    }
                                    //    else if (!string.IsNullOrWhiteSpace(svalue) && svalue.Trim().ToLower() == "carnet de extranjeria")
                                    //    {
                                    //        paciente.TipoDocumento = 2;
                                    //    }

                                    //    break;
                                    //case PacienteColumnas.nrodocumento:
                                    //    if(!string.IsNullOrWhiteSpace(paciente.tipoDocumen) && paciente.tipoDocumen.Trim().ToLower() == "dni")
                                    //    {
                                    //        paciente.NroDocumento = svalue;
                                    //    }
                                    //    else if (!string.IsNullOrWhiteSpace(paciente.tipoDocumen) && paciente.tipoDocumen.Trim().ToLower() == "carnet de extranjeria")
                                    //    {
                                    //        paciente.NroDocumento = svalue.PadLeft(12, '0');
                                    //    }

                                    //    break;
                                    //case PacienteColumnas.direccion:
                                    //    paciente.DireccionActual = svalue;
                                    //    break;
                                    //case PacienteColumnas.cel1:
                                    //    paciente.Celular1 = svalue;
                                    //    break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }

                //paciente.TipoDocumento = 2;
                //paciente.Genero = paciente.Nombres.ToLower() == "monica" ? 2 : 1;
                if (paciente.TipoDocumento.HasValue && !string.IsNullOrWhiteSpace(paciente.NroDocumento) && paciente.TipoDocumento == 1)
                {
                    pacientes.Add(paciente);
                }
            }

            /*
            * 22536	DIRISLES	DIRIS LIMA ESTE     150111
            * 22540	DIRISLNO	DIRIS LIMA NORTE    150101
            * 22538	DIRISLCE	DIRIS LIMA CENTRO   150101
            * 22533	DIRISLSU	DIRIS LIMA SUR      150104
            */
            PacienteBl pacienteBl = new PacienteBl();
            //var count = pacientes.Count(x => !string.IsNullOrWhiteSpace(x.NroDocumento));
            List<string> nroDocumentoNoEncontrados = new List<string>();
            List<Paciente> pacientesregistrados = new List<Paciente>();
            long contador = 0;
            foreach (var paciente in pacientes)
            {
                if (!string.IsNullOrWhiteSpace(paciente.tipoDocumen) && paciente.TipoDocumento.HasValue)
                {
                    List<Paciente> pacienteList = pacienteBl.GetPacientesByTipoNroDocumento(paciente.TipoDocumento.Value, paciente.NroDocumento);
                    if (!pacienteList.Any())
                    {
                        if (paciente.TipoDocumento == 1)
                        {
                            var reniecPaciente = pacienteBl.getReniec(paciente);
                            if (reniecPaciente != null)//si existe en reniec
                            {
                                reniecPaciente.IdUsuarioRegistro = idUsuarioRegistro;
                                reniecPaciente.TipoDocumento = 1;
                                reniecPaciente.UbigeoActual = new Ubigeo { Id = reniecPaciente.UbigeoReniec.Id };
                                reniecPaciente.IdPaciente = pacienteBl.InsertPaciente(reniecPaciente);
                                contador++;
                            }
                        }
                        //else
                        //{
                        //    var ubigeo = new Ubigeo { Id = "" };
                        //    paciente.IdUsuarioRegistro = idUsuarioRegistro;
                        //    paciente.TipoDocumento = 2;
                        //    paciente.UbigeoActual = ubigeo;
                        //    paciente.UbigeoReniec = ubigeo;
                        //    paciente.IdPaciente = pacienteBl.InsertPaciente(paciente);
                        //}
                    }
                    else
                    {
                        pacientesregistrados.Add(paciente);
                    }
                }
            }

            var contadortotal = contador;

            var testt = string.Join(",", nroDocumentoNoEncontrados);
        }

        public void CrearOrdenes(string file, int idUsuarioRegistro)
        {
            ExcelImportHelpers helper = new ExcelImportHelpers();
            List<PacienteOrdenExcel> pacientes = new List<PacienteOrdenExcel>();
            var datos = helper.ObtenerDatosPorHoja(file, "nuevospacientes");
            foreach (var rowItem in datos.Item1)
            {
                var paciente = new PacienteOrdenExcel();
                for (int i = 0; i < datos.Item2.Count; i++)
                {
                    var key = datos.Item2[i];
                    string svalue = string.Empty;
                    if (rowItem.Count > i)
                    {
                        svalue = !string.IsNullOrEmpty(rowItem[key]) ? rowItem[key] : string.Empty;
                        PacienteOrdenColumnas pacientecolumna;
                        if (Enum.TryParse<PacienteOrdenColumnas>(key, true, out pacientecolumna))
                        {
                            switch (pacientecolumna)//((PacienteColumnas)Enum.Parse(typeof(PacienteColumnas), key.ToLower()))
                            {
                                case PacienteOrdenColumnas.tipodocumento:
                                    //paciente.tipoDocumen = svalue;
                                    if (!string.IsNullOrWhiteSpace(svalue) && svalue.Trim().ToLower() == "dni")
                                    {
                                        paciente.TipoDocumento = 1;
                                    }
                                    //else if (!string.IsNullOrWhiteSpace(svalue) && svalue.Trim().ToLower() == "carnet de extranjeria")
                                    //{
                                    //    paciente.TipoDocumento = 2;
                                    //}

                                    break;
                                case PacienteOrdenColumnas.nrodocumento:
                                case PacienteOrdenColumnas.nro_documento:
                                    if (paciente.TipoDocumento == 1)
                                    {
                                        paciente.NroDocumento = svalue;
                                    }
                                    //else if (paciente.TipoDocumento == 2)
                                    //{
                                    //    paciente.NroDocumento = svalue.PadLeft(12, '0');
                                    //}

                                    break;
                                case PacienteOrdenColumnas.fechaobtencionmuestras:
                                case PacienteOrdenColumnas.fechaobtencion:
                                    if (!string.IsNullOrWhiteSpace(svalue))
                                    {
                                        DateTime dt;
                                        var parsed = DateTime.TryParse(svalue, out dt);
                                        if (parsed) paciente.FechaObtencionMuestras = dt;
                                    }
                                    break;
                                case PacienteOrdenColumnas.fecharecepcionenrom:
                                    if (!string.IsNullOrWhiteSpace(svalue))
                                    {
                                        DateTime dt;
                                        var parsed = DateTime.TryParse(svalue, out dt);
                                        if (parsed) paciente.FechaRecepcionRom = dt;
                                    }
                                    break;
                                case PacienteOrdenColumnas.renipresestablecimientoorigen:
                                    paciente.RenipresEstablecimientoOrigen = svalue;
                                    break;
                                case PacienteOrdenColumnas.renipresestablecimientodeenvio:
                                    paciente.RenipressEstablecimientoEnvio = svalue;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }

                //paciente.TipoDocumento = 2;
                //paciente.Genero = paciente.Nombres.ToLower() == "monica" ? 2 : 1;
                if (!string.IsNullOrWhiteSpace(paciente.NroDocumento) && paciente.TipoDocumento == 1 && paciente.FechaObtencionMuestras.HasValue)
                {
                    pacientes.Add(paciente);
                }
            }

            int idEnfermedad = int.Parse(ConfigurationManager.AppSettings["EnfVirusRespiratorio"]);
            Guid idExamen = Guid.Parse(ConfigurationManager.AppSettings["Covid19"]);
            var idTipoMuestra = int.Parse(ConfigurationManager.AppSettings["idTipoMuestra"]);
            foreach (var pacienteItem in pacientes)
            {
                try
                {
                    var oOrden = new OrdenBl();
                    var orden = new Orden();
                    var pacienteBl = new PacienteBl();
                    var examenBl = new ExamenBl();
                    var establecimientoBl = new EstablecimientoBl();
                    List<Paciente> pacienteList = pacienteBl.GetPacientesByTipoNroDocumento(pacienteItem.TipoDocumento, pacienteItem.NroDocumento);
                    var LstxOrdenMuestraLinealkobo = new List<OrdenMuestraLinealkobo>();
                    if (pacienteList.Any())
                    {
                        bool continuar = true;
                        var paciente = pacienteList.First();
                        var codigoOrdenGenerado = string.Empty;
                       
                        //Orden ordenexistente = oOrden.ObtenerIdOrdenPorDocumentoPaciente(paciente.TipoDocumento.Value, paciente.NroDocumento);
                        //if (orden.idOrden != Guid.Empty)//si idOrden != guid.empty es porque ya existe una orden con observacion = 'registro odk'
                        //{
                        //    //entrar para verificar que esa orden tenga el examen
                        //    continuar = !oOrden.ValidarExisteOrdenExamen(orden.idOrden, idExamen);
                        //}

                        if (continuar)
                        {
                            var enfermedadListAgregados = new List<Enfermedad>();
                            var ordenExamenListAgregados = new List<OrdenExamen>();
                            var ordenMaterialListAgregados = new List<OrdenMaterial>();
                            var ordenMuestraListAgregados = new List<OrdenMuestra>();

                            //codigos de establecimiento de DIRIS Lima Centro/Sur/Norte
                            /*
                             * 22536	DIRISLES	DIRIS LIMA ESTE     150111
                             * 22540	DIRISLNO	DIRIS LIMA NORTE    150101
                             * 22538	DIRISLCE	DIRIS LIMA CENTRO   150101
                             * 22533	DIRISLSU	DIRIS LIMA SUR      150104
                             */
                            var oEstablecimiento = oOrden.GetEstablecimientobyCodigoUnico(string.IsNullOrEmpty(pacienteItem.RenipresEstablecimientoOrigen) ? "10000R16" : "0");
                            var oEstablecimientoDestino = oOrden.GetEstablecimientobyCodigoUnico(string.IsNullOrEmpty(pacienteItem.RenipressEstablecimientoEnvio) ? "10000R16" : "0");

                            orden.idEstablecimiento = oEstablecimiento.IdEstablecimiento;
                            orden.IdUsuarioRegistro = idUsuarioRegistro;
                            orden.IdLaboratorioDestino = 995;
                            orden.Paciente = new PacienteBl().getPacienteById(paciente.IdPaciente);
                            orden.solicitante = "0";//Falta asignar al solicitante
                            orden.idEstablecimientoEnvio = oEstablecimientoDestino.IdEstablecimiento;//
                            using (var ordenBl = new OrdenBl())
                            {
                                //Generar Codigo de Orden
                                codigoOrdenGenerado = ordenBl.GenerarCodigoOrden(oEstablecimiento.IdEstablecimiento);
                            }
                            var laboratorio = new Laboratorio() { IdLaboratorio = 991, CodigoUnico = "10000R16" };

                            var examenList = new Examen();
                            var tipoMuestraList = new List<TipoMuestra>();
                            var materialList = new List<Material>();

                            var LstoExamen = new List<OrdenExamen>();
                            orden.ordenMuestraList = new List<OrdenMuestra>();
                            orden.ordenMuestraList.Add(new OrdenMuestra
                            {
                                idTipoMuestra = idTipoMuestra,
                                TipoMuestra = new TipoMuestra { idTipoMuestra = idTipoMuestra },
                                fechaColeccion = pacienteItem.FechaObtencionMuestras.Value,//
                                horaColeccion = DateTime.Now,
                                idProyecto = 1,
                                MuestraCodificacion = new MuestraCodificacion()
                            });

                            var ordexmen = new OrdenExamen
                            {
                                idEnfermedad = idEnfermedad,
                                Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                idExamen = idExamen,
                                Examen = new Examen { idExamen = idExamen },
                                IdTipoMuestra = idTipoMuestra,
                                ordenMuestraList = orden.ordenMuestraList
                            };
                            LstoExamen.Add(ordexmen);
                            orden.ordenExamenList = LstoExamen;

                            orden.ordenMaterialList = new List<OrdenMaterial>();

                            //ORDENMATERIAL
                            var materiales = new MaterialBl().GetMaterialesByIdTipoMuestra(idTipoMuestra);
                            var material = materiales.FirstOrDefault();
                            if (material != null)
                            {
                                orden.ordenMaterialList.Add(new OrdenMaterial
                                {
                                    cantidad = 1,
                                    idMaterial = material.idMaterial,
                                    Material = new Material { idMaterial = material.idMaterial, TipoMuestra = new TipoMuestra { idTipoMuestra = idTipoMuestra } },
                                    fechaEnvio = DateTime.Today,
                                    horaEnvio = DateTime.Now,
                                    volumenMuestraColectada = material != null ? material.volumen : 1,
                                    OrdenExamen = ordexmen,
                                    Laboratorio = laboratorio
                                });
                            }
                            List<OrdenDatoClinico> ordenDatoClinicoList = new List<OrdenDatoClinico>();
                            var datoClinicoBl = new DatoClinicoBl();
                            var categoriaDatoList = datoClinicoBl.GetCategoriaByEnfermedad(idEnfermedad, 1, idExamen.ToString());
                            var oEnfermedad = new Enfermedad();
                            oEnfermedad.idEnfermedad = idEnfermedad;
                            oEnfermedad.categoriaDatoList = categoriaDatoList;
                            enfermedadListAgregados.Add(oEnfermedad);
                            {
                                var datosClinicos =
                                       enfermedadListAgregados.SelectMany(e => e.categoriaDatoList)
                                           .SelectMany(c => c.OrdenDatoClinicoList);
                                foreach (var ordenDatoClinico in datosClinicos)
                                {
                                    var id = ordenDatoClinico.Enfermedad.idEnfermedad.ToString() +
                                             ordenDatoClinico.Dato.IdDato.ToString();

                                    OrdenDatoClinico ordenDatoClinicoFormulario =
                                        ordenDatoClinicoList.FirstOrDefault(x => x.Dato.IdDato == ordenDatoClinico.Dato.IdDato);

                                    //Si el dato clinico existe previamente en el formulario entonces se copia los valores del existente
                                    if (ordenDatoClinicoFormulario != null)
                                    {
                                        ordenDatoClinico.noPrecisa = ordenDatoClinicoFormulario.noPrecisa;
                                        ordenDatoClinico.ValorString = ordenDatoClinicoFormulario.ValorString;
                                    }
                                    else
                                    {
                                        //Si el dato clinico no existe se crea con los valores obtenidos
                                        ordenDatoClinicoFormulario = new OrdenDatoClinico();
                                        Dato datoClinico = new Dato();
                                        datoClinico.IdDato = ordenDatoClinico.Dato.IdDato;
                                        ordenDatoClinicoFormulario.Dato = datoClinico;
                                        ordenDatoClinicoList.Add(ordenDatoClinicoFormulario);


                                        var formValue = "";
                                        string checknoprecisa = "";
                                        var checkNoPrecisaBoolean = !string.IsNullOrEmpty(checknoprecisa) && (checknoprecisa.ToLower() == "on" || checknoprecisa.ToLower() == "true");

                                        if ((int)Enums.TipoCampo.CHECKBOX == ordenDatoClinico.Dato.IdTipo
                                            || (int)Enums.TipoCampo.COMBO == ordenDatoClinico.Dato.IdTipo)
                                        {
                                            ordenDatoClinico.noPrecisa = formValue == null || formValue.Equals("0");
                                            ordenDatoClinico.ValorString = formValue == null || formValue.Equals("0")
                                                ? ""
                                                : formValue;
                                        }
                                        else
                                        {
                                            ordenDatoClinico.noPrecisa = checkNoPrecisaBoolean;
                                            ordenDatoClinico.ValorString = !checkNoPrecisaBoolean ? formValue : string.Empty;
                                        }
                                        ordenDatoClinicoFormulario.noPrecisa = ordenDatoClinico.noPrecisa;
                                        ordenDatoClinicoFormulario.ValorString = ordenDatoClinico.ValorString;
                                    }
                                }
                            }
                            orden.enfermedadList = enfermedadListAgregados;
                            orden.estatus = 1;
                            orden.IdUsuarioRegistro = idUsuarioRegistro;
                            orden.fechaSolicitud = DateTime.Parse(DateTime.Now.ToShortDateString());

                            /////se mantendrá el mismo codigo de observacion?
                            orden.observacion = "Registro masivo muestras desde excel";
                            orden.Proyecto = new Proyecto
                            {
                                IdProyecto = 1
                            };
                            oOrden.InsertOrden(orden, Enums.TipoRegistroOrden.SOLO_ORDEN);
                            var OrdenMuestraLinealkobo = new OrdenMuestraLinealkobo()
                            {
                                codigoOrden = orden.codigoOrden,
                                codigoLineal = orden.ordenMuestraList.FirstOrDefault().MuestraCodificacion.codificacionLineal,
                                codigoMuestra = orden.ordenMuestraList.FirstOrDefault().MuestraCodificacion.codificacion,
                                dni = orden.Paciente.NroDocumento,
                                apepat = orden.Paciente.ApellidoPaterno,
                                apemat = orden.Paciente.ApellidoMaterno,
                                nombre = orden.Paciente.Nombres
                            };
                            LstxOrdenMuestraLinealkobo.Add(OrdenMuestraLinealkobo);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void AgregarOrdenExamen(string file, int idUsuarioRegistro)
        {
            ExcelImportHelpers helper = new ExcelImportHelpers();
            var ordenbl = new OrdenBl();
            List<PacienteOrdenExcel> pacientes = new List<PacienteOrdenExcel>();
            var datos = helper.ObtenerDatosPorHoja(file, "nuevospacientes");
            foreach (var rowItem in datos.Item1)
            {
                var paciente = new PacienteOrdenExcel();
                for (int i = 0; i < datos.Item2.Count; i++)
                {
                    var key = datos.Item2[i];
                    string svalue = string.Empty;
                    if (rowItem.Count > i)
                    {
                        svalue = !string.IsNullOrEmpty(rowItem[key]) ? rowItem[key] : string.Empty;
                        PacienteOrdenColumnas pacientecolumna;
                        if (Enum.TryParse<PacienteOrdenColumnas>(key, true, out pacientecolumna))
                        {
                            switch (pacientecolumna)//((PacienteColumnas)Enum.Parse(typeof(PacienteColumnas), key.ToLower()))
                            {
                                case PacienteOrdenColumnas.tipodoc:
                                    paciente.TipoDoc = svalue;
                                    if (!string.IsNullOrWhiteSpace(svalue))
                                    {
                                        if (svalue.Trim().ToLower() == "dni")
                                        {
                                            paciente.TipoDocumento = 1;
                                        }
                                        else if (svalue.Trim().ToLower() == "carnet de extranjeria")
                                        {
                                            paciente.TipoDocumento = 2;
                                        }
                                    }
                                    break;
                                case PacienteOrdenColumnas.nrodocumento:
                                    if (paciente.TipoDocumento == 2)
                                    {
                                        paciente.NroDocumento = svalue.PadLeft(12, '0');
                                    }
                                    else if (paciente.TipoDocumento == 1)
                                    {
                                        paciente.NroDocumento = svalue;
                                    }

                                    break;
                                //case PacienteOrdenColumnas.establecimientoorigen:
                                //    paciente.NroDocumento = svalue;
                                //    break;
                                case PacienteOrdenColumnas.fechaobtencionmuestras:
                                    if (!string.IsNullOrWhiteSpace(svalue))
                                    {
                                        DateTime dt;
                                        var parsed = DateTime.TryParse(svalue, out dt);
                                        if (parsed) paciente.FechaObtencionMuestras = dt;
                                    }
                                    break;
                                case PacienteOrdenColumnas.establecimientoorigen:
                                    paciente.EstablecimientoOrigen = svalue;
                                    break;
                                case PacienteOrdenColumnas.resultadoglosa:
                                    /*
                                        2576		NEGATIVO
                                        2577		IgM REACTIVO
                                        2578		IgG REACTIVO
                                        2579		IgM e IgG REACTIVO
                                        2580		INVALIDO
                                     */
                                    paciente.ResultadoGlosa = svalue;
                                    if (!string.IsNullOrWhiteSpace(svalue))
                                    {
                                        if (svalue.Trim().ToLower() == "negativo")
                                        {
                                            paciente.IdAnalitoOpcionResultado = 2576;
                                        }
                                        else if (svalue.Trim().ToLower() == "igm e igg positivo")
                                        {
                                            paciente.IdAnalitoOpcionResultado = 2579;
                                        }
                                        else if (svalue.Trim().ToLower() == "igm positivo")
                                        {
                                            paciente.IdAnalitoOpcionResultado = 2577;
                                        }
                                        else if (svalue.Trim().ToLower() == "igg positivo")
                                        {
                                            paciente.IdAnalitoOpcionResultado = 2578;
                                        }
                                        else if (svalue.Trim().ToLower() == "indeterminado/no valido")
                                        {
                                            paciente.IdAnalitoOpcionResultado = 2580;
                                        }
                                    }
                                    break;
                                case PacienteOrdenColumnas.usuarioejecutordni:
                                    paciente.UsuarioEjecutorDNI = svalue;
                                    break;
                                case PacienteOrdenColumnas.usuarioejecutornombres:
                                    paciente.UsuarioEjecutorNombres = svalue;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }

                if (paciente.FechaObtencionMuestras.HasValue && paciente.IdAnalitoOpcionResultado != 0)// && !string.IsNullOrWhiteSpace(paciente.EstablecimientoOrigen))
                {
                    pacientes.Add(paciente);
                }
            }

            int idEnfermedad = 1005676;//1005680;//PROD
            Guid idExamen = Guid.Parse("09147444-3411-4545-BC5B-89F6BB949293"); //Guid.Parse("CE4DC7E1 -1D85-4065-8C49-2B833D4CEEF9");//PROD
            //test QA
            /*
             4060
            4061
            4062
            4063
            4064
             */
            pacientes = new List<PacienteOrdenExcel>()
            {
                new PacienteOrdenExcel{ TipoDocumento = 1, NroDocumento = "45119877", UsuarioEjecutorDNI = "44043299", UsuarioEjecutorNombres="jose chavez", FechaObtencionMuestras = DateTime.Now, IdAnalitoOpcionResultado = 4063 }
            };

            foreach (var paciente in pacientes)
            {
                //comprobar que ya tiene una orden creada, con el examenId = 1B2BEC28-772C-49DF-BCC2-85E0C5CCA667, hisopado nasal
                Orden orden = ordenbl.ObtenerIdOrdenPorDocumentoPaciente(paciente.TipoDocumento, paciente.NroDocumento);
                if (orden.idOrden != Guid.Empty)
                {
                    if (!ordenbl.ValidarExisteOrdenExamen(orden.idOrden, idExamen))//verificar si la orden ya tiene el segundo examenId = CE4DC7E1-1D85-4065-8C49-2B833D4CEEF9, prueba rapida
                    {
                        var laboratorio = new Laboratorio() { IdLaboratorio = orden.ordenMaterialList.First().Laboratorio.IdLaboratorio, CodigoUnico = "10000R16" };

                        //var tipoMuestraList = new List<TipoMuestra>();
                        //tipoMuestraList = new TipoMuestraBl().GetTiposMuestraByIdExamen(idExamen).ToList();
                        //var idTipoMuestra = tipoMuestraList.FirstOrDefault().idTipoMuestra;
                        var idTipoMuestra = 1; //sangre

                        //var materialList = new List<Material>();
                        //materialList = new MaterialBl().GetMaterialesByIdTipoMuestra(tipoMuestraList.FirstOrDefault().idTipoMuestra).ToList();

                        var LstoExamen = new List<OrdenExamen>();
                        var ordexmn = new OrdenExamen
                        {
                            idEnfermedad = idEnfermedad,
                            Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                            idExamen = idExamen,
                            Examen = new Examen { idExamen = idExamen },
                            IdTipoMuestra = idTipoMuestra
                        };
                        LstoExamen.Add(ordexmn);
                        orden.ordenExamenList = LstoExamen;

                        orden.ordenMuestraList = new List<OrdenMuestra>();
                        orden.ordenMuestraList.Add(new OrdenMuestra
                        {
                            idTipoMuestra = idTipoMuestra,
                            TipoMuestra = new TipoMuestra { idTipoMuestra = idTipoMuestra },
                            fechaColeccion = paciente.FechaObtencionMuestras.Value,//
                            horaColeccion = DateTime.Now,
                            idProyecto = 1,
                            MuestraCodificacion = new MuestraCodificacion()
                        });

                        //ORDENMATERIAL
                        /*
                         * 
                         * idMaterial	idPresentacion	glosa	idReactivo	glosaReactivo	idTipoMuestra	nombre	volumen
                            109	            56	        MUESTRA	    0	        -	            1	        SANGRE	10.00
                         * 
                         */
                        orden.ordenMaterialList = new List<OrdenMaterial>();
                        //var materiales = new MaterialBl().GetMaterialesByIdTipoMuestra(idTipoMuestra);
                        //var material = materiales.FirstOrDefault();
                        //if (material != null)
                        //{
                        orden.ordenMaterialList.Add(new OrdenMaterial
                        {
                            cantidad = 1,
                            idMaterial = 109,//material.idMaterial,
                                             //Material = new Material { idMaterial = material.idMaterial, TipoMuestra = new TipoMuestra { idTipoMuestra = idTipoMuestra } },// 1 = sangre
                            Material = new Material { idMaterial = 109, TipoMuestra = new TipoMuestra { idTipoMuestra = idTipoMuestra } },// 1 = sangre
                            fechaEnvio = DateTime.Today,
                            horaEnvio = DateTime.Now,
                            //volumenMuestraColectada = material != null ? material.volumen : 1,
                            volumenMuestraColectada = 1,
                            OrdenExamen = ordexmn,
                            Laboratorio = laboratorio,
                            Tipo = 1
                        });
                        //}

                        var preOrdenDatoClinico = new OrdenDatoClinico
                        {
                            FechaRegistro = DateTime.Now,
                            Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad }
                        };
                        List<CategoriaDato> categoriaDatoList = new List<CategoriaDato>();

                        //CONFIRMAR
                        //categoriaDatoList.Add(new CategoriaDato
                        //{
                        //    IdCategoriaDato = 172
                        //});

                        //categoriaDatoList.Add(new CategoriaDato
                        //{
                        //    IdCategoriaDato = 171
                        //});

                        orden.Proyecto = new Proyecto
                        {
                            IdProyecto = 1
                        };

                        orden.enfermedadList = new List<Enfermedad>();

                        //OrdenMuestra, OrdenExamenOrdenMuestra, OrdenDatoCLinico, OrdenExamen, OrdenResultadoAnalito
                        ordenbl.InsertarOrdenNuevo(orden, Enums.TipoRegistroOrden.SOLO_ORDEN, paciente.IdAnalitoOpcionResultado);
                    }

                    ordenbl.InsertarOrdenEjecutorPruebaRapida(orden.idOrden, paciente.UsuarioEjecutorDNI, paciente.UsuarioEjecutorNombres);
                }
            }
        }

        public class PacienteOrdenExcel
        {
            public int TipoDocumento { get; set; }
            public string NroDocumento { get; set; }
            public string EstablecimientoOrigen { get; set; }
            public DateTime? FechaObtencionMuestras { get; set; }
            public string TipoDoc { get; set; }
            public string ResultadoGlosa { get; set; }
            public int IdAnalitoOpcionResultado { get; set; }
            public string UsuarioEjecutorDNI { get; set; }
            public string UsuarioEjecutorNombres { get; set; }
            public DateTime? FechaRecepcionRom { get; set; }
            public string RenipresEstablecimientoOrigen { get; set; }
            public string RenipressEstablecimientoEnvio { get; set; }
            public PacienteOrdenExcel()
            {
                TipoDocumento = 0;
            }
        }

        public enum PacienteOrdenColumnas
        {
            tipodocumento,
            nrodocumento,
            establecimientoorigen,
            fechaobtencionmuestras,
            tipodoc,
            resultadoglosa,
            usuarioejecutordni,
            usuarioejecutornombres,
            //
            nro_documento,
            fechaobtencion,
            fecharecepcionenrom,
            renipresestablecimientoorigen,
            renipresestablecimientodeenvio
        }
    }
}
