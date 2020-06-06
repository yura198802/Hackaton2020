using System.Threading.Tasks;

namespace Monica.MailKit.EMailConfigService
{
    public interface IEmailConfigService
    {
        /// <summary>
        /// ��������� �������� ��� SMTP � ������
        /// </summary>
        /// <returns></returns>
        Task<SendEMailConfiguration> GetConfig();
    }
}
