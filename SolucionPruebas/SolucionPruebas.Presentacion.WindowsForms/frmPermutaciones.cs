using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SolucionPruebas.Presentacion.WindowsForms
{
    public partial class frmPermutaciones : Form
    {
        public string Consultas = string.Empty;

        public frmPermutaciones()
        {
            InitializeComponent();
        }

        private void btnGenerarConsultas_Click(object sender, EventArgs e)
        {
            //Permuta("abc");
            fnPermuta("824,1,'G','HIEI830830G56','BBB',1,1000,'9050e0b4-0716-4556-9068-51365e4ec5e7',842");

        }

        public void fnPermuta(string psCaracteres)
        {
            string permutacion = string.Empty;
            bool[] marcas = new bool[psCaracteres.Split(',').Count()];
            fnPermuta(psCaracteres.Split(','), null, marcas);
        }

        public void fnPermuta(string[] psCarateresOriginales, string[] psPermutacion, bool[] marcas)
        {
            if (psPermutacion != null)
            {
                if (psCarateresOriginales.Length == psPermutacion.Length)
                {
                    txtResultado.Text += fnImprimir(psPermutacion);
                    txtResultado.Text += System.Environment.NewLine;
                }
            }

            for (int i = 0; i < marcas.Length; i++)
            {
                if (!marcas[i] == true)
                {
                    //Marcamos el caracter que vamos a permutar
                    marcas[i] = true;
                    //Invocamos al metodo recursivo añadiendo
                    //un caracter al string que permutamos
                    List<string> lsPermutaciones = new List<string>();

                    if (psPermutacion != null)
                    {
                        for (int j = 0; j < psPermutacion.Length; j++)
                        {
                            lsPermutaciones.Add(psPermutacion[j]);
                        }
                    }
                    
                    lsPermutaciones.Add(psCarateresOriginales[i]);

                    fnPermuta(psCarateresOriginales, lsPermutaciones.ToArray(), marcas);
                    //Desmarcamos el caracter para poder usarlo
                    //en otras combinaciones
                    marcas[i] = false;
                }
            }
        }

        public void fnRellenaParametros(string[] psCarateresOriginales, string[] psPermutacion, bool[] marcas)
        {
            if (psPermutacion != null)
            {
                if (psCarateresOriginales.Length == psPermutacion.Length)
                {
                    txtResultado.Text += fnImprimir(psPermutacion);
                    txtResultado.Text += System.Environment.NewLine;
                }
            }

            for (int i = 0; i < marcas.Length; i++)
            {
                if (!marcas[i] == true)
                {
                    //Marcamos el caracter que vamos a permutar
                    marcas[i] = true;
                    //Invocamos al metodo recursivo añadiendo
                    //un caracter al string que permutamos
                    List<string> lsPermutaciones = new List<string>();

                    if (psPermutacion != null)
                    {
                        for (int j = 0; j < psPermutacion.Length; j++)
                        {
                            lsPermutaciones.Add(psPermutacion[j]);
                        }
                    }

                    lsPermutaciones.Add(psCarateresOriginales[i]);

                    fnPermuta(psCarateresOriginales, lsPermutaciones.ToArray(), marcas);
                    //Desmarcamos el caracter para poder usarlo
                    //en otras combinaciones
                    marcas[i] = false;
                }
            }
        }

        public static void Permuta(string s)
        {
            //Iniciamos este array auxiliar para
            //marcar los caracteres que ya combinamos
            bool[] marcas = new bool[s.Length];
            //Llamamos al método recursivo
            Permuta(s, "", marcas);
        }

        static void Permuta(string original, string permutacion, bool[] marcas)
        {
            //Imprimimos la combinación si ya cambiamos
            //todas las letras una vez
            if (original.Length == permutacion.Length)
                MessageBox.Show(permutacion);
                //Console.WriteLine(permutacion);

            for (int i = 0; i < marcas.Length; i++)
            {
                //Vemos si está marcada para no volverla a permutar
                if (!marcas[i])
                {
                    //Marcamos el caracter que vamos a permutar
                    marcas[i] = true;
                    //Invocamos al metodo recursivo añadiendo
                    //un caracter al string que permutamos
                    Permuta(original, permutacion + original[i], marcas);
                    //Desmarcamos el caracter para poder usarlo
                    //en otras combinaciones
                    marcas[i] = false;
                }
            }
        }

        private string fnImprimir(string[] psArreglo)
        {
            string sParametros = string.Empty;
            try
            {
                for (int i = 0; i < psArreglo.Length; i++)
                {
                    sParametros += psArreglo[i];

                    if (i != psArreglo.Count() - 1)
                    {
                        sParametros += ",";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return sParametros;
        }
    }
}
