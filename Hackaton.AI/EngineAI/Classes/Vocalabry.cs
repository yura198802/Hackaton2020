using System.Collections.Generic;
using Hackaton.AI.EngineAI.Interfaces;
using Hackaton.CrmDbModel.Model;
using Hackaton.CrmDbModel.ModelDto;

namespace Hackaton.AI.EngineAI.Classes
{
    public class Vocalabry : IVocalabry
    {
        private WordDbContext _wordDbContext;
        public Vocalabry(WordDbContext wordDbContext)
        {
            _wordDbContext = wordDbContext;
        }

        public List<WordNotPersistent> GetNonPersistentVocalabry()
        {
            List<WordNotPersistent> wordNotPersistents = new List<WordNotPersistent>();
            var hWords = _wordDbContext.h_word;
            foreach (var word in hWords)
            {
                WordNotPersistent wordNotPersistent = new WordNotPersistent(word.GetWordDTO(word));
                wordNotPersistents.Add(wordNotPersistent);
            }
            return wordNotPersistents;
        }
    }
}
