using System.Text;

namespace Hackaton.UniversalAdapter.Adapter.Parser.Helper.Utils
{
    public static class ParserUtils
    {
        public static string ByteArrToString1251(this byte[] bytes)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            return Encoding.GetEncoding("windows-1251").GetString(bytes); 
        }
    }
}
