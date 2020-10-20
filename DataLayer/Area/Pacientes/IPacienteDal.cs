using Model;

namespace DataLayer.Area.Pacientes
{
    public interface IPacienteDal
    {
        void InsertPaciente(Paciente paciente);
        void UpdatePaciente(Paciente paciente);
        Paciente GetPacienteByDocumento(string nroDocumento);
    }
}
