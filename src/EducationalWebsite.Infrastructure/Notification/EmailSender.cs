using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationalWebsite.Domain.Interfaces.Users;
using MimeKit;

namespace EducationalWebsite.Infrastructure.Notification
{
    public class EmailSender : IEmailSender
    {
        private readonly GmailServiceInitializer _gmailServiceInitializer;

        public EmailSender(GmailServiceInitializer gmailServiceInitializer)
        {
            _gmailServiceInitializer = gmailServiceInitializer ?? throw new ArgumentNullException(nameof(gmailServiceInitializer));
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var gmailService = await _gmailServiceInitializer.InitializeAsync().ConfigureAwait(false);

            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress("Educational Website", "aaa@gg.com"));
            mimeMessage.To.Add(new MailboxAddress(email, email));
            mimeMessage.Subject = subject;
            mimeMessage.Body = new TextPart("html") { Text = message };
            
            // MIME to 64 bit
            var stream = new MemoryStream();
            await mimeMessage.WriteToAsync(stream).ConfigureAwait(false);
            var rawMessage = Convert.ToBase64String(stream.ToArray())
                .Replace('+', '-')
                .Replace('/', '_')
                .Replace("=", "");

            var messageToSend = new Google.Apis.Gmail.v1.Data.Message { Raw = rawMessage };
            await gmailService.Users.Messages.Send(messageToSend, "me").ExecuteAsync().ConfigureAwait(false);
                
        }
    }
}