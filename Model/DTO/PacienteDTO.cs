using System;
using System.Collections.Generic;

namespace Model.DTO
{
    [Serializable]
    public class PacienteDTO
    {
        public Paciente paciente { get; set; }

        public bool pacienteEncontrado { get; set; }

        public List<UbigeoDTO> ubigeoDTODepartamentoList { get; set; }

        public List<UbigeoDTO> ubigeoDTOProvinciaList { get; set; }

        public List<UbigeoDTO> ubigeoDTODistritoList { get; set; }

        public String codigoDepartamento { get; set; }
        public String codigoProvincia { get; set; }

        public String codigoDistrito { get; set; }


        //UBIGEO ACTUAL
        public List<UbigeoDTO> ubigeoDTODepartamentoActualList { get; set; }

        public List<UbigeoDTO> ubigeoDTOProvinciaActualList { get; set; }

        public List<UbigeoDTO> ubigeoDTODistritoActualList { get; set; }

        public String codigoDepartamentoActual { get; set; }
        public String codigoProvinciaActual { get; set; }

        public String codigoDistritoActual { get; set; }



    }
}
