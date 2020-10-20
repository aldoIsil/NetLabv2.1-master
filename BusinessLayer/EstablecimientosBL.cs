using Model;
using NetLab.Repository;
using NetLab.Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class EstablecimientosBL: IEstablecimientosBL
    {
        //private readonly IUnitOfWork unitOfWork;
        //private readonly EstablecimientoRepository establecimientoRepository;

        //public EstablecimientosBL(IUnitOfWork _unitOfWork)
        //{
        //    this.unitOfWork = _unitOfWork;
        //    this.establecimientoRepository = new EstablecimientoRepository(this.unitOfWork);
        //}
        ////private readonly EstablecimientoRepository EstablecimientoRepository;
        //private IEstablecimientoRepository IUoWRepository { get; set; }
        //private IConnectionFactory connectionFactory;

        //public EstablecimientosBL(IEstablecimientoRepository _IUoWRepository)
        //{
        //    //this.IUoWRepository = iUoWRepository;
        //    //this.EstablecimientoRepository =  new EstablecimientoRepository();
        //    //if(_IUoWRepository == null)
        //    //{
        //    //    this.IUoWRepository = new EstablecimientoRepository()
        //    //}
        //    this.IUoWRepository = _IUoWRepository;
        //    //this.IUoWRepository = new EstablecimientoRepository(this.connectionFactory);
        //}

        public List<Establecimiento> ObtenerTodosEstablecimientos()
        {
            //using (IEstablecimientoOpenRepository rep = this.IUoWRepository.Open())
            //{
            //    var establecimientos = rep.ObtenerEstablecimientos();
            //    //aqui hacer toda la logica de negocio

            //    return establecimientos;
            //}
            var establecimientos = new List<Establecimiento>();
            establecimientos = establecimientoRepository.ObtenerEstablecimientos();
            return establecimientos;
        }

        public void Dispose()
        {
            this.Dispose();
        }
    }

    public interface IEstablecimientosBL : IDisposable
    {
        List<Establecimiento> ObtenerTodosEstablecimientos();
    }
}
