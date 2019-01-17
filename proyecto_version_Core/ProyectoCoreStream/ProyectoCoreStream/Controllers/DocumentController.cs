using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ProyectoCoreStream.Models;

namespace ProyectoCoreStream.Controllers
{
   
    public class DocumentController : Controller
    {

        private readonly IHostingEnvironment _env;

        public DocumentController(IHostingEnvironment env)
        {
            _env = env;
       }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Descargar()
        {
            return View();
        }
        //metodo que llama al formulario para insertar el id de documento a descargar
        public FileResult AccionDescargar()
        {
             DocumentoDao dao = new DocumentoDao();
            dao.Descargar(Convert.ToInt32(Request.Form["id"]), "nombre");
           
            return File(dao.doc.documentoPdf, "application/pdf", "");
           // return View();
        }
        //metodo que descarga el array de bytes de la base de datos y lo muestra como un pdf
        public IActionResult Reporte()
        {
            FileStream fs = new FileStream(@"C:\Users\kcarrera\Desktop\reporte.pdf", FileMode.Create);
            Document document = new Document(iTextSharp.text.PageSize.LETTER);
            PdfWriter pw =  PdfWriter.GetInstance(document,fs);

            document.Open();
            document.Add(new Paragraph("Hola Pdf \n trabajando con archivos pdf"));
            document.Close();
            return null;
        }
        //metodo que invoca al formulario para llenar pdf
        public IActionResult FormReporte()
        {
            return View();
        }

        //metodo que genera pdf modificado con itextsharp
        public IActionResult ReporteMemoryStream()
        {
            MemoryStream ms = new MemoryStream();
            Document document = new Document(new Rectangle(1270f, 805f), 0, 0, 0, 0);
            PdfWriter pw =  PdfWriter.GetInstance(document,ms);
            document.Open();          
            PdfContentByte cb = pw.DirectContent;

            BaseFont bf2 = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            cb.SetColorFill(BaseColor.BLACK);           
            cb.SetFontAndSize(bf2, 32);

            cb.BeginText();
            cb.ShowTextAligned(3, "Nit Del Patrono:", 115, 450, 0);
            cb.ShowTextAligned(3, "Nombre Del Patrono:", 115, 380, 0);
            cb.ShowTextAligned(3, "Cantidad De Folios A Autorizar:", 115, 310, 0);
            cb.ShowTextAligned(3, "Fecha De Solicitud:", 115, 240, 0);
            cb.ShowTextAligned(3, "Fecha De Autorizacion:", 115, 170, 0);           
            cb.EndText();
           
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            cb.SetColorFill(BaseColor.DARK_GRAY);
            cb.SetFontAndSize(bf, 32);
          
            cb.BeginText();         
            cb.ShowTextAligned(3, Request.Form["codigo"].ToString(), 300, 517, 0);
            cb.ShowTextAligned(3, Request.Form["nitPatrono"].ToString(), 345, 450, 0);
            cb.ShowTextAligned(3, Request.Form["patrono"].ToString(), 415, 380, 0);
            cb.ShowTextAligned(3, Request.Form["cantidad"].ToString(), 565, 310, 0);
            cb.ShowTextAligned(3, Request.Form["fecha1"].ToString(), 385, 240, 0);
            cb.ShowTextAligned(3, Request.Form["fecha2"].ToString(), 435, 170, 0);
            cb.EndText();
         
                     
            using (var imageStream = new FileStream("Sello_Dirección_Gral_de_Trbajo-01.jpg", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
             {
                    var image = Image.GetInstance(imageStream);
                 document.Add(image );
            }


            using (var imageStream = new FileStream("qr.png", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var image = Image.GetInstance(imageStream);
                 image.SetAbsolutePosition(850, 80);
                 cb.AddImage(image);              
            }
                document.Close();


            byte[] bytesStream = ms.ToArray();          
            DocumentoDao dao = new DocumentoDao();
            dao.Guardar(bytesStream);
            return null;
        }
    }
}