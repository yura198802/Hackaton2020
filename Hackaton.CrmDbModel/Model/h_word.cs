using System.ComponentModel.DataAnnotations;
using Hackaton.CrmDbModel.ModelDto;

namespace Hackaton.CrmDbModel.Model
{
    /// <summary>
    /// Модель словарей
    /// </summary>
    public class h_word 
    {
        [Key]
        public int Sysid { get; set; }
        public string Name { get; set; }
        public bool IsFirst { get; set; }
        public string Vector { get; set; }

        public WordDTO GetWordDTO(h_word templateSource)
        {
            WordDTO word = new WordDTO();
            word.name = templateSource.Name;
            word.sysid = templateSource.Sysid;
            word.vecotr = templateSource.Vector;
            word.isfirst = templateSource.IsFirst;
            return word;
        }

    }
}
