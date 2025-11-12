using Application.Contracts.Services;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Persistence.Implementation.Services
{
    internal class EmailService(IConfiguration _configuration, ILogger<EmailService> _logger) : IEmailService
    {
        public async Task<bool> SendVerificationEmail(string email, string password)
        {
            try
            {
                var client = new MailjetClient(_configuration.GetValue<string>("MailJet:ApiKey"),
            _configuration.GetValue<string>("MailJet:ApiSecret"));

                MailjetRequest request = new MailjetRequest
                {
                    Resource = Send.Resource,
                }
                   .Property(Send.FromEmail, "issentialz@gmail.com")
                   .Property(Send.FromName, "SkillSphere-No Reply")
                   .Property(Send.Subject, "Registration - Password")
                   .Property(Send.TextPart, $"{password}")
                   .Property(Send.HtmlPart, GetMailHtmlTemplate(password))
                   .Property(Send.Recipients,
                   new JArray {
                    new JObject
                    {
                        {
                            "Email", email
                        }
                    }});

                var response = await client.PostAsync(request);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in SendVerificationEmail: {ex.Message}", ex);
                return false;
            }
        }

        private static string GetMailHtmlTemplate(string passwprd)
            => @$"<div style=""font-family: Helvetica,Arial,sans-serif;min-width:1000px;overflow:auto;line-height:2"">
                                          <div style=""margin:50px auto;width:70%;padding:20px 0"">
                                            <div style=""border-bottom:1px solid #eee"">
                                              <a href="""" style=""font-size:1.4em;color: #00466a;text-decoration:none;font-weight:600"">SkillSphere</a>
                                            </div>
                                            <p style=""font-size:1.1em"">Hello,</p>
                                            <p>Thank you for choosing SkillSphere. Use the following Password to complete your Sign Up procedures. <br>Your password is:</p>
                                            <h2 style=""background: #00466a;margin: 0 auto;width: max-content;padding: 0 10px;color: #fff;border-radius: 4px;"">{passwprd}</h2>
                                            <br><br>
                                            <p>Please do not share this password with anyone, and change it as soon as possible.</p>
                                            <p style=""font-size:0.9em;"">Regards,<br />SkillSphere</p>
                                            <hr style=""border:none;border-top:1px solid #eee"" />
                                            <div style=""float:right;padding:8px 0;color:#aaa;font-size:0.8em;line-height:1;font-weight:300"">
                                              <p>SkillSphere</p>
                                              <p><a href=""https://skillsphere.com"" style=""color:#00466a;text-decoration:none"">skillsphere.com</a></p>
                                            </div>
                                          </div>
                                        </div>";

        public Task<bool> SendEmail(string fromEmail, string toEmail, string subject, string message)
        {
            throw new NotImplementedException();
        }
    }
}
