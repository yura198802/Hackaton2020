using System.Threading.Tasks;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Hackaton.MailKit
{
	/// <summary>
	/// Интерфейс для получения клиента SMTP
	/// </summary>
    public interface IMailKitSmtpClient
    {
        /// <summary>
        /// smtp client
        /// </summary>
        Task<SmtpClient> SmtpClient();

    }
}
