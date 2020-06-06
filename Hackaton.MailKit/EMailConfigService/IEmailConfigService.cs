using System.Threading.Tasks;

namespace Monica.MailKit.EMailConfigService
{
    public interface IEmailConfigService
    {
        /// <summary>
        /// Получение настроек для SMTP и других
        /// </summary>
        /// <returns></returns>
        Task<SendEMailConfiguration> GetConfig();
    }
}
