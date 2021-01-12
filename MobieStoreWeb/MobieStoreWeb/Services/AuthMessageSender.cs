using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MobieStoreWeb.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MobieStoreWeb.Services
{
    public class AuthMessageSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<AuthMessageSender> _logger;

        public AuthMessageSender(IOptions<EmailSettings> emailSettings, ILogger<AuthMessageSender> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            string toEmail = string.IsNullOrWhiteSpace(email) ? _emailSettings.ToEmail : email;
            Execute(new List<string> { toEmail }, subject, message).Wait();
            return Task.FromResult(0);
        }

        public Task SendEmailsAsync(List<string> emails, string subject, string message)
        {
            emails = emails.Where(email => !string.IsNullOrWhiteSpace(email)).ToList();
            if (emails.Count == 0)
            {
                emails = new List<string> { _emailSettings.ToEmail };
            }
            Execute(emails, subject, message).Wait();
            return Task.FromResult(0);
        }

        private async Task Execute(List<string> emails, string subject, string message)
        {
            try
            {
                var mail = new MailMessage
                {
                    From = new MailAddress(_emailSettings.FromEmail, "H&C Shopper")
                };
                emails.ForEach(email => mail.To.Add(new MailAddress(email)));
                mail.Subject = subject;
                mail.Body = message;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                using (var smtp = new SmtpClient(_emailSettings.Domain, _emailSettings.Port))
                {
                    smtp.Credentials = new NetworkCredential(_emailSettings.UsernameEmail, _emailSettings.UsernamePassword);
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(mail);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
            }
        }
    }
}
