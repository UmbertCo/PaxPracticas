using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;

namespace P_ConectorBepensa
{
    enum EnuTipoScript { INS,SEL }

    

    class clsCorrerSP_Scripts
    {
        int _nResultado;
        public int nResultado { get { return _nResultado; } }
        public string sValorDevuelto { set; get; }
        public string sBDConexion { set; get; }
        public string sRutaScript { set; get; }
        public DataTable _tblRes= new DataTable();
        public DataTable tblResultado { get { return _tblRes; } }
        public EnuTipoScript tsTipo;
        public clsTbl_Facturas tfParametrosEntrada { set; get; }


        public clsCorrerSP_Scripts(String psConnectionString,String psRutaScript,EnuTipoScript pTipoScript) 
        {
            sBDConexion = psConnectionString;

            if (psRutaScript[0] == '/')
            {
                if (psRutaScript.Length < 2) throw new Exception("Ruta Incorrecta Scripts");


                sRutaScript = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + psRutaScript;

                tsTipo = pTipoScript;

            }
            else 
            {

                sRutaScript =  psRutaScript;

                tsTipo = pTipoScript;
            
            }//Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        
        
        }
        
        public static  String[] fnObtenerEquivalencias(String sCadena, string sReg) 
        {

            MatchCollection mEquivs = Regex.Matches(sCadena, sReg);

            List<string> sParametros = new List<string>();

            foreach (Match mEquiv in mEquivs)
            {
                sParametros.Add( mEquiv.Value);


            }

            return sParametros.ToArray();
        }

        public static int[] fnObenerPrimerCoordenada(String sCadena, string sReg) 
        {
            int[] nCoordenada = { -1, -1 };

            Match mRes = Regex.Match(sCadena, sReg);

            if (mRes.Success)
            {
                nCoordenada[0] = mRes.Index;
                nCoordenada[1] = mRes.Index + sCadena.Length;
            }

            return nCoordenada;
        }
        
        public void fnProc() 
        {
            string sScript=String.Empty;

            try
            {
                sScript = File.ReadAllText(sRutaScript, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrio un error al leer el Script " + tsTipo.ToString() + " Mensaje: " + ex.Message);
            
            }
            DataTable dtSel = new DataTable();

            using (SqlConnection scConexion = new SqlConnection(sBDConexion)) 
            {
                try
                {
                    scConexion.Open();

                    using (SqlCommand scoComando = new SqlCommand())
                    {
                        scoComando.Connection = scConexion;

                        string[] sValores;
                        String[] sColumnas;
                        string sColumnasIN;

                        switch (tsTipo)
                        {

                            case EnuTipoScript.INS:

                                #region INS
                                sValores = fnObtenerEquivalencias(sScript, @"\@\w+");

                                sColumnas = new string[sValores.Length];

                                Array.Copy(sValores, sColumnas, sValores.Length);

                                for (int i = 0; i < sColumnas.Length; i++)
                                {

                                    string sAux = sColumnas[i];

                                    sColumnas[i] = sAux.Remove(0, 1);

                                }

                                sColumnasIN = "";

                                foreach (string sColumna in sColumnas)
                                {
                                    sColumnasIN += "," + sColumna;


                                }

                                sColumnasIN = sColumnasIN.Substring(1);

                                sScript = sScript.Replace("--no modificar--", sColumnasIN);

                                scoComando.CommandText = sScript;

                                scoComando.CommandType = CommandType.Text;

                                for (int i = 0; i < sValores.Length; i++)
                                {
                                    String sValor = sValores[i];
                                    String sColumna = sColumnas[i];
                                    object oEntrada = tfParametrosEntrada.fnObtenerParametro(sColumna) == null  ? DBNull.Value : tfParametrosEntrada.fnObtenerParametro(sColumna);

                                    scoComando.Parameters.Add(sValor, oEntrada);
                                }

                                _nResultado = scoComando.ExecuteNonQuery();

                                break;

                                #endregion

                            case EnuTipoScript.SEL:

                                #region SEL


                                sValores = fnObtenerEquivalencias(sScript, @"\@\w+");

                                sColumnas = new string[sValores.Length];

                                Array.Copy(sValores, sColumnas, sValores.Length);

                                for (int i = 0; i < sColumnas.Length; i++)
                                {

                                    string sAux = sColumnas[i];

                                    sColumnas[i] = sAux.Remove(0, 1);

                                }

                                sColumnasIN = "";

                                foreach (string sColumna in sColumnas)
                                {
                                    sColumnasIN += "," + sColumna;


                                }

                                sColumnasIN = sColumnasIN.Substring(1);

                                sScript = sScript.Replace("--no modificar--", sColumnasIN);

                                scoComando.CommandText = sScript;

                                scoComando.CommandType = CommandType.Text;

                                for (int i = 0; i < sValores.Length; i++)
                                {
                                    String sValor = sValores[i];
                                    String sColumna = sColumnas[i];
                                    object oEntrada = tfParametrosEntrada.fnObtenerParametro(sColumna) == null ? DBNull.Value : tfParametrosEntrada.fnObtenerParametro(sColumna);
                                    

                                    scoComando.Parameters.Add(sValor, oEntrada);

                                }

                                SqlDataReader sdrLector = scoComando.ExecuteReader(CommandBehavior.SingleRow);

                                sdrLector.Read();

                                if (sdrLector.HasRows)
                                {
                                    sValorDevuelto = sdrLector[sColumnas[0]].ToString();

                                    _nResultado = 1;
                                }
                                else
                                    _nResultado = 0;

                                break;
                                #endregion

                        }



                    }
                }
                catch(Exception ex) 
                {

                    throw new Exception("Ocurrio un error al acceder a la Base de datos " + tsTipo.ToString() + " Mensaje: " + ex.Message);
                }

                finally 
                {
                    scConexion.Dispose();
                
                }
            
            }
        
        
        }
    }
}
