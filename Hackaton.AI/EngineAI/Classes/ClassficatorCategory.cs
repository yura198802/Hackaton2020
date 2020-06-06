using System;
using System.Collections.Generic;
using System.Linq;
using Accord.MachineLearning.VectorMachines;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Statistics.Kernels;
using Hackaton.AI.DataModel;
using Hackaton.AI.EngineAI.Interfaces;
using Hackaton.CrmDbModel.Model;

namespace Hackaton.AI.EngineAI.Classes
{
    public class ClassficatorCategory : ICategotyClassifiction
    {
        
        public MulticlassSupportVectorMachine<Linear> _machine { get; set; }
        protected MulticlassSupportVectorLearning<Linear> _categoryClassification { get; set; }
        protected List<h_message> _messages { get; set; }
        private IVocalabry _vocalabry { get; set; }
        protected IEngineParser EngineParser { get; set; }
        private WordDbContext _wordDbContext;


        public ClassficatorCategory(WordDbContext wordDbContext, IVocalabry vocalabry)
        {
            _wordDbContext = wordDbContext;
            _vocalabry = vocalabry;
            _categoryClassification = new MulticlassSupportVectorLearning<Linear>()
            {
                Learner = (p) => new LinearDualCoordinateDescent()
                {
                    Loss = Loss.L2
                }
            };

            _messages = InitLearnData();
            EngineParser = new EngineParser(_vocalabry.GetNonPersistentVocalabry());
            InitLearningMachine();
        }

        public string GetCategoryMessage(string customMessage)
        {
            try
            {
                var array = EngineParser.GetOuptputVectors(customMessage);
                IInputLayer inputLayer = new InputLayer();
                double[][] vs =
                {
                    inputLayer.GetTrainSet(array).ToArray()
                };
                var res = _machine.Decide(vs);
                return OutIntToMessage(res.FirstOrDefault());
            }
            catch
            {
                return "Иное";
            }
        }
        protected bool InitLearningMachine()
        {
            int y = 0;
            try
            {
                List<double[]> input = new List<double[]>();
                List<int> outTmp = new List<int>();

                foreach (var message in _messages)
                {
                    y++;
                    if (message.Vector == null)
                        continue;
                    outTmp.Add(MessageTypeToInt(message.TextCategory));
                    List<double> vector = new List<double>();

                    foreach (string digit in message.Vector.Split(','))
                    {
                        double i;
                        if (double.TryParse(digit.Replace('.', ','), out i))
                            vector.Add(i);
                    }
                    if (vector.Count == 50)
                        input.Add(vector.ToArray());
                }
                _machine = _categoryClassification.Learn(input.ToArray(), outTmp.ToArray());
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Не удалось инициализировать AI", ex);
            }

        }

        protected List<h_message> InitLearnData()
        {
            try
            {
                return _wordDbContext.h_message.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Не удалось сформировать обучающую выборку. ", ex);
            }

        }
        protected int MessageTypeToInt(string message)
        {
            switch (message)
            {
                case "Эксплуатация жилого фонда": return 0;
                case "Иное": return 1;
                case "ХВС до ввода в МКД (только для РСО)": return 2;
                case "Водоотведение до ввода в МКД (только для РСО)": return 3;
                default: return 1;
            }
        }
        protected string OutIntToMessage(int outValue)
        {
            switch (outValue)
            {
                case 0: return "Эксплуатация жилого фонда";
                case 1: return "Иное";
                case 2: return "ХВС до ввода в МКД (только для РСО)";
                case 3: return "Водоотведение до ввода в МКД (только для РСО)";
                default: return "Иное";
            }
        }
    }
}
