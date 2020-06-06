namespace Hackaton.CrmDbModel.ModelDto
{
    public class WordDTO
    {
        public int sysid { get; set; }
        public string name { get; set; }
        public string vecotr { get; set; }
        public bool isfirst { get; set; }
        public WordDTO()
        {
            name = string.Empty;
        }
    }
}
