using System.Collections.Generic;
using Model;

namespace BusinessLayer.Interfaces
{
    public interface IUsuarioAreaProcesamientoBl
    {
        List<Model.AreaProcesamiento> GetAreaByUsuarioId(int idUsuario);
        void AgregarAreaPorUsuario(Usuario usuario, int[] areas, int idUsuario);
        void UpdateAreaByUsuario(UsuarioAreaProcesamiento usuarioAreaProcesamiento);
    }
}