using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace ProyectoCoreStream.Models
{
    public class DocumentoDao
    {
        public Documento doc = new Documento();
        String rutaDescargar;

        public void Descargar(int id, String nombre)
        {
            doc = new Documento();
            conexionBd conexion = new conexionBd();
            conexion.abrir();

            try
            {
                SqlCommand comando = new SqlCommand("select doc from Documento where id=" + id + ";", conexion.conectarBd);
                SqlDataReader lector = comando.ExecuteReader();

                while (lector.Read())
                {
                    byte[] archivo = (byte[])lector["doc"];
                    doc.documentoPdf = new byte[archivo.Length];
                    doc.documentoPdf = archivo;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("fallo la base de datos");
            }
            conexion.cerrar();


          /*  long cantidad = doc.documentoPdf.Length;
            System.Diagnostics.Debug.WriteLine(cantidad);
            rutaDescargar =  "DownloadS/" + nombre + ".pdf";
            //rutaDescargar = Path.Combine("~/DownloadS/" + nombre + ".pdf");




            Stream fs2 = new FileStream(rutaDescargar, FileMode.Create, FileAccess.Write);

            for (long count = 0; count < cantidad; count++)
            {
                int valor = doc.documentoPdf[count];
                fs2.WriteByte((byte)valor);

            }
            fs2.Close();*/
        }


        public void Guardar(byte[] ms)
        {
            doc = new Documento();

            /* String FilePath;
             FilePath = HttpContext.Current.Server.MapPath(url);
             Stream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
             long cantidad = fs.Length;
             doc.documentoPdf = new byte[cantidad];

             for (long count = 0; count < cantidad; count++)
             {
                 fs.Seek(count, SeekOrigin.Begin);
                 int valor = fs.ReadByte();
                 doc.documentoPdf[count] = ((Byte)valor);
             }*/
            doc.documentoPdf = new byte[ms.Length];
            doc.documentoPdf = ms;
            System.Diagnostics.Debug.WriteLine(ms.Length);
            conexionBd conexion = new conexionBd();
            conexion.abrir();

            try
            {
                SqlCommand comando = new SqlCommand("Insert Into Documento(doc)values(@binaryValue)", conexion.conectarBd);
                comando.Parameters.Add("@binaryValue", System.Data.SqlDbType.VarBinary, doc.documentoPdf.Length).Value = doc.documentoPdf;
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("fallo la base de datos");
            }
            //ms.Close();
           // doc.documentoPdf = null;
            conexion.cerrar();
        }
    }
}
