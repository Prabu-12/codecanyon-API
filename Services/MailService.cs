using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using QuizplusApi.Models;
using QuizplusApi.ViewModels.Email;

namespace QuizplusApi.Services
{
    public class MailService : IMailService
    {
        private readonly AppDbContext _context;
        public MailService(AppDbContext context)
        {
            _context=context;
        }
        public async Task SendPasswordEmailAsync(ForgetPassword request)
        {
            var emailSettings=_context.SiteSettings.Where(s=>s.SiteSettingsId==1).
            Select(p=>new MailSettings{Mail=p.DefaultEmail,DisplayName=p.DisplayName,Password=p.Password,Host=p.Host,Port=p.Port}).SingleOrDefault();

            string filePath = Directory.GetCurrentDirectory() + "\\Resources\\EmailTemplate\\forgetPassword.html";
            StreamReader str = new StreamReader(filePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText.Replace("[logoPath]",request.LogoPath).Replace("[siteUrl]", request.SiteUrl).Replace("[body]", request.Body);
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(emailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(request.ToEmail));
            email.Subject=request.Subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = MailText;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(emailSettings.Mail, emailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
        public async Task SendWelcomeEmailAsync(WelcomeRequest request)
        {
            var emailSettings=_context.SiteSettings.Where(s=>s.SiteSettingsId==1).
            Select(p=>new MailSettings{Mail=p.DefaultEmail,DisplayName=p.DisplayName,Password=p.Password,Host=p.Host,Port=p.Port}).SingleOrDefault();

            string filePath = Directory.GetCurrentDirectory() + "\\Resources\\EmailTemplate\\welcome.html";
            StreamReader str = new StreamReader(filePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText.Replace("[name]", request.Name).Replace("[email]", request.ToEmail).Replace("[logoPath]",request.LogoPath).Replace("[siteUrl]", request.SiteUrl).Replace("[body]", request.Body);
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(emailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(request.ToEmail));
            email.Subject = $"Welcome {request.Name}";
            var builder = new BodyBuilder();
            builder.HtmlBody = MailText;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(emailSettings.Mail, emailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        public async Task SendInvitationEmailAsync(List<Invitation> listOfAddress)
        {
            var emailSettings=_context.SiteSettings.Where(s=>s.SiteSettingsId==1).
            Select(p=>new MailSettings{Mail=p.DefaultEmail,DisplayName=p.DisplayName,Password=p.Password,Host=p.Host,Port=p.Port}).SingleOrDefault();

            string filePath = Directory.GetCurrentDirectory() + "\\Resources\\EmailTemplate\\invitation.html";
            StreamReader str = new StreamReader(filePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText.Replace("[email]", listOfAddress[0].Email).Replace("[logoPath]",listOfAddress[0].LogoPath).Replace("[siteUrl]", listOfAddress[0].SiteUrl).Replace("[body]", listOfAddress[0].Body);
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(emailSettings.Mail);
            InternetAddressList list=new InternetAddressList();
            foreach(var item in listOfAddress)
            {
                list.Add(MailboxAddress.Parse(item.Email));
            }
            email.To.AddRange(list);
            email.Subject="Greettings";
            var builder = new BodyBuilder();
            builder.HtmlBody = MailText;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(emailSettings.Mail, emailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        public async Task SendReportEmailAsync(ReExamRequest report)
        {
            var emailSettings=_context.SiteSettings.Where(s=>s.SiteSettingsId==1).
            Select(p=>new MailSettings{Mail=p.DefaultEmail,DisplayName=p.DisplayName,Password=p.Password,Host=p.Host,Port=p.Port}).SingleOrDefault();

            string filePath = Directory.GetCurrentDirectory() + "\\Resources\\EmailTemplate\\reportStudents.html";
            StreamReader str = new StreamReader(filePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText.Replace("[logoPath]",report.LogoPath).Replace("[siteUrl]", report.SiteUrl).Replace("[body]", report.Body);
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(emailSettings.Mail);
            InternetAddressList list=new InternetAddressList();
            foreach(var item in report.emails)
            {
                list.Add(MailboxAddress.Parse(item.Email));
            }
            email.To.AddRange(list);
            email.Subject=report.Subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = MailText;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(emailSettings.Mail, emailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}