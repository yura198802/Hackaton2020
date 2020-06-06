﻿using Autofac;
using Hackaton.MailKit;
using Monica.Core.Attributes;
using Monica.MailKit.EMailConfigService;

namespace Monica.MailKit.AutofacModules
{
    [CommonModule]
    public class MailKitServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EmailConfigService>().As<IEmailConfigService>();
            builder.RegisterType<MailKitSmtpClient>().As<IMailKitSmtpClient>();
            builder.RegisterType<EmailService>().As<IEmailService>();

        }
    }
}
