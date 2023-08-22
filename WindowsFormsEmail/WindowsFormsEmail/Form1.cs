using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Mail;
using System.Net;

namespace WindowsFormsEmail
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
    
            SmtpClient SmtpServer = new SmtpClient("smtp.office365.com");  
            try
            {

                MailMessage mail = new MailMessage();       


                mail.From = new MailAddress("PAXMX_NoReply@kaltire.com");
                mail.To.Add("cesar.neg@gmail.com");
                mail.Subject = "Test Mail Gob";
                mail.Body = "Steleers 21 - Ravens 81";


                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("PAXMX_NoReply@kaltire.com", "Rin25bphT9PN");

                SmtpServer.EnableSsl = true;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


                SmtpServer.Send(mail);
                MessageBox.Show("mail Send");

                SmtpServer.Dispose();
            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                SmtpServer.Dispose();

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            IEnumerable<int> numbers = Enumerable.Range(101, 7);
            foreach (int n in numbers)
            {
                Console.WriteLine(n);
            }

            IEnumerable<int> numbers2 = Enumerable.Range(201,10);
            foreach (int n in numbers2)
            {
                Console.WriteLine(n);
            }
        }
    }
}
