using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SolucionPruebas.Presentacion.MVC.Pelicula.Controllers
{
    public class HelloWorldController : Controller
    {
        //
        // GET: /HelloWorld/

        public string Index()
        {
            return "This is my <b> action ...";
        }

        public override string Welcome()
        {
            return "This is the Welcome action method...";
        }

        public override string Welcome(string psNombre, string psNumTimes)
        {
            return HttpUtility.HtmlEncode("Hola " + psNombre + ", NumTimes is: " + psNumTimes);
        }
    }
}
