using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoCoreStream.Models
{
    public class Documento
    {
        public String nombre { get; set; }
        public byte[] documentoPdf { get; set; }
    }
}
