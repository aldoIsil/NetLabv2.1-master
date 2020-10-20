using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelDataReader;
using System.IO;
using System.Data;

namespace BusinessLayer.ProcesoDesatendido
{
    public class ExcelImportHelpers
    {
        public List<string> ObtenerListaHojas(string file)
        {
            List<string> books = new List<string>();
            using (var stream = File.Open(file, FileMode.Open, FileAccess.Read))
            {
                // Auto-detect format, supports:
                //  - Binary Excel files (2.0-2003 format; *.xls)
                //  - OpenXml Excel files (2007 format; *.xlsx)
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    do
                    {
                        books.Add(reader.Name);
                    } while (reader.NextResult());

                    int count = books.Count;
                    // The result of each spreadsheet is in result.Tables
                }
            }

            return books;
        }

        public Dictionary<string, string> ObtenerListaColumnas(string file, string nombrehoja)
        {
            using (var stream = File.Open(file, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    Dictionary<string, string> columnList = new Dictionary<string, string>();
                    do
                    {
                        if (reader.Name.ToLower() == nombrehoja.ToLower())
                        {
                            int columnsCount = reader.FieldCount;
                            while (reader.Read())
                            {
                                //var test = ((ExcelDataReader.ExcelDataReader<ExcelDataReader.Core.BinaryFormat.XlsWorkbook, ExcelDataReader.Core.BinaryFormat.XlsWorksheet>)reader).RowCells;
                                //var testdata = reader.AsDataSet();

                                if (reader.Depth == 0)
                                {
                                    for (int i = 0; i < columnsCount; i++)
                                    {
                                        if (reader.GetString(i) != null)
                                        {
                                            columnList.Add(reader.GetString(i), reader.GetString(i));
                                        }
                                    }

                                    break;
                                }
                            }
                        }
                    } while (reader.NextResult());

                    //int count = columnList.Count;
                    return columnList;
                }
            }
        }

        public string GetValueExcel(IDataReader dr, Dictionary<string, string> dictionary, string key)
        {
            string svalue = string.Empty;
            svalue = dictionary.ContainsKey(key) ? (dr[dictionary[key]] != DBNull.Value ? dr[dictionary[key]].ToString() : string.Empty) : string.Empty;

            return !string.IsNullOrEmpty(svalue) ? svalue.Trim().RemoveLineBreaks() : svalue;
        }

        public Tuple<List<Dictionary<string, string>>, List<string>> ObtenerDatosPorHoja(string file, string hoja)
        {
            List<Dictionary<string, string>> allRowsDict = new List<Dictionary<string, string>>();
            List<string> keysOrder = new List<string>();
            using (var stream = File.Open(file, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    List<string> sheetList = new List<string>();
                    do
                    {
                        if (reader.Name.ToLower() == hoja.ToLower())
                        {
                            int columnsCount = reader.FieldCount;
                            while (reader.Read())
                            {
                                try
                                {
                                if (reader.Depth == 0)
                                {
                                    for (int i = 0; i < columnsCount; i++)
                                    {
                                        keysOrder.Add(reader.GetString(i));
                                    }
                                }
                                    if (reader.Depth > 0)
                                    {
                                        List<string> rowValues = new List<string>();
                                        Dictionary<string, string> rowsValuesDict = new Dictionary<string, string>();
                                        for (int i = 0; i < columnsCount; i++)
                                        {
                                            string svalue = string.Empty;
                                            if (!reader.IsDBNull(i))
                                            {
                                                svalue = reader.GetValue(i).ToString();
                                            }

                                            if (keysOrder[i] != null)
                                            {
                                                rowsValuesDict.Add(keysOrder[i], svalue);
                                            }
                                        }

                                        //allRows.Add(rowValues);
                                        allRowsDict.Add(rowsValuesDict);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                            }
                        }
                    } while (reader.NextResult());
                }
            }

            return new Tuple<List<Dictionary<string, string>>, List<string>>(allRowsDict, keysOrder);
        }

        public DateTime ObtenerFechaDeString(string fechastring, string formatoOrigen, DateTime? fecha, DateTime? hora)
        {
            DateTime result = new DateTime();
            switch (formatoOrigen)
            {
                case "dmy hora":
                    var fechahora = fechastring.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    var solofecha = fechahora[0].Split('/').Select(x => Convert.ToInt32(x)).ToArray();
                    var solohora = fechahora[1].Split(':').Select(x => Convert.ToInt32(x)).ToArray();
                    if (fecha.HasValue)
                    {

                    }
                    else if (hora.HasValue)
                    {

                    }
                    
                    result = new DateTime(solofecha[2], solofecha[1], solofecha[0]);
                    break;
            }

            return result;
        }
    }

    public static class Extensions
    {
        public static string RemoveLineBreaks(this string lines)
        {
            return lines.Replace("\r", string.Empty).Replace("\n", string.Empty);
        }

        public static string ReplaceLineBreaks(this string lines, string replacement)
        {
            return lines.Replace("\r\n", replacement)
                        .Replace("\r", replacement)
                        .Replace("\n", replacement);
        }
    }
}
