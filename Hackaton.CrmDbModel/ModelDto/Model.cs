using System.Collections.Generic;

namespace Hackaton.CrmDbModel.ModelDto
{
    public class OutputCodeWord
    {
        public string Source { get; set; }
        public List<EncodingWord> EncodingWords { get; set; }
        public OutputCodeWord()
        {
            EncodingWords = new List<EncodingWord>();
        }
    }
    public class WordNotPersistent
    {
        public int sysid { get; set; }
        public string name { get; set; }
        public List<double> Vector { get; set; }
        public bool IsFirst { get; set; }
        public WordNotPersistent(WordDTO wordDTO)
        {
            sysid = wordDTO.sysid;
            name = wordDTO.name;
            Vector = new List<double>();
            foreach (string digit in wordDTO.vecotr.Split(' '))
            {
                double i;
                if (double.TryParse(digit.Replace('.', ','), out i))
                    Vector.Add(i);
            }
            IsFirst = wordDTO.isfirst;
        }
    }
    public class EncodingWord
    {
        public string Bigramm { get; set; }
        public List<double> Vector { get; set; }
        public EncodingWord()
        {
            Vector = new List<double>();
        }
    }
    public class ClassifcatorData
    {
        public List<ClassifcatorData> classifcatordata { get; set; }
        public ClassifcatorData()
        {
            classifcatordata = new List<ClassifcatorData>();
        }
    }
    public class ClassificatorItem
    {
        public string cat_name { get; set; }
        public string kind_name { get; set; }
        public string type_name { get; set; }

    }


    public enum NeuronTypes { Hiiden, Output }
    public enum Category { ExploitationHoues, Other, ColdWather, BaseWather };
    public enum Kind { A, B, C, D, E, F, G, J, K, L, M, N, O, P, R, S, T, Q, V, W, X, Y, Z, AB, AC, AD, AH, AJ, AK, AS, AL, AP, AE, AM, AT, AW }
    public enum ModeType { Get, Set };
}
