using System.Net.Mail;
using System.Net.Mime;

namespace CalisanYonetimSistemi.WebApiLayer.MyExtensions.Email
{
    public class EmailHelper
    {
        public async Task<bool> SendEmail(string email, string message, string subject, IFormFile attachment = null)
        {

            #region Mail Ayarlari

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("aksiot@hotmail.com");
            mailMessage.To.Add(email);
            mailMessage.Body = message;
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;

            if (attachment != null)
            {
                Attachment mailAttachment = new Attachment(attachment.OpenReadStream(), attachment.FileName, MediaTypeNames.Application.Octet);
                mailMessage.Attachments.Add(mailAttachment);
            }

            #endregion

            #region SMTP Ayarlari
            SmtpClient smtpClient = new SmtpClient();

            smtpClient.Credentials = new System.Net.NetworkCredential("aksiot@hotmail.com", "iot61aks");//("cinaliveli5@gmail.com", "cvsdhgjolvhnfnbq");
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.Host = "smtp-mail.outlook.com"; /*"smtp.gmail.com";*/
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            #endregion

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception)
            {
                throw new Exception($"Mail gönderimi sırasında bir hata oluştu. Lütfen tekrar deneyin.");
            }
        }
    }
}
