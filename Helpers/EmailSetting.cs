using Demo.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace session3Mvc.Helpers
{
	public static class EmailSetting
	{
		public static void SendEmail(Email email)
		{
			var client = new SmtpClient("smtp.gmail.com", 587);
			client.EnableSsl = true;
			client.Credentials = new NetworkCredential("marizgerges655@gmail.com", "scgnojgrqeknsnfr");
			client.Send("marizgerges655@gmail.com", email.To, email.Subject, email.Body);
		}
	}
}
