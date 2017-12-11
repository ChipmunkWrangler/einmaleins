using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Mail;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class SendSaveDataByMail : MonoBehaviour {
	[SerializeField] UnityEngine.UI.Text statusLine = null;
	[SerializeField] UnityEngine.UI.Button button = null;

	const string sender = "einmaleinsreport@crazy-chipmunk.com";
	const string receiver = "testers@crazy-chipmunk.com";
	const string server = "v078528.kasserver.com";
	const int port = 25;
	const string password = MailPassword.password;

	public void SendReport() {
		button.interactable = false;
		Send ( "Dump", XMLSerializationHandler.GetAsString());
		button.interactable = true;
	}

	void Send(string title, string body)
	{
		using (MailMessage mail = new MailMessage()) {
			mail.From = new MailAddress (sender);
			mail.To.Add (receiver);
			mail.Subject = title;
			mail.Body = body;
			mail.Priority = MailPriority.Normal;

			statusLine.text = I2.Loc.LocalizationManager.GetTermTranslation("Connecting to Smtp Server...");

			SmtpClient smtpServer = new SmtpClient (server, port);
			smtpServer.Credentials = new NetworkCredential (sender, password) as ICredentialsByHost;
			smtpServer.EnableSsl = true;
			statusLine.text = I2.Loc.LocalizationManager.GetTermTranslation("Sending message...");
			ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors){
				return true;
			};
			try {
				smtpServer.Send (mail);
				statusLine.text = I2.Loc.LocalizationManager.GetTermTranslation("Message sent. Thanks!");
			} catch(System.Exception ex) {
				string s = I2.Loc.LocalizationManager.GetTermTranslation("Error! Please email the following to") + ": \"" + ex.Message;
				while (ex.InnerException != null) {
					ex = ex.InnerException;
					s += " " + ex.Message;
				}
				s += "\"";
				statusLine.text = s;
				Debug.Log ("Exception " + ex);
			}
		}
	}
}
