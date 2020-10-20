using Reniec.Entities;

namespace BusinessLayer
{
    public interface IReniecConsumer
    {
        string ErrorMessage { get; set; }

        Persona getReniec(string dni);
    }
}
