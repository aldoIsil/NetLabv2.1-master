using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Model;
using DataLayer;
using Reniec.Entities;

namespace BusinessLayer
{
    public class PacienteBl
    {
        /// <summary>
        /// Descripción: Lista todos los pacientes por validar  con el web service de la reniec.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// </summary>
        /// <param name="pagina"></param>
        /// <param name="numRegPorPagima"></param>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <returns></returns>
        public List<Paciente> getPacientesSinValidar(int pagina, int numRegPorPagima, DateTime fechaDesde, DateTime fechaHasta)
        {

            List<Paciente> pacienteList = new List<Paciente>();

            using (var pacienteDal = new DataLayer.PacienteDal())
            {
                pacienteList = pacienteDal.GetPacientesSinValidar(pagina, numRegPorPagima, fechaDesde, fechaHasta);
            }

            return pacienteList;
        }


        /// <summary>
        /// Descripción: Lista todos los pacientes por tipo de documento y nro de documento, RETORNANDO TODA LA INFORMACION DEL PACIENTE.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="pagina"></param>
        /// <param name="numRegPorPagima"></param>
        /// <param name="tipoDocumento"></param>
        /// <param name="nroDocumento"></param>
        /// <returns></returns>
        public List<Paciente> getPacientes(int pagina, int numRegPorPagima, int tipoDocumento, String nroDocumento, String apellidoPaterno, String apellidoMaterno)
        {
            using (var pacienteDal = new PacienteDal())
            {
                var pacienteList = pacienteDal.GetPacientes(pagina, numRegPorPagima, tipoDocumento, nroDocumento, apellidoPaterno, apellidoMaterno);
                return pacienteList;
            }
        }
        public List<Paciente> getPacientes2(int pagina, int numRegPorPagima, int tipoDocumento, String nroDocumento)
        {
            using (var pacienteDal = new PacienteDal())
            {
                var pacienteList = pacienteDal.GetPacientes2(pagina, numRegPorPagima, tipoDocumento, nroDocumento);
                return pacienteList;
            }
        }
        /// <summary>
        /// Descripción: Lista todos los pacientes por tipo de documento y nro de documento, RETORNANDO TODA LA INFORMACION DEL PACIENTE.
        /// Realiza una consulta al web service del a reniec para la validacion de datos.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="pagina"></param>
        /// <param name="numRegPorPagima"></param>
        /// <param name="tipoDocumento"></param>
        /// <param name="nroDocumento"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public List<Paciente> getPacientes(int pagina, int numRegPorPagima, int tipoDocumento, String nroDocumento, ref int flag)
        {
            using (var pacienteDal = new PacienteDal())
            {
                //Se busca el paciente en RENIEC
                Persona persona = null;
                IReniecConsumer reniecConsumer = new ReniecConsumer();
                if (tipoDocumento == 1)
                {
                    persona = reniecConsumer.getReniec(nroDocumento);
                }

                //Se busca el Paciente en BD
                var pacienteList = pacienteDal.GetPacientes(pagina, numRegPorPagima, tipoDocumento, nroDocumento, "", "");
                flag = 0;
                var existeDiferencia = false;
                if (pacienteList.Count == 1 && persona != null)
                {
                    var paciente = pacienteList[0];

                    //Se verifica Apellido Paterno
                    if (!persona.ApellidoPaterno.Trim().ToUpper().Equals(paciente.ApellidoPaterno.Trim().ToUpper()))
                    {
                        existeDiferencia = true;
                    }
                    //Se verifica Apellido Materno
                    else if (!persona.ApellidoMaterno.Trim().ToUpper().Equals(paciente.ApellidoMaterno.Trim().ToUpper()))
                    {
                        existeDiferencia = true;
                    }
                    //Se verifica Apellido Nombres
                    else if (!persona.Nombres.Trim().ToUpper().Equals(paciente.Nombres.Trim().ToUpper()))
                    {
                        existeDiferencia = true;
                    }
                    //Se verifica Fecha Nacimiento
                    else if (DateTime.Compare(persona.FechaNacimiento, paciente.FechaNacimiento.Value) != 0)
                    {
                        existeDiferencia = true;
                    }
                    //Se verifica Direccion
                    else if (!persona.Direccion.DireccionReniec.Trim().ToUpper().Equals(paciente.DireccionReniec.Trim().ToUpper()))
                    {
                        existeDiferencia = true;
                    }
                    //Se verifica Ubigeo
                    else if (!persona.Direccion.CodigoUbigeo.Trim().ToUpper().Equals(paciente.UbigeoReniec.Id.ToUpper()))
                    {
                        existeDiferencia = true;
                    }
                    //Se verifica Genero
                    else
                    {
                        var generoPersona = persona.Genero == Model.Enums.Genero.Masculino ? 1 : 2;
                        if (generoPersona != paciente.Genero)
                        {
                            existeDiferencia = true;
                        }
                    }

                    //2 es un estado que indica que existe diferencia
                    if (existeDiferencia) paciente.Estado = 2;
                }
                else if (pacienteList.Count == 0)
                {
                    //Si no se encontro paciente y se encontro en RENIEC

                    if (persona != null)
                    {
                        flag = 1;
                        var paciente = new Paciente
                        {
                            Nombres = persona.Nombres,
                            NroDocumento = nroDocumento,
                            ApellidoPaterno = persona.ApellidoPaterno,
                            ApellidoMaterno = persona.ApellidoMaterno,
                            FechaNacimiento = persona.FechaNacimiento,
                            DireccionReniec = persona.Direccion.DireccionReniec,
                            UbigeoReniec = new Ubigeo { Id = persona.Direccion.CodigoUbigeo.Trim() },
                            Genero = persona.Genero == Model.Enums.Genero.Masculino ? 1 : 2
                        };
                        pacienteList.Add(paciente);
                    }
                    else
                    {
                        //Si no se encontro paciente y no encontro en RENIEC
                        flag = 2;
                    }

                }

                return pacienteList;
            }
        }


        public Task<Paciente> ObtenerDatosReniec(string dni)
        {
            /*      IReniecConsumer reniecConsumer = new ReniecConsumer();
                  Persona persona = reniecConsumer.getReniec(dni);
                  Paciente paciente = new Paciente();
                  PacienteDatoComplementario pacienteDatoComplementario = new PacienteDatoComplementario();
                  paciente.DatoComplementario = pacienteDatoComplementario;

                  String codigoDepartamentoReniec = "";
                  String codigoProvinciaReniec = "";
                  String codigoDistritoReniec = "";
                  String codigoDepartamentoActual = "";
                  String codigoProvinciaActual = "";
                  String codigoDistritoActual = "";

                  if (persona != null)
                  {
                      paciente.ApellidoPaterno = persona.ApellidoPaterno;
                      paciente.ApellidoMaterno = persona.ApellidoMaterno;
                      paciente.Nombres = persona.Nombres;
                      paciente.DatoComplementario.DireccionReniec = persona.Direccion.DireccionReniec;
                      paciente.DatoComplementario.IdUbigeo = persona.Direccion.CodigoUbigeo;
                      paciente.Genero = persona.Genero;
                      paciente.FechaNacimiento = persona.FechaNacimiento;
                      paciente.NroDocumento = dni;

                      codigoDepartamentoReniec = paciente.DatoComplementario.IdUbigeo.Substring(0, 2);
                      codigoProvinciaReniec = paciente.DatoComplementario.IdUbigeo.Substring(2, 2);
                      codigoDistritoReniec = paciente.DatoComplementario.IdUbigeo.Substring(4, 2);
                  }
                  else
                  {

                      try
                      {
                          NetlabDal netlabDal = new NetlabDal();
                          paciente = netlabDal.GetPacienteByDocumento(dni);
                          codigoDepartamentoReniec = paciente.DatoComplementario.IdUbigeo.Substring(0, 2);
                          codigoProvinciaReniec = paciente.DatoComplementario.IdUbigeo.Substring(2, 2);
                          codigoDistritoReniec = paciente.DatoComplementario.IdUbigeo.Substring(4, 2);


                          codigoDepartamentoActual = paciente.DatoComplementario.IdUbigeoActual.Substring(0, 2);
                          codigoProvinciaActual = paciente.DatoComplementario.IdUbigeoActual.Substring(2, 2);
                          codigoDistritoActual = paciente.DatoComplementario.IdUbigeoActual.Substring(4, 2);

                          UbigeoBl ubigeoBl = new UbigeoBl();
                          List<UbigeoDTO> ubigeoDTOList = ubigeoBl.ObtenerDepartamentos();

                          foreach (UbigeoDTO ubigeoDepartamento in ubigeoDTOList)
                          {
                              if (ubigeoDepartamento.codigo.Equals(codigoDepartamentoActual))
                              {
                                  paciente.DepartamentoActual = ubigeoDepartamento.nombre;
                                  foreach (UbigeoDTO ubigeoProvincia in ubigeoDepartamento.ubigeoDTOList)
                                  {
                                      if (ubigeoProvincia.codigo.Equals(codigoProvinciaActual))
                                      {
                                          paciente.ProvinciaActual = ubigeoProvincia.nombre;
                                          foreach (UbigeoDTO ubigeoDistrito in ubigeoProvincia.ubigeoDTOList)
                                          {
                                              if (ubigeoDistrito.codigo.Equals(codigoDistritoActual))
                                              {
                                                  paciente.DistritoActual = ubigeoDistrito.nombre;
                                                  break;
                                              }
                                          }
                                          break;
                                      }
                                  }
                                  break;
                              }
                          }
                      }
                      catch (Exception)
                      {

                      }
                  }

                  if (paciente != null)
                  {
                      UbigeoBl ubigeoBl = new UbigeoBl();

                      List<UbigeoDTO> ubigeoDTOList = ubigeoBl.ObtenerDepartamentos();

                      foreach (UbigeoDTO ubigeoDepartamento in ubigeoDTOList)
                      {
                          if (ubigeoDepartamento.codigo.Equals(codigoDepartamentoReniec))
                          {
                              paciente.Departamento = ubigeoDepartamento.nombre;
                              foreach (UbigeoDTO ubigeoProvincia in ubigeoDepartamento.ubigeoDTOList)
                              {
                                  if (ubigeoProvincia.codigo.Equals(codigoProvinciaReniec))
                                  {
                                      paciente.Provincia = ubigeoProvincia.nombre;
                                      foreach (UbigeoDTO ubigeoDistrito in ubigeoProvincia.ubigeoDTOList)
                                      {
                                          if (ubigeoDistrito.codigo.Equals(codigoDistritoReniec))
                                          {
                                              paciente.Distrito = ubigeoDistrito.nombre;
                                              break;
                                          }
                                      }
                                      break;
                                  }
                              }
                              break;
                          }
                      }



                  }
                  */
            var paciente = new Paciente();
            return Task.FromResult(paciente);
        }

        /// <summary>
        /// Descripción: Validar los datos del paciente con Reniec
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// </summary>
        /// <param name="paciente"></param>
        /// <returns></returns>
        public Paciente ValidarDatosPaciente(Paciente paciente)
        {
            IReniecConsumer reniecConsumer = new ReniecConsumer();
            Persona persona = reniecConsumer.getReniec(paciente.NroDocumento);



            /*Si Persona es distina de Null quiere decir que se encontro el Paciente*/
            if (persona != null)
            {
                paciente.ApellidoPaterno = persona.ApellidoPaterno;
                paciente.ApellidoMaterno = persona.ApellidoMaterno;
                paciente.Nombres = persona.Nombres;
                paciente.DireccionReniec = persona.Direccion.DireccionReniec;
                UbigeoPacienteBl ubigeoBl = new UbigeoPacienteBl();
                //paciente.UbigeoReniec = ubigeoBl.GetUbigeoById(persona.Direccion.CodigoUbigeo);
                paciente.Genero = persona.Genero == Model.Enums.Genero.Masculino ? 1 : 2;
                paciente.FechaNacimiento = persona.FechaNacimiento;


                /* codigoDepartamentoReniec = paciente.DatoComplementario.IdUbigeo.Substring(0, 2);
                 codigoProvinciaReniec = paciente.DatoComplementario.IdUbigeo.Substring(2, 2);
                 codigoDistritoReniec = paciente.DatoComplementario.IdUbigeo.Substring(4, 2);*/
                using (PacienteDal pacienteDal = new PacienteDal())
                {
                    pacienteDal.UpdateDatosReniec(paciente);
                }
                return paciente;
            }

            return null;
        }

        /// <summary>
        /// Descripción: Obtiene informacion del paciente por medio del web service de la reniec
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// </summary>
        /// <param name="paciente"></param>
        /// <returns></returns>
        public Paciente getReniec(Paciente paciente)
        {
            IReniecConsumer reniecConsumer = new ReniecConsumer();
            Persona persona = reniecConsumer.getReniec(paciente.NroDocumento);

            /*Si Persona es distina de Null quiere decir que se encontro el Paciente*/
            if (persona != null)
            {
                paciente.ApellidoPaterno = persona.ApellidoPaterno;
                paciente.ApellidoMaterno = persona.ApellidoMaterno;
                paciente.Nombres = persona.Nombres;
                paciente.DireccionReniec = persona.Direccion.DireccionReniec;
                paciente.UbigeoReniec = new Ubigeo();
                //paciente.UbigeoReniec.Id = persona.Direccion.CodigoUbigeo;

                UbigeoPacienteBl ubigeoBl = new UbigeoPacienteBl();
                paciente.UbigeoReniec = ubigeoBl.GetUbigeoById(persona.Direccion.CodigoUbigeo, persona.Direccion.NombreDistrito, paciente.IdUsuarioRegistro);

                paciente.Genero = persona.Genero == Model.Enums.Genero.Masculino ? 1 : 2;
                paciente.FechaNacimiento = persona.FechaNacimiento;
                return paciente;
            }

            return null;
        }

        /// <summary>
        /// Descripción: Registra pacientes
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// </summary>
        /// <param name="paciente"></param>
        /// <returns></returns>
        public Guid InsertPaciente(Paciente paciente)
        {
            using (var pacienteDal = new PacienteDal())
            {
                return pacienteDal.InsertPaciente(paciente);
            }
        }
        /// <summary>
        /// Descripción: Actualiza datos del paciente.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// </summary>
        /// <param name="paciente"></param>
        public void ActualizarPaciente(Paciente paciente)
        {
            using (var pacienteDal = new PacienteDal())
            {
                pacienteDal.UpdatePaciente(paciente);
            }
        }
        /// <summary>
        /// Descripción: Actualiza solo el ubigeo del paciente.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017. 
        /// </summary>
        /// <param name="paciente"></param>
        public void UpdateDatosPaciente(Paciente paciente)
        {
            using (var pacienteDal = new PacienteDal())
            {
                pacienteDal.UpdateDatosPaciente(paciente);
            }
        }

        /// <summary>
        /// Descripción: Obtiene datos del paciente filtrado por NroDocumento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// </summary>
        /// <param name="nroDocumento"></param>
        /// <returns></returns>
        public Paciente GetPacienteByDocumento(string nroDocumento)
        {
            var pacienteDal = new DataLayer.Area.Pacientes.PacienteDal();

            return pacienteDal.GetPacienteByDocumento(nroDocumento);
        }

        /// <summary>
        /// Descripción: Obtiene datos del paciente filtrado por el Codigo interno = Id.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// </summary>
        /// <param name="idPaciente"></param>
        /// <returns></returns>
        public Paciente getPacienteById(Guid idPaciente)
        {
            using (var pacienteDal = new PacienteDal())
            {
                return pacienteDal.GetPacienteById(idPaciente);
            }
        }

        /// <summary>
        /// Descripción: Obtiene datos del paciente filtrado por Tipo y Nro Documento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// </summary>
        /// <param name="tipoDocumento"></param>
        /// <param name="nroDocumento"></param>
        /// <returns></returns>
        public List<Paciente> GetPacientesByTipoNroDocumento(int tipoDocumento, String nroDocumento)
        {
            using (var pacienteDal = new PacienteDal())
            {
                return pacienteDal.GetPacientesByTipoNroDocumento(tipoDocumento, nroDocumento);
            }
        }
        /// <summary>
        /// Descripción: Lee datos del paciente en los laboratorios.
        /// Author: Juan Muga.
        /// Fecha Creacion: 01/11/2017
        /// Fecha Modificación: 
        /// </summary>
        /// <param name="nroDocumento"></param>
        /// <returns></returns>
        public List<Paciente> GetDatosPacienteByNroDocumento(string nroDocumento, int idUsuario)
        {
            using (var pacienteDal = new PacienteDal())
            {
                return pacienteDal.GetDatosPacienteByNroDocumento(nroDocumento, idUsuario);
            }
        }
        /// <summary>
        /// Descripción: Lee datos del paciente en NetLab v1.0.
        /// Author: Juan Muga.
        /// Fecha Creacion: 20/08/2018
        /// </summary>
        /// <param name="nroDocumento"></param>
        /// <param name="cCodMuestra"></param>
        /// <returns></returns>
        public List<Paciente> GetDatosPacienteNetLab1(string nroDocumento, string cCodMuestra)
        {
            using (var pacienteDal = new PacienteDal())
            {
                return pacienteDal.GetDatosPacienteNetLab1(nroDocumento, cCodMuestra);
            }
        }

        /// Descripción: Busca historial de los pacientes NetLab v2.0.
        /// Author: Yonatan Clemente
        /// Fecha Creacion: 08/05/2018
        public List<Paciente> ObtenerHistorialPaciente(int? tipoDocumento, string nroDocumento, string apellidoPaterno, string apellidoMaterno, string nombre)
        {
            using (var pacienteDal = new PacienteDal())
            {
                return pacienteDal.ObtenerHistorialPaciente(tipoDocumento, nroDocumento, apellidoPaterno, apellidoMaterno, nombre);
            }
        }

        /// <summary>
        /// Variante de metodo ObtenerHistorialPaciente agregando parametro idUsuario para la busqueda por el tipo de acceso
        /// </summary>
        /// <param name="tipoDocumento"></param>
        /// <param name="nroDocumento"></param>
        /// <param name="apellidoPaterno"></param>
        /// <param name="apellidoMaterno"></param>
        /// <param name="nombre"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public List<Paciente> ObtenerHistorialPacientePorUsuario(int? tipoDocumento, string nroDocumento, string apellidoPaterno, string apellidoMaterno, string nombre, int idUsuario)
        {
            using (var pacienteDal = new PacienteDal())
            {
                return pacienteDal.ObtenerHistorialPacientePorUsuario(tipoDocumento, nroDocumento, apellidoPaterno, apellidoMaterno, nombre, idUsuario);
            }
        }

        /// <summary>
        /// Descripción: Busca historial de los pacientes NetLab v2.0.
        /// Author: Jose Chavez
        /// Fecha Creacion: 15/07/2019
        /// </summary>
        /// <param name="paciente"></param>
        /// <returns></returns>
        public string InsertPacienteReferencia(Paciente paciente, int idestablecimiento)
        {
            using (var pacienteDal = new PacienteDal())
            {
                if (pacienteDal.InsertPacienteReferencia(paciente, idestablecimiento) > 0)
                {
                    return "Ok";
                }
                else
                {
                    return "Error";
                }
            }
        }
        /// <summary>
        /// Descripción: Busca las referencias y contrareferencias de un paciente
        /// Author: Jose Chavez
        /// Fecha Creacion: 15/07/2019
        /// </summary>
        /// <param name="idPaciente"></param>
        /// <returns></returns>
        public Paciente GetPacienteReferencia(string idPaciente)
        {
            using (var pacienteDal = new PacienteDal())
            {
                return pacienteDal.GetPacienteReferencia(idPaciente);
            }
        }

        public SolicitudExamen GetResultadoPacienteVIH(string idPaciente, int tipoSolicitud)
        {
            using (var pacienteDal = new PacienteDal())
            {
                return pacienteDal.GetResultadoPacienteVIH(idPaciente, tipoSolicitud);
            }
        }

        public void RegistroSolicitudGenotipificacion(SolicitudExamen solicitud)
        {
            using (var pacienteDal = new PacienteDal())
            {
                pacienteDal.RegistroSolicitudGenotipificacion(solicitud);
            }
        }
        public void RegistroSolicitudTropismo(SolicitudExamen solicitud)
        {
            using (var pacienteDal = new PacienteDal())
            {
                pacienteDal.RegistroSolicitudTropismo(solicitud);
            }
        }
        public List<Paciente> GetDatosPacienteByNroDocumentoCoronavirus(string nroDocumento, string nombres, string apePat, string apeMat, string idusuario)
        {
            using (var pacienteDal = new PacienteDal())
            {
                return pacienteDal.GetDatosPacienteByNroDocumentoCoronavirus(nroDocumento, nombres,apePat,apeMat, idusuario.ToString());
            }
        }

        public List<Paciente> GetDatosPacienteByNroDocumentoTbc(string nroDocumento, string nombres, string apePat, string apeMat, string idusuario)
        {
            using (var pacienteDal = new PacienteDal())
            {
                return pacienteDal.GetDatosPacienteByNroDocumentoTbc(nroDocumento, nombres, apePat, apeMat, idusuario.ToString());
            }
        }

        public List<Paciente> GetDatosPacienteByNroDocumentoVIH(string nroDocumento, string nombres, string apePat, string apeMat, string idusuario)
        {
            using (var pacienteDal = new PacienteDal())
            {
                return pacienteDal.GetDatosPacienteByNroDocumentoVIH(nroDocumento, nombres, apePat, apeMat, idusuario.ToString());
            }
        }

        public List<Ciudad> GetCiudadProcedencia(String textoBusqueda)
        {
            using (var pacienteDal = new PacienteDal())
            {
                return pacienteDal.GetCiudadProcedencia(textoBusqueda);
            }
        }
        public List<Paciente> xLstPaciente()
        {
            using (var pacienteDal = new PacienteDal())
            {
                return pacienteDal.xLstPaciente();
            }
        }

        public List<Paciente> GetDatosPacienteByNroDocumentoCoronavirusExterno(string nroDocumento, string nombres, string apePat, string apeMat, string idusuario)
        {
            using (var pacienteDal = new PacienteDal())
            {
                return pacienteDal.GetDatosPacienteByNroDocumentoCoronavirusExterno(nroDocumento, nombres, apePat, apeMat, idusuario.ToString());
            }
        }

        public List<Paciente> ObtenerSiscovidPruebaRapidaPorNroDocumento(string nroDocumento)
        {
            using (var pacienteDal = new PacienteDal())
            {
                return pacienteDal.ObtenerSiscovidPruebaRapidaPorNroDocumento(nroDocumento);
            }
        }

        public bool BuscarPacientePorNroDocumentoEnPRK(string nroDocumento)
        {
            using (var pacienteDal = new PacienteDal())
            {
                return pacienteDal.BuscarPacientePorNroDocumentoEnPRK(nroDocumento);
            }
        }

        public List<Paciente> ConsultaPacienteSolicitudVIH(int? tipoDocumento, string nroDocumento, string apellidoPaterno, string apellidoMaterno, string nombre)
        {
            using (var pacienteDal = new PacienteDal())
            {
                return pacienteDal.ConsultaPacienteSolicitudVIH(tipoDocumento, nroDocumento, apellidoPaterno, apellidoMaterno, nombre);
            }
        }
        public string GetDatoSolicitudExamenVIH(Guid idPaciente, Guid idExamen)
        {
            using (var pacienteDal = new PacienteDal())
            {
                return pacienteDal.GetDatoSolicitudExamenVIH(idPaciente, idExamen);
            }
        }
        public string ValidaResultadoVih(Guid idPaciente)
        {
            using (var pacienteDal = new PacienteDal())
            {
                return pacienteDal.ValidaResultadoVih(idPaciente);
            }
        }
    }
}