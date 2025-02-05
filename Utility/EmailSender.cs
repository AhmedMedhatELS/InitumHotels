using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class EmailSender : IEmailSender
    {
        Task IEmailSender.SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("ahmedmedhatt2112@gmail.com", "cbmw vjuh fhkg ehdo")
            };

            return client.SendMailAsync(
                new MailMessage(from: "ahmedmedhatt2112@gmail.com",
                                to: email,
                                subject,
                                message
                                )
                { IsBodyHtml = true }
                );
        }
    }
}
