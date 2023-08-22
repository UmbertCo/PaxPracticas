using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;

namespace EncriptaProcedures
{
    class Program
    {
        static void Main(string[] args)
        {

            string DB = "";
            ServerConnection objServerCOnnection = new ServerConnection();
            objServerCOnnection.LoginSecure = false;
            Console.WriteLine("Enter name or IP Address of the Database Server.");
            objServerCOnnection.ServerInstance = Console.ReadLine();
            Console.WriteLine("Enter name of the Database");
            DB = Console.ReadLine();
            Console.WriteLine("Enter user id");
            objServerCOnnection.Login = Console.ReadLine();
            Console.WriteLine("Enter Password");
            objServerCOnnection.Password = Console.ReadLine();
            Console.WriteLine(" ");
            Server srv = new Server();
            try // Check to see if server connection details are ok.
            {
                srv = new Server(objServerCOnnection);
                if (srv == null)
                {
                    Console.WriteLine("Server details entered are wrong,"
                       + " Please restart the application");
                    Console.ReadLine();
                    System.Environment.Exit(System.Environment.ExitCode);
                }
            }
            catch
            {
                Console.WriteLine("Server details entered are wrong,"
                   + " Please restart the application");
                Console.ReadLine();
                System.Environment.Exit(System.Environment.ExitCode);
            }
            Database db = new Database();
            try // Check to see if database exists.
            {
                db = srv.Databases[DB];
                if (db == null)
                {
                    Console.WriteLine("Database does not exist on the current server,"
                       + " Please restart the application");
                    Console.ReadLine();
                    System.Environment.Exit(System.Environment.ExitCode);
                }
            }
            catch
            {
                Console.WriteLine("Database does not exist on the current server,"
                   + " Please restart the application");
                Console.ReadLine();
                System.Environment.Exit(System.Environment.ExitCode);
            }
            string allSP = "";

            for (int i = 0; i < db.StoredProcedures.Count; i++)
            {
                //Define a StoredProcedure object variable by supplying the parent database 
                //and name arguments in the constructor. 
                StoredProcedure sp;
                sp = new StoredProcedure();
                sp = db.StoredProcedures[i];
                if (!sp.IsSystemObject)// Exclude System stored procedures
                {
                    if (!sp.IsEncrypted) // Exclude already encrypted stored procedures
                    {
                        try
                        {
                            string text = "";// = sp.TextBody;
                            sp.TextMode = false;
                            sp.IsEncrypted = true;
                            sp.TextMode = true;
                            sp.Alter();

                            Console.WriteLine(sp.Name); // display name of the encrypted SP.
                            sp = null;
                            text = null;
                        }
                        catch (Exception ex)
                        {
                            
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
        }
    }
}
