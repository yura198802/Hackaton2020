using Autofac;
using Monica.Core.Attributes;

namespace Hackaton.CrmDbModel.Autofac
{
    /// <summary>
    /// Модуль IoC контейнера
    /// </summary>
    [CommonModule]
    public class MonicaCrmModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            
        }
    }
}
