using MailKit.Net.Smtp;
using MimeKit;
using ProiectII.Interfaces;

namespace ProiectII.Services.UtilityServices
{
    public class EmailService : IEmailService
    {
        public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("Fox Shelter", "noreply@foxshelter.local"));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            message.Body = new BodyBuilder
            {
                HtmlBody = body
            }.ToMessageBody();

            using var client = new SmtpClient();

            try
            {
                // In Docker, host-ul 'mailpit' va fi rezolvat la containerul respectiv
                await client.ConnectAsync("mailpit", 1025, MailKit.Security.SecureSocketOptions.None);

                await client.SendAsync(message);

                await client.DisconnectAsync(true);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email error: {ex.Message}");
                // Fallback la logare daca MailPit nu e disponibil
                Console.WriteLine("================================================================");
                Console.WriteLine("OUTGOING EMAIL SYSTEM | Status: FAILED_TO_SEND");
                Console.WriteLine("To: " + toEmail);
                Console.WriteLine("Subject: " + subject);
                Console.WriteLine("Content: " + body);
                Console.WriteLine("================================================================");
                return false;
            }
        }
    }
}