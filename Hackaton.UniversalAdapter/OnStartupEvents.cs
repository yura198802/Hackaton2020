using Hackaton.UniversalAdapter.BackgroundService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Monica.Core.Events;

namespace Hackaton.UniversalAdapter
{
    public class OnStartupEvents : IOnStartupEvents
    {
        public void OnConfigureBefore(IApplicationBuilder applicationBuilder, IHostingEnvironment hostingEnvironment,
            ILoggerFactory loggerFactory) { }

        public void OnConfigureAfter(IApplicationBuilder applicationBuilder, IHostingEnvironment hostingEnvironment,
            ILoggerFactory loggerFactory) { }

        public void OnConfigureServicesAfterAddMvc(IServiceCollection services, IMvcBuilder mvcBuilder)
        {
        }

        /// <summary>
        /// Нужно зарешистрировать контекст для работы с БД.
        /// Возможно будет больше контекстов. Сейчас обязательно контекст для работы с БД IdentityServer4
        /// </summary>
        /// <param name="services"></param>
        public void OnConfigureServicesBeforeAddMvc(IServiceCollection services)
        {
        }

        public void OnInitBackendService(IServiceCollection services)
        {
            services.AddTransient<ServiceLoader>();
        }

        public void OnConfigureBeforeUseMvc(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory iLoggerFactory) { }

        public void OnConfigureAfterUseMvc(IApplicationBuilder app, IHostingEnvironment env,
            ILoggerFactory iLoggerFactory)
        {
        }
    }
}
