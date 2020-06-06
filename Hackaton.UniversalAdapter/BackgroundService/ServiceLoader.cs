using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Hackaton.UniversalAdapter.Adapter.Loader;
using Microsoft.Extensions.Logging;
using Monica.Core.Constants;

namespace Hackaton.UniversalAdapter.BackgroundService
{
    public class ServiceLoader : Microsoft.Extensions.Hosting.BackgroundService
    {
        private ILoaderFile _loaderFile;
        private string _pathDoc;
        private readonly ILogger _logger;
        protected readonly ManualResetEvent EventEntityExists = new ManualResetEvent(false);

        public ServiceLoader(ILogger logger, ILoaderFile loaderFile)
        {
            _logger = logger;
            _loaderFile = loaderFile;
            _pathDoc = Path.Combine(GlobalSettingsApp.CurrentAppDirectory, "Documents");

            EventEntityExists.Set();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Рабочий поток успешно запущен.");

            while (!stoppingToken.IsCancellationRequested)
            {
                if (!EventEntityExists.WaitOne(100))
                {
                    await Task.Delay(100, stoppingToken);
                    continue;
                }

                try
                {
                    var files = Directory.GetFiles(_pathDoc, "*.rtf*");
                    foreach (var file in files)
                    {
                        await _loaderFile.LoadFileByDatabase(file);
                        await _loaderFile.RemoveFile(file);
                    }

                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "В процессе работы, возникла ошибка.");
                }
                finally
                {
                    EventEntityExists.Reset();
                    EventEntityExists.Set();

                    await Task.Delay(100, stoppingToken);
                }
            }
        }
        
    }
}
