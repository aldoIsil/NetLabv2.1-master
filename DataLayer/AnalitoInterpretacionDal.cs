using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class AnalitoInterpretacionDal : DaoBase
    {
        public AnalitoInterpretacionDal(IDalSettings settings) : base(settings)
        {
        }

        public AnalitoInterpretacionDal() : this(new NetlabSettings())
        {
        }

        public List<ExamenResultadoInterpretacion> ObtenerAnalitoInterpretacion(Guid idAnalito)
        {
            List<ExamenResultadoInterpretacion> interpretacion = new List<ExamenResultadoInterpretacion>();
            var objCommand = GetSqlCommand("pNLS_ObtenerAnalitoInterpretacion");
            InputParameterAdd.Guid(objCommand, "idAnalito", idAnalito);
            DataTable dataTable = Execute(objCommand);
            if(dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    ExamenResultadoInterpretacion analito = new ExamenResultadoInterpretacion
                    {
                        idInterpretacion = Converter.GetInt(row, "idInterpretacion"),
                        idAnalito = Converter.GetString(row, "idAnalito"),
                        Analito = Converter.GetString(row, "nombre"),
                        GlosaParent = Converter.GetString(row, "GlosaParent"),
                        Glosa = Converter.GetString(row, "Glosa"),
                        Interpretacion = Converter.GetString(row, "interpretacion"),
                    };
                    interpretacion.Add(analito);
                }
            }            
            return interpretacion;
        }

        public void RegistrarNuevaInterpretacion(string idAnalito, string GlosaParent, string Glosa, string Interpretacion,int idUsuario)
        {
            var objCommand = GetSqlCommand("pNLI_InsertarNuevaInterpretacion");
            InputParameterAdd.Varchar(objCommand, "idAnalito", idAnalito);
            InputParameterAdd.Varchar(objCommand, "GlosaParent", GlosaParent);
            InputParameterAdd.Varchar(objCommand, "Glosa", Glosa);
            InputParameterAdd.Varchar(objCommand, "Interpretacion", Interpretacion);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", idUsuario);
            ExecuteNonQuery(objCommand);
        }

        public ExamenResultadoInterpretacion ObtenerAnalitoInterpretacionPorId(int idInterpretacion)
        {
            ExamenResultadoInterpretacion interpretacion = new ExamenResultadoInterpretacion();
            var objCommand = GetSqlCommand("pNLS_ObtenerAnalitoInterpretacionPorId");
            InputParameterAdd.Int(objCommand, "idInterpretacion", idInterpretacion);
            DataTable datatable = Execute(objCommand);
            var row = datatable.Rows[0];
            interpretacion.idAnalito = row["idAnalito"].ToString();
            interpretacion.GlosaParent = row["GlosaParent"].ToString();
            interpretacion.Glosa = row["Glosa"].ToString();
            interpretacion.Interpretacion = row["Interpretacion"].ToString();
            interpretacion.Estado = int.Parse(row["estado"].ToString());
            interpretacion.idInterpretacion = int.Parse(row["idInterpretacion"].ToString());
            return interpretacion;
        }

        public void EditarAnalitoInterpretacion(int idInterpretacion, string GlosaParent, string Glosa, string Interpretacion, int estado, int idUsuario)
        {
            var objCommand = GetSqlCommand("pNLU_EditarAnalitoInterpretacion");
            InputParameterAdd.Int(objCommand, "idInterpretacion", idInterpretacion);
            InputParameterAdd.Varchar(objCommand, "GlosaParent", GlosaParent);
            InputParameterAdd.Varchar(objCommand, "Glosa", Glosa);
            InputParameterAdd.Varchar(objCommand, "Interpretacion", Interpretacion);
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            InputParameterAdd.Int(objCommand, "estado", estado);
            ExecuteNonQuery(objCommand);
        }
    }
}
