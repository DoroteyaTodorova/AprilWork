using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HitachiSPACEProgramme.HitachiSPACEProgramme.Languages;

namespace HitachiSPACEProgramme.HitachiSPACEProgramme
{
    internal class EmailSender
    {
        public static void SendEmail(Spaceport spaceport, string analysisReportPath)
        {
            string smtpServer = GetSMTPServer();
            if (smtpServer == null)
            {
                Console.WriteLine(LanguageHandler.GetString("SMTPSelection"));
                return;
            }

            string senderEmail = GetValidEmailAddress(LanguageHandler.GetString("EnterSenderEmail"));
            if (senderEmail == null)
            {
                Console.WriteLine(LanguageHandler.GetString("SenderEmail"));
                return;
            }

            string password = GetValidPassword(LanguageHandler.GetString("EnterSenderPassword"));
            if (password == null)
            {
                Console.WriteLine(LanguageHandler.GetString("Password"));
                return;
            }

            string receiverEmail = GetValidEmailAddress(LanguageHandler.GetString("EnterReceiverEmail"));
            if (receiverEmail == null)
            {
                Console.WriteLine(LanguageHandler.GetString("RecieverEmail"));
                return;
            }



            // Create a new MailMessage
            MailMessage message = new MailMessage(senderEmail, receiverEmail);
            message.Subject = "Launch Analysis Report";
            message.Body = $"Weather forecast for spaceports attached. The best launch Date is {spaceport.Day} in {spaceport.FileName}";

            Attachment attachment = new Attachment(analysisReportPath);
            message.Attachments.Add(attachment);

            SmtpClient smtpClient = new SmtpClient(smtpServer, GetSMTPPort(smtpServer));
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(senderEmail, password);

            try
            {
                // Send the email
                smtpClient.Send(message);
                Console.WriteLine(LanguageHandler.GetString("EmailSent"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(LanguageHandler.GetString("EmailFailed"), ex.Message);
            }
            finally
            {
                // Dispose of resources
                message.Dispose();
                smtpClient.Dispose();
            }
        }


        static string GetValidEmailAddress(string prompt)
        {
            string email;
            do
            {
                Console.WriteLine(prompt);
                email = Console.ReadLine();

                if (!IsValidEmail(email))
                {
                    Console.WriteLine(LanguageHandler.GetString("InvalidEmail"));
                }

            } while (!IsValidEmail(email));

            return email;
        }

        static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            if (!email.Contains("@"))
            {
                return false;
            }

            string[] parts = email.Split('@');
            string localPart = parts[0];
            string domainPart = parts[1];

            if (localPart.Length < 2)
            {
                return false;
            }

            if (domainPart.Length < 2)
            {
                return false;
            }

            if (!domainPart.Contains("."))
            {
                return false;
            }

            string[] domainParts = domainPart.Split('.');
            string lastDomainPart = domainParts[domainParts.Length - 1];
            if (lastDomainPart.Length < 2)
            {
                return false;
            }

            return true;
        }

        static string GetValidPassword(string prompt)
        {
            string password;
            do
            {
                Console.WriteLine(prompt);
                password = Console.ReadLine();

                if (!IsValidPassword(password))
                {
                    Console.WriteLine(LanguageHandler.GetString("InvalidPassword"));
                }

            } while (!IsValidPassword(password));

            return password;
        }

        static bool IsValidPassword(string password)
        {
            if (password.Length < 3)
            {
                return false;
            }

            return true;
        }


        static string GetSMTPServer()
        {
            // Options for some popular email service providers
            Console.WriteLine($"{LanguageHandler.GetString("ChooseEmailProvider")}  [1/2/3/4]");
            Console.WriteLine("1. Gmail");
            Console.WriteLine("2. Outlook");
            Console.WriteLine("3. Office365");
            Console.WriteLine("4. Zimbra");

            int choice;
            do
            {
                Console.Write(LanguageHandler.GetString("EnterChoice"));
            } while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 4);

            switch (choice)
            {
                case 1:
                    return "smtp.gmail.com";
                case 2:
                    return "smtp-mail.outlook.com";
                case 3:
                    return "smtp.office365.com";
                case 4:
                    return "zimbra.xmission.com";
                default:
                    return null;
            }
        }

        static int GetSMTPPort(string smtpServer)
        {
            switch (smtpServer)
            {
                case "smtp.gmail.com":
                    return 587;
                case "smtp-mail.outlook.com":
                    return 587;
                case "smtp.office365.com":
                    return 587;
                case "zimbra.xmission.com":
                    return 587;
                default:
                    return 587;
            }
        }
    }
}
