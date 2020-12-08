using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobieStoreWeb.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);

        Task SendEmailsAsync(List<string> emails, string subject, string message);


    }
}
