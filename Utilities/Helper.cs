using Microsoft.Win32;
using System;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;
using System.Diagnostics;

using LearActionPlans.Models;
using LearActionPlans.DataMappers;

namespace LearActionPlans.Utilities
{
    public class Helper
    {
        //public static DataTable dtAP;

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
            var key = Registry.ClassesRoot.OpenSubKey(protocol);  //open myApp protocol's subkey
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

        public static void VytvoritEmail(string zadavatel1Email,
            string zadavatel2Email,
            BodAP bodAP,
            string cisloAPStr,
            DateTime minDatumUkonceni,
            DateTime datumUkonceni,
            string odpovedny1,
            string odpovedny2,
            string poznamka,
            int apId,
            int bodAPId,
            int idZadost)
        {
            //using (var message = new MailMessage())
            //{
            //    //MailMessage message = new MailMessage();
            //    //var smtp = new SmtpClient
            //    //{
            //    //    UseDefaultCredentials = false,
            //    //    Credentials = new NetworkCredential("bartos.grammer@seznam.cz", "stepan12"),
            //    //    //Credentials = new NetworkCredential("LSCEurope@grammer.com", "Grammer123"),
            //    //    DeliveryMethod = SmtpDeliveryMethod.Network,
            //    //    Port = 25,
            //    //    Host = "smtp.seznam.cz",
            //    //    //Host = "smtp.grammer.com",
            //    //    EnableSsl = false
            //    //};
            //    ////zajistí zobrazení češtiny v emailu
            //    //message.BodyEncoding = System.Text.Encoding.UTF8;
            //    //message.From = new MailAddress("bartos.grammer@seznam.cz");
            //    //message.Subject = string.Format(@"Request for a new Deadline");
            //    //message.IsBodyHtml = true;
            //}
            //----- odeslání požadavku -----------------------------------------------------------------------------------------------------


            var htmlTableStart = "<table style=\"border-collapse:collapse; text-align:left; font-family:Arial, Helvetica, Sans-serif;\" >";
            var htmlTableEnd = "</table>";

            //string htmlHeaderRowStart = "<tr style=\"background-color:#6FA1D2; color:#ffffff;\">";
            //string htmlHeaderRowEnd = "</tr>";

            var htmlTrStart = "<tr style=\"color:#555555;\">";
            var htmlTrEnd = "</tr>";

            var htmlTdStartFirstColumn = "<td style=\" border-color:#5c87b2; border-style:solid; border-width:thin; padding:5px; width:300px\">";
            var htmlTdEndFirstColumn = "</td>";

            var htmlTdStartSecondColumn = "<td style=\" border-color:#5c87b2; border-style:solid; border-width:thin; padding:5px; width:450px\">";
            var htmlTdEndSecondColumn = "</td>";

            var htmlTdStart = "<td style='border-color:#5c87b2; border-style:solid; border-width:thin; padding:5px;'>";
            var htmlTdEnd = "</td>";

            var htmlTdStartPozadi = "<td style=\" border-color:#5c87b2; border-style:solid; border-width:thin; padding:5px; background-color:#e1e1ff;\">";
            var htmlTdEndPozadi = "</td>";

            string zprava = htmlTableStart;
            zprava += htmlTrStart;
            zprava += htmlTdStartFirstColumn + @"<b>AP</b>" + htmlTdEndFirstColumn;
            zprava += htmlTdStartSecondColumn + cisloAPStr + htmlTdEndSecondColumn;
            zprava += htmlTrEnd;
            zprava += htmlTrStart;
            zprava += htmlTdStart + @"<b>Point AP</b>" + htmlTdEnd;
            zprava += htmlTdStart + bodAP.CisloBoduAP.ToString() + htmlTdEnd;
            zprava += htmlTrEnd;
            zprava += htmlTrStart;
            zprava += htmlTdStart + @"<b> &nbsp; &nbsp; Standard chapter</b>" + htmlTdEnd;
            zprava += htmlTdStart + bodAP.OdkazNaNormu + htmlTdEnd;
            zprava += htmlTrEnd;
            zprava += htmlTrStart;
            zprava += htmlTdStart + @"<b> &nbsp; &nbsp; Evaluation</b>" + htmlTdEnd;
            zprava += htmlTdStart + bodAP.HodnoceniNeshody + htmlTdEnd;
            zprava += htmlTrEnd;
            zprava += htmlTrStart;
            zprava += htmlTdStart + @"<b> &nbsp; &nbsp; Description of the problem</b>" + htmlTdEnd;
            zprava += htmlTdStart + bodAP.PopisProblemu + htmlTdEnd;
            zprava += htmlTrEnd;

            zprava += htmlTrStart;
            zprava += htmlTdStart + @"<b>Why made</b>" + htmlTdEnd;
            zprava += htmlTdStart + "" + htmlTdEnd;
            zprava += htmlTrEnd;

            zprava += htmlTrStart;
            zprava += htmlTdStart + @"<b> &nbsp; &nbsp; Root cause</b>" + htmlTdEnd;
            zprava += htmlTdStart + bodAP.SkutecnaPricinaWM + htmlTdEnd;
            zprava += htmlTrEnd;

            zprava += htmlTrStart;
            zprava += htmlTdStart + @"<b> &nbsp; &nbsp; Corrective action" + htmlTdEnd;
            zprava += htmlTdStart + bodAP.NapravnaOpatreniWM + htmlTdEnd;
            zprava += htmlTrEnd;

            zprava += htmlTrStart;
            zprava += htmlTdStart + @"<b>Why shipped</b>" + htmlTdEnd;
            zprava += htmlTdStart + "" + htmlTdEnd;
            zprava += htmlTrEnd;

            zprava += htmlTrStart;
            zprava += htmlTdStart + @"<b> &nbsp; &nbsp; Root cause</b>" + htmlTdEnd;
            zprava += htmlTdStart + bodAP.SkutecnaPricinaWS + htmlTdEnd;
            zprava += htmlTrEnd;

            zprava += htmlTrStart;
            zprava += htmlTdStart + @"<b> &nbsp; &nbsp; Corrective action</b>" + htmlTdEnd;
            zprava += htmlTdStart + bodAP.NapravnaOpatreniWS + htmlTdEnd;
            zprava += htmlTrEnd;

            zprava += htmlTrStart;
            zprava += htmlTdStartPozadi + @"<b>Last Deadline</b>" + htmlTdEndPozadi;
            var posledniMoznyTermin = minDatumUkonceni;
            posledniMoznyTermin = posledniMoznyTermin.AddDays(-1);
            zprava += htmlTdStartPozadi + @"<b>" + posledniMoznyTermin.ToShortDateString() + @"</b>" + htmlTdEndPozadi;
            zprava += htmlTrEnd;
            zprava += htmlTrStart;
            zprava += htmlTdStartPozadi + @"<b>New Deadline</b>" + htmlTdEndPozadi;
            zprava += htmlTdStartPozadi + @"<b>" + datumUkonceni.ToShortDateString() + @"</b>" + htmlTdEndPozadi;
            zprava += htmlTrEnd;
            zprava += htmlTrStart;
            zprava += htmlTdStartPozadi + @"<b>Note</b>" + htmlTdEndPozadi;
            //action_["textBoxPoznamka"]
            zprava += htmlTdStartPozadi + poznamka + htmlTdEndPozadi;
            zprava += htmlTrEnd;

            zprava += htmlTrStart;
            zprava += htmlTdStart + @"<b>Responsible #1</b>" + htmlTdEnd;
            zprava += htmlTdStart + odpovedny1 + htmlTdEnd;
            zprava += htmlTrEnd;
            zprava += htmlTrStart;
            zprava += htmlTdStart + @"<b>Responsible #2</b>" + htmlTdEnd;
            zprava += htmlTdStart + odpovedny2 + htmlTdEnd;
            zprava += htmlTrEnd;
            zprava += htmlTableEnd;
            //zprava += "<a href=javascript:runProgram()>Odkaz</a>";
            //Process the request - vyřídit žádost

            //zprava += string.Format(@"<p style='font-family:Arial, Helvetica, Sans-serif; font-size:150 %; '><a href='LearAPConfirmation:?{0}&{1}&{2}&{3}&{4}&{5}' type='application/octet-stream'>Process the request</a></p>", cisloAPStr_, apId, bodAPId, Convert.ToInt32(action_["akceId"]), idZadost, FormMain.VlastnikIdAkce);
            //zprava += string.Format(@"<p style='font-family:Arial, Helvetica, Sans-serif; font-size:150 %; '><a href='LearAPConfirmation:?{0}&{1}&{2}&{3}&{4}&{5}' type='application/octet-stream'>Process the request</a></p>", cisloAPStr_, apId, bodAPId, Convert.ToInt32(action_["akceId"]), idZadost, Convert.ToInt32(action_["comboBoxOdpovednaOsoba1Id"]));
            //odebrán čtvrtý parametr
            zprava += string.Format(@"<p style='font-family:Arial, Helvetica, Sans-serif; font-size:150 %; '><a href='LearAPConfirmation:?{0}&{1}&{2}&{3}&{4}' type='application/octet-stream'>Process the request</a></p>", cisloAPStr, apId, bodAPId, idZadost, bodAP.OdpovednaOsoba1Id);

            //message.To.Add(new MailAddress("bartos.grammer@seznam.cz"));

            // tady se uloží email do tabulky EmailOdeslat a zavolá se aplikace pro odeslání emailu LaerEmailOdeslat
            //OdeslatEmail(smtp, message);
            // uložit do tabulky EmailOdelat
            var predmet = @"Request for a new Deadline";

            OdeslatEmailDataMapper.UlozitEmailNovyBodAP(zadavatel1Email, predmet, zprava);

            // spustit externí program pro odeslání emailů
            // Prepare the process to run
            using (var proc = Process.Start("C:\\Users\\pc\\source\\repos\\LearSendEmail\\bin\\Release\\netcoreapp3.1\\LearSendEmail.exe"))
            {
                proc.WaitForExit();

                // Retrieve the app's exit code
                var exitCode = proc.ExitCode;
            }
            //var start = new ProcessStartInfo
            //{
            //    // Enter in the command line arguments, everything you would enter after the executable name itself
            //    //Arguments = arguments,
            //    // Enter the executable to run, including the complete path
            //    FileName = "C:\\Users\\pc\\source\\repos\\LearSendEmail\\bin\\Release\\netcoreapp3.1\\LearSendEmail.exe",
            //    // Do you want to show a console window?
            //    WindowStyle = ProcessWindowStyle.Hidden,
            //    CreateNoWindow = true
            //};
            //int exitCode;

            //ZapsatEmail(zadavatel1Email, predmet, message.Body);
        }

        //private static void ZapsatEmail(string emailKomu, string predemet, string zprava)
        //{
        //    OdeslatEmailDataMapper.UlozitEmailNovyBodAP(emailKomu, predemet, zprava);
        //}
    }
}
