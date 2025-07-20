using System.Net;
using System.Net.Mail;

namespace login_system.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string htmlContent)
    {
        var smtpClient = new SmtpClient(_configuration["Email:Smtp"])
        {
            Port = int.Parse(_configuration["Email:Port"]!),
            Credentials = new NetworkCredential(
                _configuration["Email:Username"],
                _configuration["Email:Password"]),
            EnableSsl = true,  
        };

        var message = new MailMessage
        {
            From = new MailAddress(_configuration["Email:From"]!),
            Subject = subject,
            Body = htmlContent,
            IsBodyHtml = true,
        };
        message.To.Add(toEmail);

        await smtpClient.SendMailAsync(message);
    }

    public string LoadTemplateAndReplace(string filePath, Dictionary<string, string> replacements)
    {
        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), filePath);
        var html = File.ReadAllText(fullPath);

        foreach (var item in replacements)
        {
            html = html.Replace(item.Key, item.Value);
        }

        return html;
    }
}