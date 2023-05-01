using System.Collections.Generic;
using System.Linq;
using MimeKit;

namespace BlogCoreAPI.Models.Mails;

public class Message
{
    public List<MailboxAddress> To { get; }
    public string Subject { get; }
    public string Content { get; }

    public Message(IEnumerable<EmailIdentity> to, string subject, string content)
    {
        To = new List<MailboxAddress>();
        To.AddRange(to.Select(x => new MailboxAddress(x.Name, x.Email)));
        Subject = subject;
        Content = content;
    }
}