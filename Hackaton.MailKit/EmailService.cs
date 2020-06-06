using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MimeKit;
using MimeKit.Text;
using Monica.MailKit;
using Monica.MailKit.EMailConfigService;

namespace Hackaton.MailKit
{
	/// <summary>
	/// ������ �������� �����
	/// </summary>
    public class EmailService : IEmailService
    {
        /// <summary>
        /// ��������� ������������ �������
        /// </summary>
        private IEmailConfigService _emailConfigService;
        private IMailKitSmtpClient _mailKitSmtpClient;

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="emailConfigService"></param>
        /// <param name="mailKitSmtpClient"></param>
        public EmailService(IEmailConfigService emailConfigService, IMailKitSmtpClient mailKitSmtpClient)
        {
            _emailConfigService = emailConfigService;
            _mailKitSmtpClient = mailKitSmtpClient;
        }


		/// <summary>
		/// �������� ������
		/// </summary>
		/// <param name="mailTo">���� e-mail, �������� ","</param>
		/// <param name="mailCc">����� e-mail, �������� ","</param>
		/// <param name="mailBcc">������� ����� e-mail, �������� ","</param>
		/// <param name="subject">����</param>
		/// <param name="message">����</param>
		/// <param name="encoding">���������</param>
		/// <param name="isHtml">��� HTML</param>
		public async Task Send(string mailTo, string mailCc, string mailBcc, string subject, string message, Encoding encoding, bool isHtml = false)
        {
            await SendEmail(mailTo, mailCc, mailBcc, subject, message, encoding, isHtml);
        }


		/// <summary>
		/// �������� ������, ����������
		/// </summary>
		/// <param name="mailTo">���� e-mail, �������� ","</param>
		/// <param name="mailCc">����� e-mail, �������� ","</param>
		/// <param name="mailBcc">������� ����� e-mail, �������� ","</param>
		/// <param name="subject">����</param>
		/// <param name="message">����</param>
		/// <param name="encoding">���������</param>
		/// <param name="isHtml">��� HTML</param>
		public async Task SendAsync(string mailTo, string mailCc, string mailBcc, string subject, string message, Encoding encoding, bool isHtml = false)
        {
            await SendEmail(mailTo, mailCc, mailBcc, subject, message, encoding, isHtml);
        }

		/// <summary>
		/// ��������������� �������� ������
		/// </summary>
		/// <param name="mailTo">���� e-mail, �������� ","</param>
		/// <param name="mailCc">����� e-mail, �������� ","</param>
		/// <param name="mailBcc">������� ����� e-mail, �������� ","</param>
		/// <param name="subject">����</param>
		/// <param name="message">����</param>
		/// <param name="encoding">���������</param>
		/// <param name="isHtml">��� HTML</param>
		private async Task SendEmail(string mailTo, string mailCc, string mailBcc, string subject, string message, Encoding encoding, bool isHtml)
        {
            var modelSettings = await _emailConfigService.GetConfig();
            var to = new string[0];
            var cc = new string[0];
            var bcc = new string[0];
            if (!string.IsNullOrEmpty(mailTo))
	            to = mailTo.Split(',').Select(x => x.Trim()).ToArray();
            if (!string.IsNullOrEmpty(mailCc))
                cc = mailCc.Split(',').Select(x => x.Trim()).ToArray();
            if (!string.IsNullOrEmpty(mailBcc))
                bcc = mailBcc.Split(',').Select(x => x.Trim()).ToArray();

			// HACK �������� �������� ������������ E-mail ?
            var mimeMessage = new MimeMessage();

            // �� ���� ������ ��� � �������� �����
            mimeMessage.From.Add(new MailboxAddress(modelSettings.SmtpEmailDisplay, modelSettings.SmtpEmail));

            // ����
            foreach (string tos in to)
            {
                mimeMessage.To.Add(MailboxAddress.Parse(tos));
            }

            // �����
            foreach (string ccs in cc)
            {
                mimeMessage.Cc.Add(MailboxAddress.Parse(ccs));
            }

            // ������� �����
            foreach (string bccs in bcc)
            {
                mimeMessage.Bcc.Add(MailboxAddress.Parse(bccs));
            }

            // ���� ������
            mimeMessage.Subject = subject;

            // ���� ������
            TextPart body;

            body = isHtml ? new TextPart(TextFormat.Html) : new TextPart(TextFormat.Text);
            
            // ���������
            body.SetText(encoding, message);

            // ���� ������
            mimeMessage.Body = body;

            using (var client = await _mailKitSmtpClient.SmtpClient())
            {
                await client.SendAsync(mimeMessage);
                await client.DisconnectAsync(true);
			}
        }
    }
}
