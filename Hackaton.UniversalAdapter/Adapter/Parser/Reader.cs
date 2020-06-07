using System;
using System.Collections.Generic;
using System.IO;
using Hackaton.UniversalAdapter.Adapter.Parser.Helper.Utils;

namespace Hackaton.UniversalAdapter.Adapter.Parser
{
    public class Reader : IReader
    {
        public List<string> Lines { get; set; }
        public Reader(byte[] file)
        {
            ReadDocument(file);
        }
        private List<string> ReadDocument(byte[] file)
        {
            Lines = new List<string>();
            try
            {
                var fileString = file.ByteArrToString1251();
                foreach (var line in fileString.Split('\n'))
                {
                    Lines.Add(line);
                }
                return Lines;
            }
            catch(Exception e)
            {
                return new List<string> ();
            }
        }
    }
}
