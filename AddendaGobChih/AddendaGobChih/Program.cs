using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Xml;

namespace AddendaGobChih
{
    class Program
    {
        static void Main(string[] args)
        {

            //Console.WriteLine("Enter Date : Format M/D/YYYY \n");


            Console.WriteLine("Para Iniciar presiona Enter");

            Console.ReadLine();


            //var DateInput = Console.ReadLine() + " 00:00:00";

            //DateTime myDate;


            try
            {

                //DateTime.TryParse(DateInput, out myDate);  

                string conString = AddendaGobChih.Properties.Settings.Default.ConnectionString;
                using (SqlConnection connection = new SqlConnection(conString))
                {

                    connection.Open();
                    DataTable dataTable = new DataTable();

                    using (SqlCommand command = new SqlCommand("SELECT id_cfd,xml  FROM tbl_cfd_Comprobantes_cat_tem WHERE id_cfd >= 780025", connection))
                    //using (SqlCommand command = new SqlCommand("SELECT id_cfd,xml  FROM tbl_cfd_Comprobantes_cat_tem WHERE id_cfd >= 328", connection))
                    {

                        //command.Parameters.Add(new SqlParameter("@FechaTimbrado", myDate));
                        command.CommandTimeout = 10800;

                        using (SqlDataReader reader = command.ExecuteReader()) 
                        {
                            if (reader != null)
                            { 
                                dataTable.Load(reader);
                            }
                        }
                    }

                    Int32 count = 0;

                    foreach (DataRow dr in dataTable.Rows)
                    {
                        count++;

                        XmlDocument pxComprobante = new XmlDocument();

                        XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(pxComprobante.NameTable);
                        nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");

                        Int32 IdCFD = Convert.ToInt32(dr["id_cfd"]);
                        string sAddenda = string.Empty;

                        pxComprobante.LoadXml(dr["xml"].ToString());

                        try { sAddenda = pxComprobante.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda", nsmComprobante).OuterXml; }
                        catch { }

                        if (sAddenda != string.Empty) 
                        {
                            try
                            {
                                using (SqlCommand cmd = new SqlCommand("INSERT INTO tblAddenda (IdCFD,Addenda) VALUES (@IdCFD,@sAddenda)", connection))
                                {
                                    cmd.Parameters.Add(new SqlParameter("@IdCFD", IdCFD));
                                    cmd.Parameters.Add(new SqlParameter("@sAddenda", sAddenda));
                                    cmd.ExecuteNonQuery();

                                }
                            }
                            catch { } 
                            
                        }

                        Console.WriteLine("Registro {0} de {1}", count, dataTable.Rows.Count);
                    }


      
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("err " + ex.Message);
            }

            Console.ReadLine();
        }
    }
}
