using System.Threading;
using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;
using BlogCoreAPI.Models.Mails;
using BlogCoreAPI.Models.Settings;
using Microsoft.Extensions.Options;

namespace BlogCoreAPI.Services.MailService;


/// <summary>
/// Service for sending emails.
/// Code taken and modified from https://code-maze.com/aspnetcore-send-email/ / https://github.com/CodeMazeBlog/email-attachments-aspnet-core
/// </summary>
public class EmailService : IEmailService
{
    private readonly EmailConfigurationSettings _emailConfiguration;

    public EmailService(IOptions<EmailConfigurationSettings> emailConfiguration)
    {
        _emailConfiguration = emailConfiguration.Value;
    }

    public async Task SendEmailAsync(Message message, CancellationToken token)
    {
        var mailMessage = CreateEmailMessage(message);

        await SendAsync(mailMessage, token);
    }

    private MimeMessage CreateEmailMessage(Message message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(_emailConfiguration.NameOfFrom, _emailConfiguration.From));
        emailMessage.To.AddRange(message.To);
        emailMessage.Subject = message.Subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = message.Content
        };

        emailMessage.Body = bodyBuilder.ToMessageBody();
        return emailMessage;
    }

    private async Task SendAsync(MimeMessage mailMessage, CancellationToken token)
    {
        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(_emailConfiguration.SmtpServer, _emailConfiguration.Port, true, token);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            await client.AuthenticateAsync(_emailConfiguration.UserName, _emailConfiguration.Password, token);

            await client.SendAsync(mailMessage, token);
        }
        finally
        {
            await client.DisconnectAsync(true, token);
            client.Dispose();
        }
    }
}