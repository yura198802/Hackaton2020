using System.Collections.Generic;
using System.Threading.Tasks;
using Hackaton.UniversalAdapter.Adapter.AiEngine.Model;

namespace Hackaton.UniversalAdapter.Adapter.AiEngine.Interface
{
    /// <summary>
    /// Загрузчик данных для морфологического разбора
    /// </summary>
    public interface ILoaderInfoAotRu
    {
        /// <summary>
        /// Загрузить данные с сервиса Aot.ru
        /// </summary>
        /// <param name="content">Текст</param>
        /// <returns>Модель</returns>
        Task<List<AotModel>> LoaderAotModel(string content);
    }
}
