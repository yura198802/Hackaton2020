using Autofac;
using Hackaton.UniversalAdapter.Adapter.AiEngine.Classes;
using Hackaton.UniversalAdapter.Adapter.AiEngine.Classes.AIParseEngine.Classes;
using Hackaton.UniversalAdapter.Adapter.AiEngine.Classes.AIParseEngine.Interfaces;
using Hackaton.UniversalAdapter.Adapter.AiEngine.Interface;
using Hackaton.UniversalAdapter.Adapter.Loader;
using Hackaton.UniversalAdapter.Adapter.Parser;
using Hackaton.UniversalAdapter.Adapter.User;
using Monica.Core.Attributes;

namespace Hackaton.UniversalAdapter.Autofac
{
    /// <summary>
    /// Модуль автофака
    /// </summary>
    [CommonModule]
    public class MonicaUniversalModule : Module
    {
        /// <summary>
        /// загрузить записимости
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ParserAdapterAdapter>().As<IParserAdapter>();
            builder.RegisterType<LoaderInfoAotRu>().As<ILoaderInfoAotRu>();
            builder.RegisterType<AiParser>().As<IAiParser>();
            builder.RegisterType<UserAdapter>().As<IUserAdapter>();
            builder.RegisterType<LoaderFile>().As<ILoaderFile>();
            builder.RegisterType<AiSentenceEngine>().As<IAiSentence>();
            builder.RegisterType<AiGroupEngine>().As<IAiGroup>();
        }
    }
}
