using System.Text.RegularExpressions;
using System;
using System.Globalization;

public class RFC
{
    public static bool EsRFCValido(string RFC)
    {
        if (PersonaFisica_RFC(RFC) && ValidarUltimaLetraRFC(RFC) && ValidarFechaRFC(RFC))
            return true;

        if (General_RFC(RFC) && ValidarUltimaLetraRFC(RFC) && ValidarFechaRFC(RFC))
            return true;

        return false;
    }

    private static bool PersonaFisica_RFC(string RFC)
    {
        try
        {
            if (RFC.Trim().Length == 13 || RFC.Trim().Length == 12)
            {
                string pattern = "^[A-Z&]{3,4}(\\d{6})(([A-Z0-9]){3})?$";
                Match RFCMatch = Regex.Match(RFC, pattern);
                if (RFC != string.Empty)
                {
                    if (!RFCMatch.Success)
                        return false;
                }
            }
            else
                return false;

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(string.Format("ExpRegulares.PersonaFisica_RFC: {0}", ex.Message));
            return false;
        }
    }

    private static bool General_RFC(string RFC)
    {
        try
        {
            string pattern = "^([A-Z\\s]{3,4})\\d{6}(([A-Z\\w]|[0-9]){3})$";
            Match RFCMatch = Regex.Match(RFC.Trim(), pattern);
            if (RFC.Trim() != string.Empty)
            {
                if (!RFCMatch.Success)
                    return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(string.Format("ExpRegulares.General_RFC: {0}", ex.Message));
            return false;
        }
    }

    private static bool ValidarUltimaLetraRFC(string RFC)
    {
        try
        {
            if (RFC.EndsWith("A") || RFC.EndsWith("0") || RFC.EndsWith("1") || RFC.EndsWith("2") || RFC.EndsWith("3") || RFC.EndsWith("4") || RFC.EndsWith("5")
                || RFC.EndsWith("6") || RFC.EndsWith("7") || RFC.EndsWith("8") || RFC.EndsWith("9"))
                return true;
            else
                return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine(string.Format("ExpRegulares.ValidarUltimaLetraRFC: {0}", ex.Message));
            return false;
        }
    }

    private static bool ValidarFechaRFC(string RFC)
    {
        string fecha = string.Empty;
        bool esFecha = false;
        DateTime resultado;

        fecha = RFC.Substring(RFC.Length - 9, 6);
        esFecha = DateTime.TryParseExact(fecha, "yyMMdd", null, DateTimeStyles.None, out resultado);

        return esFecha;
    }
}