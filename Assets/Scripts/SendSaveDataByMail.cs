using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class SendSaveDataByMail : MonoBehaviour
{
    const string Sender = "einmaleinsreport@crazy-chipmunk.com";
    const string Receiver = "testers@crazy-chipmunk.com";
    const string Server = "v078528.kasserver.com";
    const int Port = 25;

    static readonly string Password = MailPassword.Password;

    [SerializeField] UnityEngine.UI.Text statusLine = null;
    [SerializeField] UnityEngine.UI.Button sendButton = null;

    public void SendReport()
    {
        sendButton.interactable = false;
        Send("Dump", XMLSerializationHandler.GetAsString());
        sendButton.interactable = true;
    }

    void Send(string title, string body)
    {
        using (var mail = new MailMessage())
        {
            mail.From = new MailAddress(Sender);
            mail.To.Add(Receiver);
            mail.Subject = title;
            mail.Body = body;
            mail.Priority = MailPriority.Normal;

            statusLine.text = I2.Loc.LocalizationManager.GetTermTranslation("Connecting to Smtp Server...");

            var smtpServer = new SmtpClient(Server, Port);
            smtpServer.Credentials = new NetworkCredential(Sender, Password) as ICredentialsByHost;
            smtpServer.EnableSsl = true;
            statusLine.text = I2.Loc.LocalizationManager.GetTermTranslation("Sending message...");
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            try
            {
                smtpServer.Send(mail);
                statusLine.text = I2.Loc.LocalizationManager.GetTermTranslation("Message sent. Thanks!");
            }
            catch (System.Exception ex)
            {
                string s = I2.Loc.LocalizationManager.GetTermTranslation("Error! Please email the following to") + ": \"";
                statusLine.text = s + AssemblyCSharp.ExceptionPrettyPrint.Msg(ex) + "\"";
            }
        }
    }
}
