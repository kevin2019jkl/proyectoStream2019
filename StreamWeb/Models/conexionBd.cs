using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
namespace StreamWeb.Models
{
    public class conexionBd
    {
        String cadena = "Data Source=DISI01030125\\SQLEXPRESS; Initial Catalog=PruebaBD; Integrated Security=True";
        public SqlConnection conectarBd = new SqlConnection();

        public conexionBd()
        {
            conectarBd.ConnectionString = cadena;
        }
        public void abrir()
        {
            try
            {
                conectarBd.Open();
                System.Diagnostics.Debug.WriteLine("se inicio conexion");

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("fallo enconexioBD");
            }
        }
        public void cerrar()
        {
            conectarBd.Close();
            System.Diagnostics.Debug.WriteLine("se cerro conexion");
        }
    }
}