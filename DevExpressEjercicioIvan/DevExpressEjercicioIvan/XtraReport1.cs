﻿using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Globalization;
using System.Text;

namespace DevExpressEjercicioIvan
{
    public partial class XtraReport1 : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraReport1()
        {
            InitializeComponent();
        }

        private void xrtblCellCantidadLetra_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell CeldaTotal = (XRTableCell)sender;
            if (CeldaTotal.Text != "")
            {
                (sender as XRTableCell).Text = fnTextoImporte(CeldaTotal.Text, "MXN").ToUpper();
            }
        }

        private string fnTextoImporte(string psValor, string psMoneda)
        {
            CultureInfo languaje;
            NumaletNomina parser = new NumaletNomina();
            parser.LetraCapital = true;

            switch (psMoneda)
            {
                case "MXN":
                    parser.TipoMoneda = NumaletNomina.Moneda.Peso;
                    break;
                case "USD":
                    parser.TipoMoneda = NumaletNomina.Moneda.Dolar;
                    break;
                case "XEU":
                    parser.TipoMoneda = NumaletNomina.Moneda.Euro;
                    break;
            }

            languaje = new CultureInfo("es-Mx");

            return parser.ToCustomString(Convert.ToDouble(psValor, languaje));
        }

        public sealed class NumaletNomina
        {
            private const int UNI = 0, DIECI = 1, DECENA = 2, CENTENA = 3;
            private static string[,] _matriz = new string[CENTENA + 1, 10] 
        { 
            {null," uno", " dos", " tres", " cuatro", " cinco", " seis", " siete", " ocho", " nueve"}, 
            {" diez"," once"," doce"," trece"," catorce"," quince"," dieciseis"," diecisiete"," dieciocho"," diecinueve"}, 
            {null,null,null," treinta"," cuarenta"," cincuenta"," sesenta"," setenta"," ochenta"," noventa"}, 
            {null,null,null,null,null," quinientos",null," setecientos",null," novecientos"}
        };

            //*********************************************
            //Código agregado por Ivan Lopez - 21 de Abril 2011
            //Esta propiedad permite definir la moneda que será usada
            //Asignando automáticamente el separadorDecimal, cultura y la abreviación de moneda
            //En la parte final de la cadena
            //NOTA: se deja la cultura a la de méxico pues es aquí donde se forma la cadena
            private string _abvMoneda;
            private Moneda _tipoMoneda;
            public Moneda TipoMoneda
            {
                get
                {
                    return _tipoMoneda;
                }
                set
                {
                    switch (value)
                    {
                        case Moneda.Peso:
                            _tipoMoneda = value;
                            _abvMoneda = "MXN";
                            _separadorDecimalSalida = "pesos";
                            _cultureInfo = new CultureInfo("es-MX");
                            break;
                        case Moneda.Dolar:
                            _tipoMoneda = value;
                            _abvMoneda = "USD";
                            _separadorDecimalSalida = "dólares";
                            _cultureInfo = new CultureInfo("es-MX");
                            break;
                        case Moneda.Euro:
                            _tipoMoneda = value;
                            _abvMoneda = "XEU";
                            _separadorDecimalSalida = "euros";
                            _cultureInfo = new CultureInfo("es-MX");
                            break;
                    }
                }
            }

            public enum Moneda
            {
                Peso,
                Dolar,
                Euro
            }
            //*****************************************

            #region Miembros estáticos

            private const Char sub = (Char)26;
            //Cambiar acá si se quiere otro comportamiento en los métodos de clase
            public const String SeparadorDecimalSalidaDefault = "con";
            public const String MascaraSalidaDecimalDefault = "00'/100.-'";
            public const Int32 DecimalesDefault = 2;
            public const Boolean LetraCapitalDefault = false;
            public const Boolean ConvertirDecimalesDefault = false;
            public const Boolean ApocoparUnoParteEnteraDefault = false;
            public const Boolean ApocoparUnoParteDecimalDefault = false;

            #endregion

            #region Propiedades de instancia

            private Int32 _decimales = DecimalesDefault;
            private CultureInfo _cultureInfo = CultureInfo.CurrentCulture;
            private String _separadorDecimalSalida = SeparadorDecimalSalidaDefault;
            private Int32 _posiciones = DecimalesDefault;
            private String _mascaraSalidaDecimal, _mascaraSalidaDecimalInterna = MascaraSalidaDecimalDefault;
            private Boolean _esMascaraNumerica = true;
            private Boolean _letraCapital = LetraCapitalDefault;
            private Boolean _convertirDecimales = ConvertirDecimalesDefault;
            private Boolean _apocoparUnoParteEntera = false;
            private Boolean _apocoparUnoParteDecimal;

            /// <summary>
            /// Indica la cantidad de decimales que se pasarán a entero para la conversión
            /// </summary>
            /// <remarks>Esta propiedad cambia al cambiar MascaraDecimal por un valor que empieze con '0'</remarks>
            public Int32 Decimales
            {
                get { return _decimales; }
                set
                {
                    if (value > 10) throw new ArgumentException(value.ToString() + " excede el número máximo de decimales admitidos, solo se admiten hasta 10.");
                    _decimales = value;
                }
            }

            /// <summary>
            /// Objeto CultureInfo utilizado para convertir las cadenas de entrada en números
            /// </summary>
            public CultureInfo CultureInfo
            {
                get { return _cultureInfo; }
                set { _cultureInfo = value; }
            }

            /// <summary>
            /// Indica la cadena a intercalar entre la parte entera y la decimal del número
            /// </summary>
            public String SeparadorDecimalSalida
            {
                get { return _separadorDecimalSalida; }
                set
                {
                    _separadorDecimalSalida = value;
                    //Si el separador decimal es compuesto, infiero que estoy cuantificando algo,
                    //por lo que apocopo el "uno" convirtiéndolo en "un"
                    if (value.Trim().IndexOf(" ") > 0)
                        _apocoparUnoParteEntera = true;
                    else _apocoparUnoParteEntera = false;
                }
            }

            /// <summary>
            /// Indica el formato que se le dara a la parte decimal del número
            /// </summary>
            public String MascaraSalidaDecimal
            {
                get
                {
                    if (!String.IsNullOrEmpty(_mascaraSalidaDecimal))
                        return _mascaraSalidaDecimal;
                    else return "";
                }
                set
                {
                    //determino la cantidad de cifras a redondear a partir de la cantidad de '0' o '#' 
                    //que haya al principio de la cadena, y también si es una máscara numérica
                    int i = 0;
                    while (i < value.Length
                        && (value[i] == '0')
                            | value[i] == '#')
                        i++;
                    _posiciones = i;
                    if (i > 0)
                    {
                        _decimales = i;
                        _esMascaraNumerica = true;
                    }
                    else _esMascaraNumerica = false;
                    _mascaraSalidaDecimal = value;
                    if (_esMascaraNumerica)
                        _mascaraSalidaDecimalInterna = value.Substring(0, _posiciones) + "'"
                            + value.Substring(_posiciones)
                            .Replace("''", sub.ToString())
                            .Replace("'", String.Empty)
                            .Replace(sub.ToString(), "'") + "'";
                    else
                        _mascaraSalidaDecimalInterna = value
                            .Replace("''", sub.ToString())
                            .Replace("'", String.Empty)
                            .Replace(sub.ToString(), "'");
                }
            }

            /// <summary>
            /// Indica si la primera letra del resultado debe estár en mayúscula
            /// </summary>
            public Boolean LetraCapital
            {
                get { return _letraCapital; }
                set { _letraCapital = value; }
            }

            /// <summary>
            /// Indica si se deben convertir los decimales a su expresión nominal
            /// </summary>
            public Boolean ConvertirDecimales
            {
                get { return _convertirDecimales; }
                set
                {
                    _convertirDecimales = value;
                    _apocoparUnoParteDecimal = value;
                    if (value)
                    {// Si la máscara es la default, la borro
                        if (_mascaraSalidaDecimal == MascaraSalidaDecimalDefault)
                            MascaraSalidaDecimal = "";
                    }
                    else if (String.IsNullOrEmpty(_mascaraSalidaDecimal))
                        //Si no hay máscara dejo la default
                        MascaraSalidaDecimal = MascaraSalidaDecimalDefault;
                }
            }

            /// <summary>
            /// Indica si de debe cambiar "uno" por "un" en las unidades.
            /// </summary>
            public Boolean ApocoparUnoParteEntera
            {
                get { return _apocoparUnoParteEntera; }
                set { _apocoparUnoParteEntera = value; }
            }

            /// <summary>
            /// Determina si se debe apococopar el "uno" en la parte decimal
            /// </summary>
            /// <remarks>El valor de esta propiedad cambia al setear ConvertirDecimales</remarks>
            public Boolean ApocoparUnoParteDecimal
            {
                get { return _apocoparUnoParteDecimal; }
                set { _apocoparUnoParteDecimal = value; }
            }

            #endregion

            #region Constructores

            public NumaletNomina()
            {
                MascaraSalidaDecimal = MascaraSalidaDecimalDefault;
                SeparadorDecimalSalida = SeparadorDecimalSalidaDefault;
                LetraCapital = LetraCapitalDefault;
                ConvertirDecimales = _convertirDecimales;
            }

            public NumaletNomina(Boolean ConvertirDecimales, String MascaraSalidaDecimal, String SeparadorDecimalSalida, Boolean LetraCapital)
            {
                if (!String.IsNullOrEmpty(MascaraSalidaDecimal))
                    this.MascaraSalidaDecimal = MascaraSalidaDecimal;
                if (!String.IsNullOrEmpty(SeparadorDecimalSalida))
                    _separadorDecimalSalida = SeparadorDecimalSalida;
                _letraCapital = LetraCapital;
                _convertirDecimales = ConvertirDecimales;
            }

            #endregion

            #region Conversores de instancia

            public String ToCustomString(Double Numero)
            {
                return Convertir((Decimal)Numero,
                    _decimales, _separadorDecimalSalida,
                    _mascaraSalidaDecimalInterna,
                    _esMascaraNumerica,
                    _letraCapital,
                    _convertirDecimales,
                    _apocoparUnoParteEntera,
                    _apocoparUnoParteDecimal,
                    _abvMoneda);
            }

            public String ToCustomString(String Numero)
            {
                Double dNumero;
                if (Double.TryParse(Numero, NumberStyles.Float, _cultureInfo, out dNumero))
                    return ToCustomString(dNumero);
                else throw new ArgumentException("'" + Numero + "' no es un número válido.");
            }

            public String ToCustomString(Decimal Numero)
            { return ToString(Convert.ToDouble(Numero)); }

            public String ToCustomString(Int32 Numero)
            { return Convertir((Decimal)Numero, 0, _separadorDecimalSalida, _mascaraSalidaDecimalInterna, _esMascaraNumerica, _letraCapital, _convertirDecimales, _apocoparUnoParteEntera, false); }

            #endregion

            #region Conversores estáticos

            public static String ToString(Int32 Numero)
            {
                return Convertir((Decimal)Numero, 0, null, null, true, LetraCapitalDefault, ConvertirDecimalesDefault, ApocoparUnoParteEnteraDefault, ApocoparUnoParteDecimalDefault);
            }

            public static String ToString(Double Numero)
            { return Convertir((Decimal)Numero, DecimalesDefault, SeparadorDecimalSalidaDefault, MascaraSalidaDecimalDefault, true, LetraCapitalDefault, ConvertirDecimalesDefault, ApocoparUnoParteEnteraDefault, ApocoparUnoParteDecimalDefault); }

            public static String ToString(String Numero, CultureInfo ReferenciaCultural)
            {
                Double dNumero;
                if (Double.TryParse(Numero, NumberStyles.Float, ReferenciaCultural, out dNumero))
                    return ToString(dNumero);
                else throw new ArgumentException("'" + Numero + "' no es un número válido.");
            }

            public static String ToString(String Numero)
            {
                return NumaletNomina.ToString(Numero, CultureInfo.CurrentCulture);
            }

            public static String ToString(Decimal Numero)
            { return ToString(Convert.ToDouble(Numero)); }

            #endregion

            //Sobrecargar hecha por Ivan Lopez para no tener que modificar todas los métodos
            private static String Convertir(Decimal Numero, Int32 Decimales, String SeparadorDecimalSalida, String MascaraSalidaDecimal, Boolean EsMascaraNumerica, Boolean LetraCapital, Boolean ConvertirDecimales, Boolean ApocoparUnoParteEntera, Boolean ApocoparUnoParteDecimal)
            {
                return Convertir(Numero, Decimales, SeparadorDecimalSalida, MascaraSalidaDecimal, EsMascaraNumerica, LetraCapital, ConvertirDecimales, ApocoparUnoParteEntera, ApocoparUnoParteDecimal, string.Empty);
            }

            private static String Convertir(Decimal Numero, Int32 Decimales, String SeparadorDecimalSalida, String MascaraSalidaDecimal, Boolean EsMascaraNumerica, Boolean LetraCapital, Boolean ConvertirDecimales, Boolean ApocoparUnoParteEntera, Boolean ApocoparUnoParteDecimal, string _Moneda)
            {
                Int64 Num;
                Int32 terna, pos, centenaTerna, decenaTerna, unidadTerna, iTerna;
                String numcad, cadTerna;
                StringBuilder Resultado = new StringBuilder();

                Num = (Int64)Math.Abs(Numero);

                if (Num >= 1000000000000 || Num < 0) throw new ArgumentException("El número '" + Numero.ToString() + "' excedió los límites del conversor: [0;1.000.000.000.000)");
                if (Num == 0)
                    Resultado.Append(" cero");
                else
                {
                    numcad = Num.ToString();
                    iTerna = 0;
                    pos = numcad.Length;

                    do //Se itera por las ternas de atrás para adelante
                    {
                        iTerna++;
                        cadTerna = String.Empty;
                        if (pos >= 3)
                            terna = Int32.Parse(numcad.Substring(pos - 3, 3));
                        else
                            terna = Int32.Parse(numcad.Substring(0, pos));

                        centenaTerna = (Int32)(terna / 100);
                        decenaTerna = terna - centenaTerna * 100;
                        unidadTerna = (decenaTerna - (Int32)(decenaTerna / 10) * 10);

                        if ((decenaTerna > 0) && (decenaTerna < 10))
                            cadTerna = _matriz[UNI, unidadTerna] + cadTerna;
                        else if ((decenaTerna >= 10) && (decenaTerna < 20))
                            cadTerna = cadTerna + _matriz[DIECI, decenaTerna - (Int32)(decenaTerna / 10) * 10];
                        else if (decenaTerna == 20)
                            cadTerna = cadTerna + " veinte";
                        else if ((decenaTerna > 20) && (decenaTerna < 30))
                            cadTerna = " veinti" + _matriz[UNI, unidadTerna].Substring(1, _matriz[UNI, unidadTerna].Length - 1);
                        else if ((decenaTerna >= 30) && (decenaTerna < 100))
                            if (unidadTerna != 0)
                                cadTerna = _matriz[DECENA, (Int32)(decenaTerna / 10)] + " y" + _matriz[UNI, unidadTerna] + cadTerna;
                            else
                                cadTerna += _matriz[DECENA, (Int32)(decenaTerna / 10)];

                        switch (centenaTerna)
                        {
                            case 1:
                                if (decenaTerna > 0) cadTerna = " ciento" + cadTerna;
                                else cadTerna = " cien" + cadTerna;
                                break;
                            case 5:
                            case 7:
                            case 9:
                                cadTerna = _matriz[CENTENA, (Int32)(terna / 100)] + cadTerna;
                                break;
                            default:
                                if ((Int32)(terna / 100) > 1) cadTerna = _matriz[UNI, (Int32)(terna / 100)] + "cientos" + cadTerna;
                                break;
                        }
                        //Reemplazo el 'uno' por 'un' si no es en las únidades o si se solicító apocopar
                        if ((iTerna > 1 | ApocoparUnoParteEntera) && decenaTerna == 21)
                            cadTerna = cadTerna.Replace("veintiuno", "veintiún");
                        else if ((iTerna > 1 | ApocoparUnoParteEntera) && unidadTerna == 1 && decenaTerna != 11)
                            cadTerna = cadTerna.Substring(0, cadTerna.Length - 1);
                        //Acentúo 'dieciseís', 'veintidós', 'veintitrés' y 'veintiséis'
                        else if (decenaTerna == 16) cadTerna = cadTerna.Replace("dieciseis", "dieciséis");
                        else if (decenaTerna == 22) cadTerna = cadTerna.Replace("veintidos", "veintidós");
                        else if (decenaTerna == 23) cadTerna = cadTerna.Replace("veintitres", "veintitrés");
                        else if (decenaTerna == 26) cadTerna = cadTerna.Replace("veintiseis", "veintiséis");
                        //Reemplazo 'uno' por 'un' si no es en las únidades o si se solicító apocopar (si _apocoparUnoParteEntera es verdadero) 
                        switch (iTerna)
                        {
                            case 3:
                                if (Num < 2000000) cadTerna += " millón";
                                else cadTerna += " millones";
                                break;
                            case 2:
                            case 4:
                                if (terna > 0) cadTerna += " mil";
                                break;
                        }
                        Resultado.Insert(0, cadTerna);
                        pos = pos - 3;
                    } while (pos > 0);
                }
                //Se agregan los decimales si corresponde
                if (Decimales > 0)
                {
                    Resultado.Append(" " + SeparadorDecimalSalida + " ");
                    Int32 EnteroDecimal = (Int32)Math.Round((Double)(Numero - (Int64)Numero) * Math.Pow(10, Decimales), 0);

                    if (ConvertirDecimales)
                    {
                        Boolean esMascaraDecimalDefault = MascaraSalidaDecimal == MascaraSalidaDecimalDefault;
                        Resultado.Append(Convertir((Decimal)EnteroDecimal, 0, null, null, EsMascaraNumerica, false, false, (ApocoparUnoParteDecimal && !EsMascaraNumerica/*&& !esMascaraDecimalDefault*/), false) + " "
                            + (EsMascaraNumerica ? "" : MascaraSalidaDecimal));
                    }
                    else
                        if (EsMascaraNumerica) Resultado.Append(EnteroDecimal.ToString(MascaraSalidaDecimal));
                        else Resultado.Append(EnteroDecimal.ToString() + " " + MascaraSalidaDecimal);
                }

                //Código añadido por Ivan Lopez - 21 de Abril de 2011 - Agregamos la abreviación de moneda al final de la candena
                Resultado.Append(_Moneda);

                //Se pone la primer letra en mayúscula si corresponde y se retorna el resultado
                if (LetraCapital)
                    return Resultado[1].ToString().ToUpper() + Resultado.ToString(2, Resultado.Length - 2);
                else
                    return Resultado.ToString().Substring(1);
            }
        }
    }
}
