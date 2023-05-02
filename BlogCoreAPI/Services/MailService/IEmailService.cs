using System.Threading;
using System.Threading.Tasks;
using BlogCoreAPI.Models.Mails;

namespace BlogCoreAPI.Services.MailService;

public interface IEmailService
{
    Task SendEmailAsync(Message message, CancellationToken token);
}