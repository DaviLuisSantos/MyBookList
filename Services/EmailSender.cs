using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web; // Importante para Uri.EscapeDataString (caso precise)

namespace MyBookList.Services;

public interface IEmailSender
{
    Task SendAccountActivationEmailAsync(string toEmail, string activationToken);
    // Adicione o método já existente aqui
    Task SendEmailAsync(string toEmail, string subject, string message);
}

public class EmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailSender> _logger;

    public EmailSender(IConfiguration configuration, ILogger<EmailSender> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendAccountActivationEmailAsync(string toEmail, string activationToken)
    {
        // Obtém as configurações do SMTP (como no método SendEmailAsync)
        var smtpServer = _configuration["SmtpSettings:Server"];
        var smtpPort = _configuration.GetValue<int>("SmtpSettings:Port");
        var smtpUsername = _configuration["SmtpSettings:Username"];
        var smtpPassword = _configuration["SmtpSettings:Password"];
        var fromEmail = _configuration["SmtpSettings:FromEmail"];
        var fromName = _configuration["SmtpSettings:FromName"] ?? "Sistema";

        if (string.IsNullOrEmpty(smtpServer) || smtpPort == 0 || string.IsNullOrEmpty(smtpUsername) || string.IsNullOrEmpty(smtpPassword) || string.IsNullOrEmpty(fromEmail))
        {
            _logger.LogError("Configurações SMTP incompletas. Verifique o appsettings.json.");
            throw new InvalidOperationException("Configurações SMTP incompletas. Verifique o appsettings.json.");
        }

        string activationLink = $"https://mybooklist-frontend.vercel.app//register/activate?email={toEmail}&token={activationToken}";

        // Cria o corpo do e-mail
        string subject = "Ativação da sua conta";
        string message = $@"
                <h1>Bem-vindo(a)!</h1>
                <p>Obrigado(a) por se registrar.</p>
                <p>Para ativar sua conta, clique no link abaixo:</p>
                <a href='{activationLink}'>Ativar minha conta</a>
                <p>Se o botão não funcionar, copie e cole o seguinte link no seu navegador:</p>
                <p>{activationLink}</p>
            ";

        try
        {
            MailMessage mail = new MailMessage
            {
                From = new MailAddress(fromEmail, fromName),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };
            mail.To.Add(toEmail);

            SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(smtpUsername, smtpPassword)
            };

            await smtpClient.SendMailAsync(mail);

            _logger.LogInformation($"E-mail de ativação enviado com sucesso para {toEmail}.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao enviar e-mail de ativação para {toEmail}.");
            throw;
        }
    }
    //implementação já existente.
    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        // Obtém as configurações do SMTP das configurações
        var smtpServer = _configuration["SmtpSettings:Server"];
        var smtpPort = _configuration.GetValue<int>("SmtpSettings:Port");
        var smtpUsername = _configuration["SmtpSettings:Username"];
        var smtpPassword = _configuration["SmtpSettings:Password"];
        var fromEmail = _configuration["SmtpSettings:FromEmail"];
        var fromName = _configuration["SmtpSettings:FromName"] ?? "Sistema";

        // Validação das configurações
        if (string.IsNullOrEmpty(smtpServer) || smtpPort == 0 || string.IsNullOrEmpty(smtpUsername) || string.IsNullOrEmpty(smtpPassword) || string.IsNullOrEmpty(fromEmail))
        {
            _logger.LogError("Configurações SMTP incompletas. Verifique o appsettings.json.");
            throw new InvalidOperationException("Configurações SMTP incompletas. Verifique o appsettings.json.");
        }

        try
        {
            // Cria a mensagem de e-mail
            MailMessage mail = new MailMessage
            {
                From = new MailAddress(fromEmail, fromName),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };
            mail.To.Add(toEmail);

            // Configura o cliente SMTP
            SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort)
            {
                EnableSsl = true, // Requer SSL
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(smtpUsername, smtpPassword)
            };

            // Envia o e-mail
            await smtpClient.SendMailAsync(mail);

            _logger.LogInformation($"E-mail enviado com sucesso para {toEmail}.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao enviar e-mail para {toEmail}.");
            throw; // Relança a exceção para que o chamador possa tratá-la
        }

    }
}