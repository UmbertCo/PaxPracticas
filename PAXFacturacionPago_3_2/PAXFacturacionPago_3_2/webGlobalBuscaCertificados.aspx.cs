using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography.X509Certificates;

public partial class webGlobalBuscaCertificados : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        GetCertificateBySubject(DropDownList1.SelectedValue, DropDownList2.Text);
    }

    public void GetCertificateBySubject(string storage, string location)
    {
        var storageObj = new StoreName();
        var locationObj = new StoreLocation();

        if (StoreName.My.ToString() == storage)
        {
            storageObj = StoreName.My;
        }

        if (StoreName.Root.ToString() == storage)
        {
            storageObj = StoreName.Root;
        }

        if (StoreName.TrustedPeople.ToString() == storage)
        {
            storageObj = StoreName.TrustedPeople;
        }


        if (StoreLocation.CurrentUser.ToString() == location)
        {
            locationObj = StoreLocation.CurrentUser;
        }


        if (StoreLocation.LocalMachine.ToString() == location)
        {
            locationObj = StoreLocation.LocalMachine;
        }


        ListBox1.Items.Clear();
        var store = new X509Store(storageObj, locationObj);

        store.Open(OpenFlags.ReadOnly);

        var certificates = store.Certificates;
        foreach (var certificate in certificates)
        {
            var friendlyName = certificate.FriendlyName;
            var xname = certificate.GetName(); //obsolete
            Console.WriteLine(friendlyName);
            ListBox1.Items.Add(xname);
        }

        store.Close();




        //// Load the certificate from the certificate store.
        //X509Certificate2 cert = null;

        //X509Store store = new X509Store("My", StoreLocation.CurrentUser);

        //try
        //{
        //    // Open the store.
        //    store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

        //    // Get the certs from the store.
        //    X509Certificate2Collection CertCol = store.Certificates;

        //    // Find the certificate with the specified subject.
        //    foreach (X509Certificate2 c in CertCol)
        //    {
        //        //if (c.Subject.Contains(CertificateSubject))
        //        //{
        //        //    cert = c;
        //        //    break;
        //        //}
        //    }

        //    // Throw an exception of the certificate was not found.
        //    if (cert == null)
        //    {
        //        throw new CryptographicException("The certificate could not be found.");
        //    }
        //}
        //finally
        //{
        //    // Close the store even if an exception was thrown.
        //    store.Close();
        //}

        //return cert;


    }
}