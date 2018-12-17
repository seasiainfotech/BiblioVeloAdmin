using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace BiblioVelo.Data
{
    public class Common
    {
        public static Boolean Mail(string EmailID, string Password, string Subject)
        {
            try
            {
                string templatePath = string.Empty;
                templatePath = "~/Templates/ForgotPassword.html";
                System.IO.StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath(templatePath));
                string readFile = reader.ReadToEnd();
                string myString = readFile;
                myString = myString.Replace("Email", EmailID);
                myString = myString.Replace("Pwd", Password);
                string body = Convert.ToString(myString);
                string fromEmail = Convert.ToString(ConfigurationManager.AppSettings["From"]);
                string fromName = Convert.ToString(ConfigurationManager.AppSettings["UserName"]);
                bool result = SendMail(fromEmail, fromName, EmailID, body, Subject);
                if (result == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public static bool SendMail(string from, string fromName, string to, string message, string subject)
        {
            try
            {
                MailAddress fromMailAddress = new MailAddress(from, fromName);
                MailAddress toMailAddress = new MailAddress(to, to);
                using (MailMessage msg = new MailMessage(fromMailAddress, toMailAddress))
                {
                    string host = ConfigurationManager.AppSettings["Host"].ToString();
                    int port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
                    msg.Subject = subject;
                    msg.Body = message;
                    msg.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient(host, port);
                    string username = ConfigurationManager.AppSettings["UserName"].ToString();
                    string password = ConfigurationManager.AppSettings["Password"].ToString();
                    if (username != null)
                    {
                        smtp.Credentials = new NetworkCredential(username, password);
                        smtp.UseDefaultCredentials = true;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.EnableSsl = true;
                    }
                    smtp.Send(msg);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// for Encrypt
        /// </summary>
        /// <param name="clearText"></param>
        /// <returns></returns>
        public static string Encrypt(string clearText)
        {
            string EncryptionKey = GetEncrypionKey();
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        /// <summary>
        /// for decrypt
        /// </summary>
        /// <param name="cipherText"></param>
        /// <returns></returns>
        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = GetEncrypionKey();
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        /// <summary>
        /// For Encrypions
        /// </summary>
        /// <returns></returns>
        public static string GetEncrypionKey()
        {
            return Convert.ToString(ConfigurationManager.AppSettings["EncryptionKey"]);
        }

        public static string RandomString(int Size)
        {
            Random rand = new Random();
            string input = "abcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < Size; i++)
            {
                ch = input[rand.Next(0, input.Length)];
                builder.Append(ch);
            }
            return builder.ToString();
        }
        public static void ExcepLog(Exception ex)
        {
            try
            {
                string strPath = HostingEnvironment.MapPath("~/ExceptionLog/Exception.txt");
                if (!File.Exists(strPath))
                {
                    if (strPath != null) File.Create(strPath).Dispose();
                }

                if (strPath != null)
                    using (StreamWriter sw = File.AppendText(strPath))
                    {
                        sw.WriteLine("=====Error Log ==");
                        sw.WriteLine("===Error Occured on===" + DateTime.Now);
                        sw.WriteLine("Error Message : " + ex.Message);
                        sw.WriteLine("Stack Trace : " + ex.StackTrace);
                        sw.WriteLine("");
                        sw.WriteLine("");
                        sw.WriteLine("");
                    }
            }
            catch (Exception exception)
            {

            }
        }
    }
}
