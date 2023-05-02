using System.Threading;
using System.Threading.Tasks;
using BlogCoreAPI.Models.Mails;
using BlogCoreAPI.Services.MailService;

namespace BlogCoreAPI.FunctionalTests.Models;

public class EmailServiceMock : IEmailService
{
    public Task SendEmailAsync(Message message, CancellationToken token)
    {
        return Task.CompletedTask;
    }
}