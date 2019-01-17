using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Text;
using System.Net;

namespace StreamWeb.Models
{
    public class documentoDao
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
                SqlCommand comando = new SqlCommand("select doc from Documento where id="+id+";", conexion.conectarBd);
                SqlDataReader lector = comando.ExecuteReader() ;
                
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


            long cantidad = doc.documentoPdf.Length;
            System.Diagnostics.Debug.WriteLine(cantidad);
            rutaDescargar= HttpContext.Current.Server.MapPath("~/DownloadS/" + nombre + ".pdf");
           


           
           Stream fs2 = new FileStream(rutaDescargar, FileMode.Create, FileAccess.Write);

             for (long count = 0; count < cantidad; count++)
             {
                 int valor = doc.documentoPdf[count];
                 fs2.WriteByte((byte)valor);

             }
            fs2.Close();
        }


        public void Guardar(String url)
        {
            doc = new Documento();
         
             String FilePath;
            FilePath = HttpContext.Current.Server.MapPath(url);
            Stream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
            long cantidad = fs.Length;
           doc.documentoPdf = new byte[cantidad];
          
             for (long count = 0; count < cantidad; count++)
             {
                 fs.Seek(count, SeekOrigin.Begin);
                 int valor = fs.ReadByte();
                 doc.documentoPdf[count] = ((Byte)valor);
            }
         
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
            fs.Close();
            conexion.cerrar();
        }
    }
}





















// Stream fs = new FileStream("C: \\Users\\kcarrera\\Desktop\\verbos.pdf", FileMode.Open, FileAccess.Read);
/* for(long count = 0; count < cantidad; count++)
            {
                fs.Seek(count, SeekOrigin.Begin);
                int valor = fs.ReadByte();
                Console.WriteLine($"Posicion {count}: {(char)valor}");
            }*/

/*StreamReader sr = new StreamReader(fs);
while (!sr.EndOfStream)
{
    Console.WriteLine(sr.ReadLine());
}*/
// StreamWriter sw = new StreamWriter(fs2);

// SqlCommand comando = new SqlCommand(String.Format("selec * from Valores"));
//SqlCommand comando = new SqlCommand(String.Format("Insert Into Valores(valor1)values('nuevotexto')"));
//SqlCommand comando = new SqlCommand("Insert Into Valores(valor1)values('nuevotexto3')",conexion.conectarBd);
//SqlCommand comando = new SqlCommand(String.Format("Insert Into Documento(doc)values({0})", doc.documentoPdf));
//  System.Diagnostics.Debug.WriteLine("fallo la base de datos");
//byte[] arrayArchivo = doc.documentoPdf;
//guardo el array de bits en la base de datos
// System.Diagnostics.Debug.WriteLine(valor);
//guardo el archivo con un array de bits