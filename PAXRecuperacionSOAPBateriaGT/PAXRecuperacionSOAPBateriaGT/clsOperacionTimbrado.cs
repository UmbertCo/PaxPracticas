using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAXRecuperacionSOAPBateriaGT
{
    public class clsOperacionTimbrado
    {
        public clsOperacionTimbrado()
        {
        }

        public void fnGeneracionTimbrado()
        {
            DateTime Fecha = DateTime.Now;

            try
            {
                clsTimbrado Timbrado = new clsTimbrado();
                Timbrado.fnTimbrado();
            }
            catch (Exception ex)
            {
                clsLog.EscribirLog("Error fnGeneracionTimbrado - " + ex.Message);
            }
        }
    }    
}
