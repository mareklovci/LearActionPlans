using Microsoft.Win32;
using System;
using System.Data;
using System.Net.Mail;
using System.Windows.Forms;

namespace LearActionPlans.Utilities
{
    public class Helper
    {
        public static DataTable dtAP;

        public static byte OdeslatEmail(SmtpClient smtp, MailMessage message)
        {
            byte stav;
            try
            {
                //pro Lear to uložím do souboru
                smtp.Send(message);
                MessageBox.Show(@"Email was sent", @"Email", MessageBoxButtons.OK, MessageBoxIcon.Information);
                stav = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                stav = 2;
            }

            return stav;
        }

        public static void RegisterMyProtocol(string protocol, string myAppPath)  //myAppPath = full path to your application
        {
            //RegistryKey key = Registry.ClassesRoot.OpenSubKey("LearActionPlans");  //open myApp protocol's subkey
            RegistryKey key = Registry.ClassesRoot.OpenSubKey(protocol);  //open myApp protocol's subkey
            //RegistryKey subKey = Registry.ClassesRoot.OpenSubKey(protocol + @"\\" + @"shell\\open\\command");  //open myApp protocol's subkey

            //object s = subKey.GetValue();
            //if (subKey.GetValueNames())
            if (key == null)  //if the protocol is not registered yet...we register it
            {
                //key = Registry.ClassesRoot.CreateSubKey("LearActionPlans");
                key = Registry.ClassesRoot.CreateSubKey(protocol);
                //key.SetValue(string.Empty, "URL: LearActionPlans Protocol");
                key.SetValue("URL Protocol", string.Empty);

                key = key.CreateSubKey(@"shell\open\command");
                //key.SetValue(string.Empty, myAppPath1 + " " + "%1");
                key.SetValue(string.Empty, myAppPath + " " + "%1");

                //%1 represents the argument - this tells windows to open this program with an argument / parameter
            }

            key.Close();
        }
    }
}
