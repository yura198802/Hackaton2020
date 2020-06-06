using Hackaton.AI.EngineAI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Hackaton.AI.DataModel;
using Hackaton.CrmDbModel.Model;
using Hackaton.CrmDbModel.ModelDto;
using Monica.Core.Utils;

namespace Hackaton.AI.EngineAI.Classes
{
    public class InputLayer : IInputLayer
    {
        protected List<List<OutputCodeWord>> _outputCodeWordsArray;
        private WordDbContext _wordDbContext;
        
        //  protected IEnumerable<EncodingWord> _encodingWords;
        public InputLayer()
        {
            _wordDbContext = AutoFac.Resolve<WordDbContext>();
            // _outputCodeWordsArray = outputCodeWords;
        }

        public virtual double[][] GetInputsData()
        {
            try
            {
                List<double[]> array = new List<double[]>();



                return array.ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception("Не удалось псформировать входные данные, на этапе обученя ", ex);
            }

        }
        public bool CreateTraningInDB(string jsonSource)
        {
            int index = 0;
            try
            {
                IVocalabry iVocalabry = new Vocalabry(_wordDbContext);
                IEngineParser engineParser = new EngineParser(iVocalabry.GetNonPersistentVocalabry());
                List<ClassificatorItem> oMycustomclassname = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ClassificatorItem>>(jsonSource);
                foreach (var item in oMycustomclassname)
                {
                    index++;
                    if (index < 204)
                        continue;
                    h_message newMessage = new h_message();
                    newMessage.TextType = item.type_name;
                    newMessage.TextCategory = item.cat_name;
                    newMessage.TextKind = item.kind_name;
                    var array = engineParser.GetOuptputVectors(item.type_name);
                    string tmpVector = string.Empty;
                    foreach (var vector in GetTrainSet(array))
                    {
                        tmpVector += (vector.ToString()).Replace(',', '.') + ",";
                    }

                    tmpVector = tmpVector.Remove(tmpVector.Length - 1);
                    newMessage.Vector = tmpVector;
                    _wordDbContext.Add(newMessage);
                    _wordDbContext.SaveChanges();
                }
                _wordDbContext.SaveChanges();
                Console.WriteLine(oMycustomclassname.Count);

                return true;
            }
            catch (Exception ex)
            {
                try
                {
                    _wordDbContext.SaveChanges();
                    return false;

                }
                catch
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// Нужно вызвать в цикле, по всей выборке сообщений для обучения.
        /// </summary>
        /// <param name="outputCodeWords"></param>
        /// <param name="validCategory"></param>
        public virtual IEnumerable<double> GetTrainSet(List<OutputCodeWord> outputCodeWords)
        {
            try
            {
                var encodingWords = GetEncodingWords(outputCodeWords);
                List<double> resultVector = GetEmptyVector50();
                foreach (var encd in encodingWords)
                {
                    if (string.IsNullOrWhiteSpace(encd.Bigramm))
                        continue;
                    for (int i = 0; i < 50; i++)
                    {
                        resultVector[i] += encd.Vector[i];
                    }
                }
                //for(int i = 0; i<50; i++)
                //{
                //    resultVector[i] = Math.Pow(1 + Math.Exp(-resultVector[i]), -1);
                //}
                return resultVector;
            }
            catch (Exception ex)
            {
                throw new Exception("Не удалось сформировать входной слой", ex);
            }
        }
        protected virtual IEnumerable<EncodingWord> GetEncodingWords(List<OutputCodeWord> outputCodeWords)
        {
            try
            {
                List<EncodingWord> allEncodingWords = new List<EncodingWord>();
                List<double> resultVector = GetEmptyVector50();
                foreach (var word in outputCodeWords)
                    allEncodingWords = allEncodingWords.Union(word.EncodingWords).ToList();
                return allEncodingWords;
            }
            catch (Exception ex)
            {
                throw new Exception("Не удалось получить все биграммы, ", ex);
            }
        }
        protected virtual List<double> GetEmptyVector50()
        {
            List<double> resultVector = new List<double>();
            for (int y = 0; y < 50; y++)
                resultVector.Add(0);
            return resultVector;
        }
    }
}
