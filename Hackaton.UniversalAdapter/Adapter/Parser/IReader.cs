using System.Collections.Generic;

namespace Hackaton.UniversalAdapter.Adapter.Parser
{
    public interface IReader
    {
        /// <summary>
        /// Список, хранит в себе строки считанного файла
        /// </summary>
        List<string> Lines { get; set; }
    }
}
