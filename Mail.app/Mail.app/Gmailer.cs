using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.IO;
using System.Configuration;
using System.Collections.ObjectModel;

namespace Mail.app
{
    public class GMailer
    {

        public static string GmailUsername { get; set; }
        public static string GmailPassword { get; set; }
        public static string GmailHost { get; set; }
        public static int GmailPort { get; set; }
        public static bool GmailSSL { get; set; }

        public static string ToEmail { get; set; }
        public static string Subject { get; set; }
        public static string Body { get; set; }
        public static bool IsHtml { get; set; }

     
        static GMailer()
        {
            GmailHost = ConfigurationManager.AppSettings["smtpServer"];
            GmailPort = 587; // Gmail can use ports 25, 465 & 587; but must be 25 for medium trust environment.
            GmailSSL = true;
            //GmailUsername = ConfigurationManager.AppSettings["smtpUser"];
            //GmailPassword = ConfigurationManager.AppSettings["smtpPass"];
        }
        public static void Send(string  _entris, ObservableCollection<string> files)
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Host = GmailHost;
            smtp.Port = GmailPort;
            smtp.EnableSsl = GmailSSL;
           
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(GmailUsername, GmailPassword);

            using (var message = new MailMessage(GmailUsername, "apforms123@gmail.com"))
            {
               
                message.Subject = "AP Forms";
                message.Body = "<b>AP Forms Results</b>" + "\r\n\r\n" + _entris + "\r\n\r\n" + "Thanks & Regards,\r\n" + "<br/>" + GmailUsername;
                message.IsBodyHtml = true;
                foreach (var v in files)
                {
                    message.Attachments.Add(new Attachment(v));
                }
                smtp.Send(message);
            }
        }
       

      

    }
}