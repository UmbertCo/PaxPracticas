using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SolucionPruebas.Presentacion.MVC.Autenticacion.Models
{
    public class EmpresaModels : Controller
    {
        public int IdEmpresa { get; set; }

        public ActionResult Index()
        {
            return View();
        }

    }
}
