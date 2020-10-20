using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class MuestrasODKCoronavirus
    {
        public string ident { get; set; }
        public string fec_notificacion { get; set; }
        public string cod_renipres { get; set; }
        public string tipo_doc { get; set; }
        public string nro_documento { get; set; }
        public string ape_nom_paciente { get; set; }
        public string ape_mat_paciente { get; set; }
        public string ape_pat_paciente { get; set; }
        public string fec_nac_pac { get; set; }
        public string celular_pac { get; set; }
        public string Sexo_pac { get; set; }
        public string email_pc { get; set; }
        public string direccion_o_residencia { get; set; }        
        public string direccion_pac { get; set; }
        public string cod_dept_pac { get; set; }    
        public string cod_prov_pac { get; set; }
        public string cod_dist_pac { get; set; }
        public string ocupacion { get; set; }
        public string tipo_seguro { get; set; }
        public string prueba_rapida { get; set; }
        public string fec_prueba_rapida { get; set; }
        public string fec_ini_sintomas { get; set; }//527	Fecha de Inicio de Sintomas
        public string esta_hipostilizado { get; set; }//534	Hospitalización
        public string fec_hospitalizacion { get; set; }
        public string tiene_sintomas { get; set; }
        public string sintomas { get; set; }
        public string dolor_sintomas { get; set; }
        public string otros_sintomas { get; set; }
        public string temperatura { get; set; }//528	Temperatura
        public string signos { get; set; }
        public string otros_signos { get; set; }
        public string comorbilidad { get; set; }//530	Tos 518	Asma 520	Diabetes miellitus
        public string ha_viajado { get; set; }
        public string viaje_1_pais { get; set; }
        public string viaje_1_ciudad { get; set; }
        public string viaje_2_pais { get; set; }
        public string viaje_2_ciudad { get; set; }
        public string viaje_3_pais { get; set; }
        public string viaje_3_ciudad { get; set; }
        public string tuvo_contacto { get; set; }
        public string fec_muestra { get; set; }
        public string tipo_muestra { get; set; }
        public string tiene_dni { get; set; }
        public string dni_inv { get; set; }
        public string ubicacion_latitud { get; set; }
        public string ubicacion_longitud { get; set; }

        public string resultadoPruebaRapida { get; set; }
        public string fecresultadoPruebaRapida { get; set; }
    }

    [Serializable]
    public class TramaData
    {
        public string IdTabla { get; set; }
        public string IdTablaHija { get; set; }
        public string Campo1 { get; set; }
        public string Campo2 { get; set; }
        public string Campo3 { get; set; }
        public string Campo4 { get; set; }
        public string Campo5 { get; set; }
        public int McaInh { get; set; }
    }
}
