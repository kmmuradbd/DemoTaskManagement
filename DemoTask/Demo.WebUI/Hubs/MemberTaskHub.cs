using Demo.WebUI.DBQuery;
using Demo.WebUI.Helpers;
using Demo.WebUI.Models;
using DemoTask.Domain.DomainObject;
using Microsoft.AspNetCore.SignalR;
using System.Net;
using System.Net.Mail;

namespace Demo.WebUI.Hubs
{
    public class MemberTaskHub : Hub
    {
        MemberTaskDBQuery AppMemberTask;
        UserDBQuery AppUser;

        public MemberTaskHub(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("TMConnection");
            AppMemberTask = new MemberTaskDBQuery(connectionString);
            AppUser = new UserDBQuery(connectionString);
        }

        public async Task SendMemberTasks()
        {
            var httpContext = Context.GetHttpContext();
            string userName = SessionHelper.GetObjectFromJson<string>(httpContext.Session, "userName");
            string roleId = SessionHelper.GetObjectFromJson<string>(httpContext.Session, "roleId");

            var memberTasks = AppMemberTask.GetMemberTasks(userName, roleId);
            await Clients.All.SendAsync("ReceivedMemberTasks", memberTasks);
        }


        public async Task SendUser()
        {
            var httpContext = Context.GetHttpContext();
            string userName = SessionHelper.GetObjectFromJson<string>(httpContext.Session, "userName");
            var userData = AppUser.GetUser(userName);
            await Clients.All.SendAsync("ReceivedUser", userData);
        }

        public async Task SendMemberTaskLastUpdate()
        {
            var httpContext = Context.GetHttpContext();
            string userName = SessionHelper.GetObjectFromJson<string>(httpContext.Session, "userName");
            DateTime lastCreatedDate = NotificationCache.GetLastCreatedDate(userName) ?? DateTime.Now;

            var memberLastTasks = AppMemberTask.GetMemberTaskLastUpdates(userName, lastCreatedDate);

                NotificationCache.SetLastCreatedDate(userName, lastCreatedDate);

            // Send updates to clients
            await Clients.All.SendAsync("ReceivedMemberTaskLastUpdate", memberLastTasks);
        }


        public async Task SendEmailNotification(string toEmail, string subject, string messageBody)
        {
            try
            {
                var fromAddress = new MailAddress("test@gmail.com", "Software Gaze");
                var toAddress = new MailAddress(toEmail);

                // ⚠️ Use an App Password here, not your Gmail login password
                const string fromPassword = "*******";

                using (var smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(fromAddress.Address, fromPassword);

                    using (var message = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = subject,
                        Body = messageBody,
                        IsBodyHtml = true
                    })
                    {
                        await smtp.SendMailAsync(message);
                    }
                }
                string successMessage = $"Email sent to {toEmail} successfully.";
                await Clients.Caller.SendAsync("EmailStatus", successMessage);
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("EmailStatus", $"Error sending email: {ex.Message}");
            }
        }



    }
}
