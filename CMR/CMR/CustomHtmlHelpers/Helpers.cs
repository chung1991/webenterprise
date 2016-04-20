using CMR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CMR.CustomHtmlHelpers
{
    public static class Helpers
    {
        public static async Task<string> sendMail(String sendTo, String content)
        {

            var message = new MailMessage();
            message.To.Add(new MailAddress(sendTo));
            message.Subject = "Course Monitoring Report";
            message.Body = content;
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                await smtp.SendMailAsync(message);
            }
            return "Send Ok";
        }

    }
}