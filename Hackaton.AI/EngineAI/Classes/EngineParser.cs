using Hackaton.AI.EngineAI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Hackaton.CrmDbModel.Model;
using Hackaton.CrmDbModel.ModelDto;
using Monica.Core.Utils;

namespace Hackaton.AI.EngineAI.Classes
{
    public class EngineParser : IEngineParser
    {
        protected IEnumerable<WordNotPersistent> VocalabryFirstBigramm { get; set; }
        protected IEnumerable<WordNotPersistent> VocalabryEndBigramm { get; set; }
        private WordDbContext _wordDbContext;
        public EngineParser(List<WordNotPersistent> wordNotPersistents)
        {
            _wordDbContext = AutoFac.Resolve<WordDbContext>();
            VocalabryFirstBigramm = wordNotPersistents?.Where(f => f.IsFirst);
            VocalabryEndBigramm = wordNotPersistents?.Where(f => !f.IsFirst);
        }
        public virtual OutputCodeWord CodeOneWord(string sourceWord)
        {
            try
            {
                List<EncodingWord> codeWords = new List<EncodingWord>();
                string tmpSource = sourceWord;
                IBigramm iBigramm = new Bigramm();
                codeWords.Add(iBigramm.GetFirstBigramm(this.VocalabryFirstBigramm, ref tmpSource));
                foreach (var item in iBigramm.GetEndBigramms(this.VocalabryEndBigramm, ref tmpSource))
                    codeWords.Add(item);
                OutputCodeWord outputCodeWord = new OutputCodeWord();
                outputCodeWord.Source = sourceWord;
                outputCodeWord.EncodingWords = codeWords;
                return outputCodeWord;
            }
            catch (Exception ex)
            {
                throw new Exception("Не удалось закодировать используя входной словарь");
            }
        }
        public virtual List<OutputCodeWord> GetOuptputVectors(string message)
        {
            string inputMeessage = message;
            string[] words = inputMeessage.Split(new[] { '/', ' ', '(', ')', ',', '*', ':', '.', ';', '-' });

            IVocalabry iVocalabry = new Vocalabry(_wordDbContext);
            IEngineParser engineParser = new EngineParser(iVocalabry.GetNonPersistentVocalabry());
            List<OutputCodeWord> result = new List<OutputCodeWord>();
            int debugIndex = 0;
            foreach (var item in words)
            {
                if (string.IsNullOrWhiteSpace(item))
                    continue;
                debugIndex++;
                result.Add(engineParser.CodeOneWord(item));
            }
            return result;
        }

    }
}
