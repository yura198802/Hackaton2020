using System;
using System.IO;

namespace Hackaton.UniversalAdapter.Adapter.Parser
{
    public class Reader : IReader
    {
        private string _contentFile = string.Empty;
        public string ReadDoc(string path="")
        {
            try
            {
                StreamReader reader = new StreamReader(path);
                var _contentFile = reader.ReadToEnd();
                return _contentFile;
            }
            catch(Exception e)
            {
                return _contentFile = string.Empty;
            }
        }
    }
}
