namespace login_system.Services;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string htmlContent);
    string LoadTemplateAndReplace(string filePath, Dictionary<string, string> replacements);
}