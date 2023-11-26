using System;
using System.Threading.Tasks;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Mailjet.Client.TransactionalEmails;
using NUnit.Framework;

namespace SendGridTest
{
    [TestFixture]
    public class SendMailTest
    {
   
        [Test]
        public static async Task SendGridTest()
        {
            MailjetClient client = new MailjetClient(
            Environment.GetEnvironmentVariable("MJ_APIKEY_PUBLIC"),
            Environment.GetEnvironmentVariable("MJ_APIKEY_PRIVATE"));


            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource
            };

            // construct your email with builder
            var email = new TransactionalEmailBuilder()
                   .WithFrom(new SendContact("moulinette.a.vincent@gmail.com"))
                   .WithSubject("Sending with Mailjet is Fun")
                   .WithHtmlPart("<strong>and easy to do anywhere, even with C#</strong>")
                   .WithTo(new SendContact("vincent.tollu@gmail.com"))
                   .Build();

            // invoke API to send email
            var response = await client.SendTransactionalEmailAsync(email);

            // check response
            NUnit.Framework.Assert.AreEqual(1, response.Messages.Length);
        }
    }
}
