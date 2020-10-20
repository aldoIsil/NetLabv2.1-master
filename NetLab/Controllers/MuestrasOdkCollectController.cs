using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayer;
using BusinessLayer.DatoClinico;
using DataLayer;
using Enums;
using Model;
using PagedList;

namespace NetLab.Controllers
{
    public class MuestrasOdkCollectController : Controller
    {
        // GET: MuestrasOdkCollect
        public ActionResult Index()
        {
            var MuestraODK = GetDatosMuestrasODK();
            Session["Odk"] = MuestraODK;
            return View(MuestraODK.ToPagedList(1, 5000));
        }
        public List<MuestrasODKCoronavirus> GetDatosMuestrasODK()
        {
            return new List<MuestrasODKCoronavirus>();//(new OrdenBl().GetOrdenesConsultaExternaODK());
        }
        public void ProcesarDatosODK()
        {
            var ListaDatosODK = (List<MuestrasODKCoronavirus>)Session["Odk"];
            //1.-leer el registro ODK
            try
            {
                foreach (var item in ListaDatosODK)
                {
                    Model.Paciente nPaciente = new Model.Paciente();
                    var pacienteBl = new PacienteBl();
                    var oDatoRegistro = new OrdenBl();
                    var oPacienteRegistro = new PacienteBl();
                    //1.- Validar datos Obligatorios / Retornar errores
                    //AVISAR A WILLY QUE EL UBIGEO LO SAQUE DE LA TABLA UBIGEO PACIENTE
                    //2.- Validar DNI del paciente y DNI del solicitantes: En base local sino en base reniec
                    //Paciente - Local
                    var tipoDocumento = -1;
                    var pacienteList = new List<Paciente>();
                    var paciente = new Paciente();
                    switch (item.tipo_doc)
                    {
                        case "dni":
                            tipoDocumento = 1;
                            pacienteList = pacienteBl.getPacientes(1, 10000, tipoDocumento, item.nro_documento, "", "");
                            if (pacienteList.Count == 0)
                            {
                                paciente.NroDocumento = item.nro_documento;
                                paciente.IdUsuarioRegistro = 72;
                                paciente.ocupacion = item.ocupacion;
                                paciente = pacienteBl.getReniec(paciente);
                                if (paciente != null)
                                {
                                    paciente.UbigeoActual = new Ubigeo { Id = item.direccion_pac };
                                    paciente.estatus = 1;
                                    paciente.TipoDocumento = tipoDocumento;
                                    paciente.Estado = 0;
                                    paciente.IdPaciente = oPacienteRegistro.InsertPaciente(paciente);
                                }
                            }
                            else
                            {
                                paciente.IdPaciente = pacienteList.Where(c => c.NroDocumento == item.nro_documento).FirstOrDefault().IdPaciente;
                            }
                            break;
                        case "pasaporte":
                            tipoDocumento = 2;
                            pacienteList = pacienteBl.getPacientes(1, 10000, tipoDocumento, item.nro_documento, "", "");
                            if (pacienteList.Count == 0)
                            {
                                paciente.NroDocumento = item.nro_documento;
                                paciente.Celular1 = item.celular_pac;
                                paciente.ocupacion = item.ocupacion;
                                paciente.Nombres = item.ape_nom_paciente;
                                paciente.ApellidoMaterno = item.ape_mat_paciente;
                                paciente.ApellidoPaterno = item.ape_pat_paciente;
                                paciente.Genero = item.Sexo_pac == "masculino" ? 1 : 2;
                                paciente.correoElectronico = item.email_pc;
                                paciente.TipoDocumento = tipoDocumento;
                                paciente.UbigeoReniec = new Ubigeo() { Id = item.cod_dist_pac };
                                paciente.UbigeoActual = new Ubigeo() { Id = item.cod_dist_pac };
                                paciente.DireccionActual = item.direccion_pac;
                                paciente.DireccionReniec = item.direccion_pac;
                                paciente.mcaDatoTutor = 0;
                                paciente.tipoSeguroSalud = 0;
                                paciente.estatus = 1;
                                paciente.IdUsuarioRegistro = 72;
                                paciente.IdPaciente = oPacienteRegistro.InsertPaciente(paciente);
                            }
                            else
                            {
                                paciente.IdPaciente = pacienteList.Where(c => c.NroDocumento == item.nro_documento).FirstOrDefault().IdPaciente;
                            }
                            break;
                        case "ce":
                            tipoDocumento = 3;
                            pacienteList = pacienteBl.getPacientes(1, 10000, tipoDocumento, item.nro_documento, "", "");
                            if (pacienteList.Count == 0)
                            {
                                paciente.NroDocumento = item.nro_documento;
                                paciente.Celular1 = item.celular_pac;
                                paciente.ocupacion = item.ocupacion;
                                paciente.Nombres = item.ape_nom_paciente;
                                paciente.ApellidoMaterno = item.ape_mat_paciente;
                                paciente.ApellidoPaterno = item.ape_pat_paciente;
                                paciente.Genero = item.Sexo_pac == "masculino" ? 1 : 2;
                                paciente.correoElectronico = item.email_pc;
                                paciente.TipoDocumento = tipoDocumento;
                                paciente.UbigeoReniec = new Ubigeo() { Id = item.cod_dist_pac };
                                paciente.UbigeoActual = new Ubigeo() { Id = item.cod_dist_pac };
                                paciente.DireccionActual = item.direccion_pac;
                                paciente.DireccionReniec = item.direccion_pac;
                                paciente.mcaDatoTutor = 0;
                                paciente.tipoSeguroSalud = 0;
                                paciente.estatus = 1;
                                paciente.IdUsuarioRegistro = 72;
                                paciente.IdPaciente = oPacienteRegistro.InsertPaciente(paciente);
                            }
                            else
                            {
                                paciente.IdPaciente = pacienteList.Where(c => c.NroDocumento == item.nro_documento).FirstOrDefault().IdPaciente;
                            }
                            break;
                        default:
                            break;
                    }
                    //Solicitante - Local
                    //  Falta comprobar el solicitante.


                    // Registrar Orden, CON FÉ!!!
                    try
                    {
                        var oOrden = new OrdenBl();
                        var orden = new Orden();
                        var enfermedadListAgregados = new List<Enfermedad>();
                        var ordenExamenListAgregados = new List<OrdenExamen>();
                        var ordenMaterialListAgregados = new List<OrdenMaterial>();
                        var ordenMuestraListAgregados = new List<OrdenMuestra>();

                        var oEstablecimiento = new Establecimiento();// oOrden.GetEstablecimientobyCodigoUnico(string.IsNullOrEmpty(item.cod_renipres) ? "" : "0");
                        orden.idEstablecimiento = oEstablecimiento.IdEstablecimiento;
                        orden.EstablecimientoCodigoUnico = oEstablecimiento.CodigoUnico;
                        orden.IdUsuarioRegistro = 72;//WebService OGTI eqhali
                        orden.Paciente = new PacienteBl().getPacienteById(paciente.IdPaciente);
                        orden.solicitante = "0";//Falta asignar al solicitante
                        
                        int idEnfermedad = 1005680;
                        var examen = item.tipo_muestra.Split(',');
                        var laboratorio = new Laboratorio() { IdLaboratorio = 995, CodigoUnico = "10000R16" };
                        var enfermedad = new Enfermedad() { idEnfermedad = idEnfermedad };
                        var enfermedadList = new List<Enfermedad>();
                        foreach (var itemExamen in examen)
                        {
                            var existeEnfermedad = false;
                            var existeMuestra = false;
                            var duplicado = false;
                            var examenList = new Examen();
                            var tipoMuestraList = new List<TipoMuestra>();
                            var materialList = new List<Material>();
                            //
                            tipoMuestraList = new TipoMuestraBl().GetTiposMuestraByIdExamen(Guid.Parse(itemExamen)).ToList();
                            var idTipoMuestra = tipoMuestraList.FirstOrDefault().idTipoMuestra;
                            materialList = new MaterialBl().GetMaterialesByIdTipoMuestra(tipoMuestraList.FirstOrDefault().idTipoMuestra).ToList();
                            var LstoExamen = new List<OrdenExamen>();
                            LstoExamen.Add(new OrdenExamen
                            {
                                idEnfermedad = idEnfermedad,
                                Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                idExamen = Guid.Parse(item.tipo_muestra),
                                Examen = new Examen { idExamen = Guid.Parse(item.tipo_muestra) },
                                IdTipoMuestra = idTipoMuestra
                            });
                            orden.ordenExamenList = LstoExamen;
                            orden.ordenMuestraList = new List<OrdenMuestra>();
                            orden.ordenMaterialList = new List<OrdenMaterial>();

                            orden.ordenMuestraList.Add(new OrdenMuestra
                            {
                                idTipoMuestra = idTipoMuestra,
                                TipoMuestra = new TipoMuestra { idTipoMuestra = idTipoMuestra },
                                fechaColeccion = DateTime.Parse(item.fec_muestra),
                                horaColeccion = DateTime.Now,
                                idProyecto = 1,
                                MuestraCodificacion = new MuestraCodificacion()
                            });

                            //ORDENMATERIAL
                            var materiales = new MaterialBl().GetMaterialesByIdTipoMuestra(idTipoMuestra);
                            var material = materiales.FirstOrDefault();
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

                            //if (ordenExamen == null)
                            //{
                            //    ordenExamen = new OrdenExamen
                            //    {
                            //        Enfermedad = enfermedad,
                            //        Examen = examenList,
                            //        IdTipoMuestra = idTipoMuestra,
                            //        ordenMuestraList = new List<OrdenMuestra>()
                            //    };
                            //    var ordenMuestra = ordenMuestraListAgregados.FirstOrDefault(x => x.TipoMuestra.idTipoMuestra == idTipoMuestra);
                            //    var tipoMuestra = tipoMuestraList.FirstOrDefault(x => x.idTipoMuestra == idTipoMuestra);
                            //    var ordenDal = new OrdenDal();
                            //    var nroMuestra = 0;
                            //    if (ordenMuestra == null)
                            //    {
                            //        ordenMuestra = new OrdenMuestra
                            //        {
                            //            idOrdenMuestra = Guid.NewGuid(),
                            //            MuestraCodificacion = new MuestraCodificacion(),
                            //            TipoMuestra = tipoMuestra,
                            //            ordenExamenList = new List<OrdenExamen>()
                            //        };
                            //        var fecha = Convert.ToString(DateTime.Parse(item.fec_muestra).ToShortDateString()).Split('/');
                            //        ordenMuestra.fechaColeccion = new DateTime(Convert.ToInt32(fecha[2]), Convert.ToInt32(fecha[1]), Convert.ToInt32(fecha[0]));
                            //        var hora = Convert.ToString(DateTime.Now.ToLocalTime().TimeOfDay).Split(':');
                            //        ordenMuestra.horaColeccion = new DateTime(Convert.ToInt32(fecha[2]), Convert.ToInt32(fecha[1]), Convert.ToInt32(fecha[0]), Convert.ToInt32(hora[0]), Convert.ToInt32(hora[1]), 0);

                            //        ordenMuestraListAgregados.Add(ordenMuestra);
                            //    }
                            //    else
                            //    {
                            //        existeMuestra = true;
                            //    }
                            //    if (ordenExamen.ordenMuestraList.FirstOrDefault(x => x.TipoMuestra.idTipoMuestra == idTipoMuestra) == null)
                            //    {
                            //        ordenExamen.ordenMuestraList.Add(ordenMuestra);
                            //    }
                            //    ordenExamenListAgregados.Add(ordenExamen);
                            //    ordenMuestra.ordenExamenList.Add(ordenExamen);
                            //    var material = new MaterialBl().GetMaterialesByIdTipoMuestra(idTipoMuestra).FirstOrDefault();
                            //    ordenMaterialListAgregados = CreateObjectOrdenMaterial(0, false, 0, 0, material,
                            //                            "", "", ordenMuestra, laboratorio, ordenExamen, ordenMaterialListAgregados, nroMuestra);
                            //}
                        }

                        var preOrdenDatoClinico = new OrdenDatoClinico
                        {
                            FechaRegistro = DateTime.Now,
                            Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad }
                        };
                        List<CategoriaDato> categoriaDatoList = new List<CategoriaDato>();

                        var resPruebaRapida = 546;
                        if (item.prueba_rapida == "si")
                        {
                            resPruebaRapida = 501;
                        }
                        else if (item.prueba_rapida == "no")
                        {
                            resPruebaRapida = 500;
                        }
                        else if (item.prueba_rapida == "indeterminado")
                        {
                            resPruebaRapida = 502;
                        }
                        else
                        {
                            resPruebaRapida = 546;
                        }
                        var lst = new List<OrdenDatoClinico>();
                        lst.Add(new OrdenDatoClinico()
                        {
                            idCategoriaDato = 172,
                            FechaRegistro = DateTime.Now,
                            Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                            ValorString = item.fec_notificacion,
                            noPrecisa = false,
                            Dato = new Dato { IdDato = 553 }
                        });

                        lst.Add(new OrdenDatoClinico
                        {
                            idCategoriaDato = 171,
                            FechaRegistro = DateTime.Now,
                            Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                            ValorString = resPruebaRapida.ToString(),
                            noPrecisa = resPruebaRapida ==  546,
                            Dato = new Dato { IdDato = 551 }
                        });
                        lst.Add(new OrdenDatoClinico
                        {
                            idCategoriaDato = 171,
                            FechaRegistro = DateTime.Now,
                            Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                            ValorString = item.fec_prueba_rapida,
                            noPrecisa = item.fec_prueba_rapida == null,
                            Dato = new Dato { IdDato = 552 }
                        });
                        lst.Add(new OrdenDatoClinico
                        {
                            idCategoriaDato = 172,
                            FechaRegistro = DateTime.Now,
                            Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                            ValorString = item.ocupacion,
                            noPrecisa = item.ocupacion == null,
                            Dato = new Dato { IdDato = 554 }
                        });
                        lst.Add(new OrdenDatoClinico
                        {
                            idCategoriaDato = 172,
                            FechaRegistro = DateTime.Now,
                            Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                            ValorString = item.esta_hipostilizado == "si" ? "1": "0",
                            noPrecisa = item.esta_hipostilizado == null,
                            Dato = new Dato { IdDato = 555 }
                        });
                        lst.Add(new OrdenDatoClinico
                        {
                            idCategoriaDato = 172,
                            FechaRegistro = DateTime.Now,
                            Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                            ValorString = item.fec_hospitalizacion,
                            noPrecisa = item.esta_hipostilizado == null,
                            Dato = new Dato { IdDato = 556 }
                        });
                        var sintomas = item.tiene_sintomas.Split(',');
                        foreach (var sintoma in sintomas)
                        {
                            if (sintoma == "tos")
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = "1",
                                    noPrecisa = item.tiene_sintomas == "no",
                                    Dato = new Dato { IdDato = 557 }
                                });
                            }
                            else
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = null,
                                    noPrecisa = item.tiene_sintomas == "no",
                                    Dato = new Dato { IdDato = 557 }
                                });
                            }
                            if (sintoma == "garganta")
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = "1",
                                    noPrecisa = item.tiene_sintomas == "no",
                                    Dato = new Dato { IdDato = 558 }
                                });
                            }
                            else
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = null,
                                    noPrecisa = item.tiene_sintomas == "no",
                                    Dato = new Dato { IdDato = 558 }
                                });
                            }
                            if (sintoma == "nasal")
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = "1",
                                    noPrecisa = item.tiene_sintomas == "no",
                                    Dato = new Dato { IdDato = 559 }
                                });
                            }
                            else
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = null,
                                    noPrecisa = item.tiene_sintomas == "no",
                                    Dato = new Dato { IdDato = 559 }
                                });
                            }
                            if (sintoma == "respiratoria")
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = "1",
                                    noPrecisa = item.tiene_sintomas == "no",
                                    Dato = new Dato { IdDato = 560 }
                                });
                            }
                            else
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = null,
                                    noPrecisa = item.tiene_sintomas == "no",
                                    Dato = new Dato { IdDato = 560 }
                                });
                            }
                            if (sintoma == "fiebre")
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = "1",
                                    noPrecisa = item.tiene_sintomas == "no",
                                    Dato = new Dato { IdDato = 561 }
                                });
                            }
                            else
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = null,
                                    noPrecisa = item.tiene_sintomas == "no",
                                    Dato = new Dato { IdDato = 561 }
                                });
                            }
                            if (sintoma == "malestar")
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = "1",
                                    noPrecisa = item.tiene_sintomas == "no",
                                    Dato = new Dato { IdDato = 562}
                                });
                            }
                            else
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = null,
                                    noPrecisa = item.tiene_sintomas == "no",
                                    Dato = new Dato { IdDato = 562 }
                                });
                            }
                            if (sintoma == "diarrea")
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = "1",
                                    noPrecisa = item.tiene_sintomas == "no",
                                    Dato = new Dato { IdDato = 563 }
                                });
                            }
                            else
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = null,
                                    noPrecisa = item.tiene_sintomas == "no",
                                    Dato = new Dato { IdDato = 563 }
                                });
                            }
                            if (sintoma == "nauseas")
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = "1",
                                    noPrecisa = item.tiene_sintomas == "no",
                                    Dato = new Dato { IdDato = 564 }
                                });
                            }
                            else
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = null,
                                    noPrecisa = item.tiene_sintomas == "no",
                                    Dato = new Dato { IdDato = 564 }
                                });
                            }
                            if (sintoma == "cefalea")
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = "1",
                                    noPrecisa = item.tiene_sintomas == "no",
                                    Dato = new Dato { IdDato = 565 }
                                });
                            }
                            else
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = null,
                                    noPrecisa = item.tiene_sintomas == "no",
                                    Dato = new Dato { IdDato = 565 }
                                });
                            }
                            if (sintoma == "confusion")
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = "1",
                                    noPrecisa = item.tiene_sintomas == "no",
                                    Dato = new Dato { IdDato = 566 }
                                });
                            }
                            else
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = null,
                                    noPrecisa = item.tiene_sintomas == "no",
                                    Dato = new Dato { IdDato = 566 }
                                });
                            }
                        }
                        var dolores = item.dolor_sintomas.Split(',');
                        foreach (var dolor in dolores)
                        {
                            if (dolor == "muscular")
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = "1",
                                    noPrecisa = false,
                                    Dato = new Dato { IdDato = 567 }
                                });
                            }
                            else
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = null,
                                    noPrecisa = true,
                                    Dato = new Dato { IdDato = 567 }
                                });
                            }
                            if (dolor == "abdominal")
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = "1",
                                    noPrecisa = false,
                                    Dato = new Dato { IdDato = 568 }
                                });
                            }
                            else
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = null,
                                    noPrecisa = true,
                                    Dato = new Dato { IdDato = 568 }
                                });
                            }
                            if (dolor == "pecho")
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = "1",
                                    noPrecisa = false,
                                    Dato = new Dato { IdDato = 569 }
                                });
                            }
                            else
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = null,
                                    noPrecisa = true,
                                    Dato = new Dato { IdDato = 569 }
                                });
                            }
                            if (dolor == "articulaciones")
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = "1",
                                    noPrecisa = false,
                                    Dato = new Dato { IdDato = 570 }
                                });
                            }
                            else
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = null,
                                    noPrecisa = true,
                                    Dato = new Dato { IdDato = 570 }
                                });
                            }
                        }
                        lst.Add(new OrdenDatoClinico
                        {
                            idCategoriaDato = 172,
                            FechaRegistro = DateTime.Now,
                            Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                            ValorString = item.otros_signos,
                            noPrecisa = item.otros_signos == null,
                            Dato = new Dato { IdDato = 571 }
                        });
                        var lstSignos = item.signos.Split(',');
                        foreach (var signo in lstSignos)
                        {
                            if (signo == "exudado")
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = "1",
                                    noPrecisa = false,
                                    Dato = new Dato { IdDato = 572 }
                                });
                            }
                            else
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = null,
                                    noPrecisa = true,
                                    Dato = new Dato { IdDato = 572 }
                                });
                            }
                            if (signo == "inyeccion")
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = "1",
                                    noPrecisa = false,
                                    Dato = new Dato { IdDato = 573 }
                                });
                            }
                            else
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = null,
                                    noPrecisa = true,
                                    Dato = new Dato { IdDato = 573 }
                                });
                            }
                            if (signo == "convulsión")
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = "1",
                                    noPrecisa = false,
                                    Dato = new Dato { IdDato = 574 }
                                });
                            }
                            else
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = null,
                                    noPrecisa = true,
                                    Dato = new Dato { IdDato = 574 }
                                });
                            }
                            if (signo == "coma")
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = "1",
                                    noPrecisa = false,
                                    Dato = new Dato { IdDato = 575 }
                                });
                            }
                            else
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = null,
                                    noPrecisa = true,
                                    Dato = new Dato { IdDato = 575 }
                                });
                            }
                            if (signo == "disnea")
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = "1",
                                    noPrecisa = false,
                                    Dato = new Dato { IdDato = 576 }
                                });
                            }
                            else
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = null,
                                    noPrecisa = true,
                                    Dato = new Dato { IdDato = 576 }
                                });
                            }
                        }
                        var Lstcomor = item.comorbilidad.Split(',');
                        foreach (var comor in Lstcomor)
                        {
                            if (comor == "embarzo")
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = "1",
                                    noPrecisa = false,
                                    Dato = new Dato { IdDato = 578 }
                                });
                            }
                            else
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = null,
                                    noPrecisa = true,
                                    Dato = new Dato { IdDato = 578 }
                                });
                            }
                            if (comor == "cardiovascular")
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = "1",
                                    noPrecisa = false,
                                    Dato = new Dato { IdDato = 579 }
                                });
                            }
                            else
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = null,
                                    noPrecisa = true,
                                    Dato = new Dato { IdDato = 579 }
                                });
                            }
                            if (comor == "diabetes")
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = "1",
                                    noPrecisa = false,
                                    Dato = new Dato { IdDato = 580 }
                                });
                            }
                            else
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = null,
                                    noPrecisa = true,
                                    Dato = new Dato { IdDato = 580 }
                                });
                            }
                            if (comor == "neurologica")
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = "1",
                                    noPrecisa = false,
                                    Dato = new Dato { IdDato = 581 }
                                });
                            }
                            else
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = null,
                                    noPrecisa = true,
                                    Dato = new Dato { IdDato = 581 }
                                });
                            }
                            if (comor == "parto")
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = "1",
                                    noPrecisa = false,
                                    Dato = new Dato { IdDato = 583 }
                                });
                            }
                            else
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = null,
                                    noPrecisa = true,
                                    Dato = new Dato { IdDato = 583 }
                                });
                            }
                            if (comor == "inmunodeficiencia")
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = "1",
                                    noPrecisa = false,
                                    Dato = new Dato { IdDato = 584 }
                                });
                            }
                            else
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = null,
                                    noPrecisa = true,
                                    Dato = new Dato { IdDato = 584 }
                                });
                            }
                            if (comor == "hepatico")
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = "1",
                                    noPrecisa = false,
                                    Dato = new Dato { IdDato = 584 }
                                });
                            }
                            else
                            {
                                lst.Add(new OrdenDatoClinico
                                {
                                    idCategoriaDato = 172,
                                    FechaRegistro = DateTime.Now,
                                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                                    ValorString = null,
                                    noPrecisa = true,
                                    Dato = new Dato { IdDato = 584 }
                                });
                            }
                        }
                        lst.Add(new OrdenDatoClinico
                        {
                            idCategoriaDato = 172,
                            FechaRegistro = DateTime.Now,
                            Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                            ValorString = item.viaje_1_pais,
                            noPrecisa = item.viaje_1_pais == null,
                            Dato = new Dato { IdDato = 587 }
                        });
                        lst.Add(new OrdenDatoClinico
                        {
                            idCategoriaDato = 172,
                            FechaRegistro = DateTime.Now,
                            Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                            ValorString = item.viaje_1_ciudad,
                            noPrecisa = item.viaje_1_ciudad == null,
                            Dato = new Dato { IdDato = 588 }
                        });
                        lst.Add(new OrdenDatoClinico
                        {
                            idCategoriaDato = 172,
                            FechaRegistro = DateTime.Now,
                            Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                            ValorString = item.viaje_2_pais,
                            noPrecisa = item.viaje_2_pais == null,
                            Dato = new Dato { IdDato = 589 }
                        });
                        lst.Add(new OrdenDatoClinico
                        {
                            idCategoriaDato = 172,
                            FechaRegistro = DateTime.Now,
                            Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                            ValorString = item.viaje_2_ciudad,
                            noPrecisa = item.viaje_2_ciudad == null,
                            Dato = new Dato { IdDato = 590 }
                        });
                        lst.Add(new OrdenDatoClinico
                        {
                            idCategoriaDato = 172,
                            FechaRegistro = DateTime.Now,
                            Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                            ValorString = item.viaje_3_pais,
                            noPrecisa = item.viaje_3_pais == null,
                            Dato = new Dato { IdDato = 592 }
                        });
                        lst.Add(new OrdenDatoClinico
                        {
                            idCategoriaDato = 172,
                            FechaRegistro = DateTime.Now,
                            Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                            ValorString = item.tuvo_contacto,
                            noPrecisa = item.tuvo_contacto == null,
                            Dato = new Dato { IdDato = 595 }
                        });

                        orden.ordenDatoClinicoList = lst;
                        categoriaDatoList.Add(new CategoriaDato
                        {
                            IdCategoriaDato = 171
                        });
                        categoriaDatoList.Add(new CategoriaDato
                        {
                            IdCategoriaDato = 172
                        });
                        var w = new List<Enfermedad>();
                           w.Add(new Enfermedad
                        {
                            idEnfermedad = 1005635,//VIH
                            categoriaDatoList = categoriaDatoList
                        });
                        //orden.ordenExamenList = ordenExamenListAgregados;
                        //orden.ordenMuestraList = ordenMuestraListAgregados;
                        //orden.ordenMaterialList = ordenMaterialListAgregados;
                        //orden.enfermedadList = enfermedadListAgregados;
                        orden.enfermedadList = w;
                        orden.estatus = 1;
                        orden.IdUsuarioRegistro = 72;
                        orden.fechaSolicitud = DateTime.Parse(DateTime.Now.ToShortDateString());
                        orden.observacion = "Registro ODK";
                        //ordenBL.InsertOrden(orden, Enums.TipoRegistroOrden.SOLO_ORDEN);
                    }

                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    //
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //private List<DatosAdicionales> DatoClinico(List<DatosAdicionales> LstDatosAdicionales)
        //{
        //    var result = new List<DatosAdicionales>();
        //    if (LstDatosAdicionales.Count > 0)
        //    {
        //        foreach (var item in LstDatosAdicionales)
        //        {
        //            var oTramaDatos = new OrdenDal().GetTramaData().Where(x => x.IdTabla == "EQUIVALENCIA" && x.IdTablaHija == "SIGNOSYSINTOMAS");
        //            foreach (var itemDatosClinicos in oTramaDatos)
        //            {
        //                var x = new DatosAdicionales();
        //                if (!String.IsNullOrEmpty(item.codigo) && !String.IsNullOrEmpty(item.valor))
        //                {
        //                    x.idDato = "551";
        //                       ///* oTramaDatos.Where(i => i.Campo4 == item.codigo && itemDatosClinicos.IdTabla == "EQUIVALENCIA" && itemDatosClinicos.IdTablaHija == "SIGNOSYSINTOMAS").ToList().Count() == 0 ? null : oTramaD*/atos.Where(i => i.Campo4 == item.codigo && itemDatosClinicos.IdTabla == "EQUIVALENCIA" && itemDatosClinicos.IdTablaHija == "SIGNOSYSINTOMAS").FirstOrDefault().Campo2;
        //                    if (!String.IsNullOrEmpty(x.idDato))
        //                    {
        //                        switch (item.codigo)
        //                        {
        //                            case " ":
        //                                //Obtener Equivalencia
        //                                x.valor = oTramaDatos.Where(i => i.Campo5 == "0001" && i.Campo1 == item.valor && itemDatosClinicos.IdTabla == "EQUIVALENCIA" && itemDatosClinicos.IdTablaHija == "SIGNOSYSINTOMAS").FirstOrDefault().Campo2;
        //                                x.noPrecisa = "0";
        //                                break;
        //                            case "cod_tip_poblacion":
        //                                x.valor = oTramaDatos.Where(i => i.Campo5 == "0002" && i.Campo2 == item.valor && itemDatosClinicos.IdTabla == "DATO").FirstOrDefault().Campo4;
        //                                x.noPrecisa = "0";
        //                                break;
        //                            case "cod_tip_riesgo":
        //                                x.valor = oTramaDatos.Where(i => i.Campo5 == "0003" && i.Campo2 == item.valor && itemDatosClinicos.IdTabla == "DATO").FirstOrDefault().Campo4;
        //                                x.noPrecisa = "0";
        //                                break;
        //                            case "cod_inmunospresion":
        //                                x.valor = oTramaDatos.Where(i => i.Campo5 == "0004" && i.Campo2 == item.valor && itemDatosClinicos.IdTabla == "DATO").FirstOrDefault().Campo4;
        //                                x.noPrecisa = "0";
        //                                break;
        //                            case "cod_tip_contacto":
        //                                x.valor = oTramaDatos.Where(i => i.Campo5 == "0005" && i.Campo2 == item.valor && itemDatosClinicos.IdTabla == "DATO").FirstOrDefault().Campo4;
        //                                x.noPrecisa = "0";
        //                                break;
        //                            case "cod_tip_exposicion":
        //                                x.valor = oTramaDatos.Where(i => i.Campo5 == "0006" && i.Campo2 == item.valor && itemDatosClinicos.IdTabla == "DATO").FirstOrDefault().Campo4;
        //                                x.noPrecisa = "0";
        //                                break;
        //                            case "cod_otros_riesgos":
        //                                x.valor = oTramaDatos.Where(i => i.Campo5 == "0007" && i.Campo2 == item.valor && itemDatosClinicos.IdTabla == "DATO").FirstOrDefault().Campo4;
        //                                x.noPrecisa = "0";
        //                                break;
        //                            case "cod_pob_riesgo":
        //                                x.valor = oTramaDatos.Where(i => i.Campo5 == "0002" && i.Campo2 == item.valor && itemDatosClinicos.IdTabla == "DATO").FirstOrDefault().Campo4;
        //                                x.noPrecisa = "0";
        //                                break;
        //                            case "cod_ret_programa":
        //                                x.valor = oTramaDatos.Where(i => i.Campo5 == "0008" && i.Campo1 == item.valor && itemDatosClinicos.IdTabla == "DATO").FirstOrDefault().Campo4;
        //                                x.noPrecisa = "0";
        //                                break;
        //                            default:
        //                                x.valor = item.valor;
        //                                x.noPrecisa = "0";
        //                                break;
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    x.valor = null;
        //                    x.noPrecisa = "1";
        //                }
        //                result.Add(x);
        //                break;
        //            }
        //        }
        //    }
        //    return result;
        //}
        private List<OrdenMaterial> CreateObjectOrdenMaterial(int cantidad, bool noPrecisaVolumen, decimal volumen,
           int tipoMaterial, Material material, string fechaEnvio, string horaEnvio, OrdenMuestra ordenMuestra,
           Laboratorio laboratorio, OrdenExamen ordenExamen, List<OrdenMaterial> ordenMaterialListAgregados, int nroMuestra)
        {
            var ordenMaterial = new OrdenMaterial
            {
                idOrdenMaterial = Guid.NewGuid(),
                cantidad = cantidad,
                OrdenExamen = ordenExamen,
                noPrecisaVolumen = noPrecisaVolumen ? 1 : 0,
                volumenMuestraColectada = noPrecisaVolumen ? 0 : volumen,
                Tipo = tipoMaterial,
                Material = material,
                ordenMuestra = ordenMuestra,
                Laboratorio = laboratorio,
                ordenMuestraRecepcionList = new List<OrdenMuestraRecepcion>(),
                nroMuestra = nroMuestra
            };

            //var fecha = fechaEnvio.Split('/'); juan muga
            //ordenMaterial.fechaEnvio = new DateTime(Convert.ToInt32(fecha[2]), Convert.ToInt32(fecha[1]), Convert.ToInt32(fecha[0])); juan muga
            ordenMaterial.fechaEnvio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            //var hora = horaEnvio.Split(':'); juan muga
            //ordenMaterial.horaEnvio = new DateTime(Convert.ToInt32(fecha[2]), Convert.ToInt32(fecha[1]), Convert.ToInt32(fecha[0]), Convert.ToInt32(hora[0]), Convert.ToInt32(hora[1]), 0); juan muga
            ordenMaterial.horaEnvio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
            
            ordenMaterialListAgregados.Add(ordenMaterial);
            return ordenMaterialListAgregados;
        }
     
    }
   
}
