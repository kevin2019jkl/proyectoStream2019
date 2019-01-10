using StreamWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;



namespace StreamWeb.Controllers
{
    public class DocumentoController : Controller
    {
        // GET: Documento
        public ActionResult Index()
        {
            String ruta = Server.MapPath("~/DownloadS");
            String ruta2 = Server.MapPath("~/Uploads");
            DeleteFolderContent(ruta);
            DeleteFolderContent(ruta2);
            return View();
        }
        public ActionResult Guardar()
        {
            return View();
        }
        public ActionResult Descargar()
        {
            return View();
        }

        public ActionResult AccionGuardar()
        {
            documentoDao dao = new documentoDao();
           
           return View();
        }
        [HttpPost]
        public void subir(HttpPostedFileBase file)
        {
            System.Diagnostics.Debug.Write(file.FileName);
                 documentoDao dao = new documentoDao();
                 if (file == null) return;
                 string archivo = (DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + file.FileName).ToLower();
                 file.SaveAs(Server.MapPath("~/Uploads/" + archivo));
                 dao.Guardar("~/Uploads/" + archivo);
        }
        
        public FileResult AccionDescargar()
        {
            documentoDao dao = new documentoDao();
            dao.Descargar(Convert.ToInt32(Request.Form["id"]), "nombre");
           // string nombre = "archivo.pdf";
            String ruta =  Server.MapPath("~/DownloadS/" + "nombre" + ".pdf");
            // return File(ruta, "application/pdf", Request.Form["nombre"].ToString() + ".pdf");
            return File(ruta, "application/pdf", "");
        }







        //metodos para limpiar los directorios despues de guardar un archivo en la base de datos
        public static void DeleteFolderContent(string fullPath)
        {
            DeleteFolder(fullPath);
            CreateEmptyDirectory(fullPath);
        }
        public static void CreateEmptyDirectory(string fullPath)
        {
            if (!System.IO.Directory.Exists(fullPath))
            {
                System.IO.Directory.CreateDirectory(fullPath);
            }
        }
        public static void DeleteFolder(string fullPath)
        {
            if (System.IO.Directory.Exists(fullPath))
            {
                System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(fullPath)
                {
                    Attributes = System.IO.FileAttributes.Normal
                };

                foreach (var info in directory.GetFileSystemInfos("*", System.IO.SearchOption.AllDirectories))
                {
                    System.IO.FileInfo fInfo = info as System.IO.FileInfo;
                    if (fInfo != null) info.Attributes = System.IO.FileAttributes.Normal;
                }
                System.Threading.Thread.Sleep(100);
                directory.Delete(true);
            }

        }
    }
}