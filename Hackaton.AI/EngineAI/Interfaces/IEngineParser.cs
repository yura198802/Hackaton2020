using System.Collections.Generic;
using Hackaton.CrmDbModel.ModelDto;

namespace Hackaton.AI.EngineAI.Interfaces
{
    public interface IEngineParser
    {
        OutputCodeWord CodeOneWord(string source);
        List<OutputCodeWord> GetOuptputVectors(string message);
    }
}
