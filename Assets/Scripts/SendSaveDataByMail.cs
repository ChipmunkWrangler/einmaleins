using System;
using System.Net;
using System.Net.Mail;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

internal class SendSaveDataByMail : MonoBehaviour
{
    private const string Sender = "einmaleinsreport@crazy-chipmunk.com";
    private const string Receiver = "testers@crazy-chipmunk.com";
    private const string Server = "v078528.kasserver.com";
    private const int Port = 25;

    private static readonly string Password = MailPassword.Password;
    [SerializeField] private Selectable sendButton;

    [SerializeField] private Text statusLine;

    public void SendReport()
    {
        sendButton.interactable = false;
        Send("Dump", XMLSerializationHandler.GetAsString());
        sendButton.interactable = true;
    }

    private void Send(string title, string body)
    {
        using (var mail = new MailMessage())
        {
            mail.From = new MailAddress(Sender);
            mail.To.Add(Receiver);
            mail.Subject = title;
            mail.Body = body;
            mail.Priority = MailPriority.Normal;

            statusLine.text = LocalizationManager.GetTermTranslation("Connecting to Smtp Server...");

            var smtpServer = new SmtpClient(Server, Port);
            smtpServer.Credentials = new NetworkCredential(Sender, Password);
            smtpServer.EnableSsl = true;
            statusLine.text = LocalizationManager.GetTermTranslation("Sending message...");
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            try
            {
                smtpServer.Send(mail);
                statusLine.text = LocalizationManager.GetTermTranslation("Message sent. Thanks!");
            }
            catch (Exception ex)
            {
                var s = LocalizationManager.GetTermTranslation("Error! Please email the following to") + ": \"";
                statusLine.text = s + ExceptionPrettyPrint.Msg(ex) + "\"";
            }
        }
    }
}