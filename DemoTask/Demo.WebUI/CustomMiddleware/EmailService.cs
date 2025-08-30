using MailKit.Net.Smtp;
using MimeKit;

namespace Demo.WebUI.CustomMiddleware
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string messagebody)
        {
            try
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress(_configuration["SmtpSettings:SenderName"], _configuration["SmtpSettings:SenderEmail"]));
                emailMessage.To.Add(new MailboxAddress("", toEmail));
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart("plain") { Text = messagebody };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_configuration["SmtpSettings:Server"], int.Parse(_configuration["SmtpSettings:Port"]), MailKit.Security.SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_configuration["SmtpSettings:Username"], _configuration["SmtpSettings:Password"]);
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }

            }
            catch (Exception)
            {

                throw;
            }

            //try
            //{
            //    var fromAddress = new MailAddress("demotask372@gmail.com", "Software Gaze");
            //    var toAddress = new MailAddress(toEmail);

            //    const string fromPassword = "lkgs lawe xoak cswg";// App password: name Demo Project  password: lkgs lawe xoak cswg

            //    using (var smtp = new SmtpClient("smtp.gmail.com", 587))
            //    {
            //        smtp.EnableSsl = true;
            //        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            //        smtp.UseDefaultCredentials = false;
            //        smtp.Credentials = new NetworkCredential(fromAddress.Address, fromPassword);

            //        using (var message = new MailMessage(fromAddress, toAddress)
            //        {
            //            Subject = subject,
            //            Body = messagebody,
            //            IsBodyHtml = true
            //        })
            //        {
            //            await smtp.SendMailAsync(message);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //}
        }
    }
}
