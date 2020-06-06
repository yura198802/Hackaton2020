using Autofac;
using Hackaton.AI.EngineAI.Classes;
using Hackaton.AI.EngineAI.Interfaces;
using Monica.Core.Attributes;

namespace Hackaton.AI.AutofacModule
{
    /// <summary>
    /// Модуль IoC контейнера
    /// </summary>
    [CommonModule]
    public class HackatonAiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ClassficatorCategory>().As<ICategotyClassifiction>();
            builder.RegisterType<Vocalabry>().As<IVocalabry>();
        }
    }
}
