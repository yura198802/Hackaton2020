using Hackaton.AI.EngineAI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Hackaton.CrmDbModel.ModelDto;

namespace Hackaton.AI.EngineAI.Classes
{
    public class Bigramm : IBigramm
    {
        public virtual EncodingWord GetFirstBigramm(IEnumerable<WordNotPersistent> vocalabryFirstBigramm, ref string tmpWordSource)
        {
            try
            {
                int countDebug = 0;
                //List<WordNotPersistent> debugArray = vocalabryFirstBigramm.OrderByDescending(f => f.name.Length).ToList();
                //Console.WriteLine(debugArray[1878].name);
                foreach (var beginBigram in vocalabryFirstBigramm.OrderByDescending(f => f.name.Length))
                {

                    string realBigram = beginBigram.name.Substring(1);
                    if (realBigram.Length == tmpWordSource.Length)
                    {
                        int indexValidate = 0;
                        for (int i = 0; i <= realBigram.Length - 1; i++)
                        {
                            if (tmpWordSource.ToUpper()[i] == realBigram.ToUpper()[i])
                                indexValidate++;
                        }
                        if (indexValidate == realBigram.Length)
                        {
                            tmpWordSource = string.Empty;
                            EncodingWord encodingWord = new EncodingWord();
                            encodingWord.Vector = beginBigram.Vector;
                            encodingWord.Bigramm = realBigram;
                            return encodingWord;
                        }
                    }

                    if (realBigram.Length < tmpWordSource.Length)
                    {
                        countDebug++;
                        int indexValidate = 0;
                        for (int i = 0; i <= realBigram.Length - 1; i++)
                        {
                            if (tmpWordSource.ToUpper()[i] == realBigram.ToUpper()[i])
                                indexValidate++;
                        }
                        if (indexValidate == realBigram.Length)
                        {
                            tmpWordSource = tmpWordSource.Substring(indexValidate);
                            EncodingWord encodingWord = new EncodingWord();
                            encodingWord.Vector = beginBigram.Vector;
                            encodingWord.Bigramm = realBigram;
                            return encodingWord;
                        }
                    }
                    countDebug++;
                    Console.WriteLine(countDebug);
                }
                tmpWordSource = string.Empty;
                return new EncodingWord();
            }
            catch (Exception ex)
            {
                throw new Exception("Не удалось определить начальный слог из входного словаря, " + ex.Message);
            }
        }

        public virtual List<EncodingWord> GetEndBigramms(IEnumerable<WordNotPersistent> vocalabryEndBigramm, ref string tmpSource)
        {
            try
            {
                List<EncodingWord> codeWords = new List<EncodingWord>();
                while (tmpSource != string.Empty)
                {
                    codeWords.Add(GetEndBigrammsRecursion(vocalabryEndBigramm, ref tmpSource));
                }
                return codeWords;
            }
            catch (Exception ex)
            {
                throw new Exception("Не удалось определить конечные слоги из входного словаря, " + ex.Message);
            }
        }
        public virtual EncodingWord GetEndBigrammsRecursion(IEnumerable<WordNotPersistent> vocalabryEndBigramm, ref string tmpSource)
        {
            try
            {
                foreach (var endBigram in vocalabryEndBigramm.OrderByDescending(f => f.name.Length))
                {
                    if (endBigram.name.Length == tmpSource.Length)
                    {
                        int indexValidate = 0;
                        for (int i = 0; i <= endBigram.name.Length - 1; i++)
                        {
                            if (tmpSource.ToUpper()[i] == endBigram.name.ToUpper()[i])
                                indexValidate++;
                        }
                        if (indexValidate == endBigram.name.Length)
                        {
                            tmpSource = string.Empty;
                            EncodingWord encodingWord = new EncodingWord();
                            encodingWord.Vector = endBigram.Vector;
                            encodingWord.Bigramm = endBigram.name;
                            return encodingWord;
                        }
                    }
                    if (endBigram.name.Length < tmpSource.Length)
                    {
                        int indexValidate = 0;
                        for (int i = 0; i <= endBigram.name.Length - 1; i++)
                        {
                            if (tmpSource.ToUpper()[i] == endBigram.name.ToUpper()[i])
                                indexValidate++;
                        }
                        if (indexValidate == endBigram.name.Length)
                        {
                            tmpSource = tmpSource.Substring(indexValidate);
                            EncodingWord encodingWord = new EncodingWord();
                            encodingWord.Vector = endBigram.Vector;
                            encodingWord.Bigramm = endBigram.name;
                            return encodingWord;
                        }
                    }
                }
                tmpSource = string.Empty;
                return new EncodingWord();
            }
            catch (Exception ex)
            {
                throw new Exception("Не удалось определить конечные слоги из входного словаря, " + ex.Message);
            }
        }
    }
}
