using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace SolucionPruebas.Servicios.Modulos
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class JSONService : IJSONService
    {
        public string XMLData(string psId)
        {
            return "Requeriste el producto " + psId;
        }

        public string JSONData(string psId)
        {
            return "Requeriste el producto " + psId;
        }

        public string PruebaJson(string psId)
        {
            return "Requeriste el producto " + psId;
        }
    }
}
