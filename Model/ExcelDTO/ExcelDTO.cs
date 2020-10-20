using System;

namespace Model.ExcelDTO
{
    [Serializable]
    public class ExcelEstablecimiento
    {
        public string Establecimiento { get; set; }
        public string Codigo { get; set; }
        public int Anio { get; set; }
        public int Orden { get; set; }
    }

    [Serializable]
    public enum EstablecimientoColumnas
    {
        establecimiento,
        codigo,
        año,
        orden
    }

    #region ExcelPaciente

    [Serializable]
    public class ExcelPacienteDTO
    {
        public int? TipoDocumento { get; set; }
        public string NroDocumento { get; set; }
        public string FechaNacimiento { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Nombres { get; set; }
        public int Genero { get; set; }
        public int Ubigeo { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Cel1 { get; set; }
        public string Cel2 { get; set; }
        public string Correo { get; set; }
        public string Profesion { get; set; }
        public string Seguro { get; set; }
        public int OtroSeguro { get; set; }
    }

    [Serializable]
    public enum PacienteColumnas
    {
        //tipodocumento,
        //nrodocumento,
        //fechanacimiento,
        //apellidopaterno,
        //apellidomaterno,
        //nombres,
        //genero,
        //ubigeo,
        //direccion,
        //telefono,
        //cel1,
        //cel2,
        //correo,
        //profesion,
        //seguro,
        //otroseguro,
        //
        Nro_documento,
        fechaobtencion,
        fecharecepcionenrom,
        renipresestablecimientoorigen,
        renipresestablecimientodeenvio
    }

    #endregion

    #region ExcelSolicitante

    [Serializable]
    public class ExcelSolicitanteDTO
    {
        public string Dni { get; set; }
        public string CodigoProf { get; set; }
        public string ApePat { get; set; }
        public string ApeMat { get; set; }
        public string Nombres { get; set; }
        public string Correo { get; set; }
        public string Profesion { get; set; }
        public string Telefono { get; set; }
        public string CentroTrabajo { get; set; }
        public string Item { get; set; }
    }

    [Serializable]
    public enum SolicitanteColumnas
    {
        dni,
        codigoprof,
        apepat,
        apemat,
        nombres,
        correo,
        profesion,
        telefono,
        centrotrabajo,
        item
    }

    #endregion

    #region ExcelOrden

    [Serializable]
    public class ExcelOrden
    {
        public string CodigoOrden { get; set; }
        public string Dni { get; set; }
        public string Motivo { get; set; }
        public string EESSOrigen { get; set; }
        public string FechaSolicitud { get; set; }
        public string Solicitante { get; set; }
        public string Observaciones { get; set; }
    }

    [Serializable]
    public enum OrdenColumnas
    {
        codigodeorden,
        dni,
        motivo,
        eessorigen,
        fechasolicitud,
        solicitante,
        observaciones
    }

    #endregion

    #region ExcelOrdenMuestra

    [Serializable]
    public class ExcelOrdenMuestra
    {
        public string CodigoOrden { get; set; }
        public string Item { get; set; }
        public int IdTipoMuestra { get; set; }
        public DateTime FechaObten { get; set; }
        public DateTime HoraObten { get; set; }
        public int CodigoMuestra { get; set; }
    }

    [Serializable]
    public enum OrdenMuestraColumnas
    {
        codigodeorden,
        item,
        idtipomuestra,
        fechaobten,
        horaobten,
        codigomuestra
    }

    #endregion

    [Serializable]
    public class ExcelOrdenExamenMuestra
    {
        public string CodigoOrden { get; set; }
        public string Item { get; set; }
        public Guid IdExamen { get; set; }
        public int IdTipoMuestra { get; set; }
        public int EstabDestino { get; set; }
        public int NumeroMuestra { get; set; }
    }

    [Serializable]
    public enum OrdenExamenMuestraColumnas
    {
        codigodeorden,
        item,
        idexamen,
        idtipomuestra,
        estabdestino,
        numeromuestra
    }

    [Serializable]
    public class ExcelOrdenExamen
    {
        public string CodigoOrden { get; set; }
        public string Item { get; set; }
        public int IdEnfermedad { get; set; }
        public int IdTipoMuestra { get; set; }
        public Guid IdExamen { get; set; }
    }

    [Serializable]
    public enum OrdenExamenColumnas
    {
        codigodeorden,
        item,
        idenfermedad,
        idtipomuestra,
        idexamen
    }

    [Serializable]
    public class TBC
    {

    }

    [Serializable]
    public class ExcelVIH
    {
        public string codigoorden { get; set; }
        public string motivonoti { get; set; }
        public string etnia { get; set; }
        public string grado_instrucc { get; set; }
        public string condic_espec { get; set; }
        public string sexo_nac { get; set; }
        public string ident_genero { get; set; }
        public string antec_sexual { get; set; }
        public string via_transm { get; set; }
        public DateTime? fecha_tamiz1 { get; set; }
        public bool fecha_tamiz1_nop { get; set; }
        public string prueba_tamiz1 { get; set; }
        public DateTime? fecha_tamiz2 { get; set; }
        public bool fecha_tamiz2_nop { get; set; }
        public string prueba_tamiz2 { get; set; }
        public DateTime? fecha_conf1 { get; set; }
        public bool prueba_tamiz2_nop { get; set; }
        public string prueba_conf1 { get; set; }
        public DateTime? fecha_conf2 { get; set; }
        public bool fecha_conf2_nop { get; set; }
        public string prueba_conf2 { get; set; }
        public DateTime? fecha_ini_trat { get; set; }
        public bool fecha_ini_trat_nop { get; set; }
        public string estadio { get; set; }
        public string infecc_tbc { get; set; }
        public DateTime? fecha_def { get; set; }
        public bool fecha_def_nop { get; set; }
        public string asoc_sida { get; set; }
        public string otra_causa { get; set; }
        public bool otra_causa_nop { get; set; }
        public DateTime? fecha_ini_sint { get; set; }
        public bool fecha_ini_sint_nop { get; set; }
    }

    [Serializable]
    public enum VIHColumnas
    {
        codigodeorden,
        motivonoti,
        etnia,
        grado_instrucc,
        condic_espec,
        sexo_nac,
        ident_genero,
        antec_sexual,
        via_transm,
        fecha_tamiz1,
        fecha_tamiz1_nop,
        prueba_tamiz1,
        fecha_tamiz2,
        fecha_tamiz2_nop,
        prueba_tamiz2,
        fecha_conf1,
        prueba_tamiz2_nop,
        prueba_conf1,
        fecha_conf2,
        fecha_conf2_nop,
        prueba_conf2,
        fecha_ini_trat,
        fecha_ini_trat_nop,
        estadio,
        infecc_tbc,
        fecha_def,
        fecha_def_nop,
        asoc_sida,
        otra_causa,
        otra_causa_nop,
        fecha_ini_sint,
        fecha_ini_sint_nop
    }
}
