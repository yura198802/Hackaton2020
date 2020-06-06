using System.Collections.Generic;
using Hackaton.CrmDbModel.ModelDto;

namespace Hackaton.AI.EngineAI.Interfaces
{
    public interface IInputLayer
    {
        IEnumerable<double> GetTrainSet(List<OutputCodeWord> outputCodeWords);
        double[][] GetInputsData();
    }
}
