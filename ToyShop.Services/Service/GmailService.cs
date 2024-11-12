using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ToyShop.ModelViews.GmailModel;

namespace ToyShop.Services.Service
{
	public class GmailService
	{
		private IConfiguration _configuration;

		public GmailService()
		{
			if (_configuration == null) InitializeConfiguration();
		}

		private void InitializeConfiguration()
		{
			var path = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
			_configuration = new ConfigurationBuilder()
				.SetBasePath(path + "\\EduToyRent.API")
				.AddJsonFile("appsettings.json")
				.Build();
		}

		public  bool SendEmailSingle(EmailRequestModel request)
		{
			try
			{
				var message = new MailMessage();
				message.From = new MailAddress(_configuration["Smtp:Email"]);
				message.To.Add(new MailAddress(request.ReceiverEmail));
				message.Subject = request.EmailSubject;
				message.Body = request.EmailBody;
				message.IsBodyHtml = request.IsHtml;

				var smtpClient = new SmtpClient("smtp.gmail.com")
				{
					Port = int.Parse(_configuration["Smtp:Port"]),
					Credentials = new NetworkCredential(_configuration["Smtp:Email"], _configuration["Smtp:Password"]),
					EnableSsl = true,
				};

				smtpClient.Send(message);
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
		}
	}
}
