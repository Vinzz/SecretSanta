using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace SendGridTest
{
    [TestFixture]
    public class SendMailTest
    {
   
        [Test]
        public static async Task SendGridTest()
        {
            var apiKey = Environment.GetEnvironmentVariable("SendGridKey");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("moulinette.a.vincent@gmail.com");
            var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress("vincent.tollu@gmail.com");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

        }
    }
}
