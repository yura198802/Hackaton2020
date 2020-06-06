using System.Collections.Generic;

namespace Hackaton.CrmDbModel.ModelDto.AiWord
{
    public class PredicatDto
    {
        /// <summary>
        /// Заголовок
        /// </summary>
        public string Title { get; set; }
    }
    public class PredicatEntityDto
    {
        /// <summary>
        /// Заголовок
        /// </summary>
        public string Title { get; set; }
        public int Last { get; set; }
        public int Start { get; set; }
        /// <summary>
        /// Список предложений НПА
        /// </summary>
        public List<InfoNpa> InfoNpas { get; set; }
    }

    public class InfoNpa
    {
        public string Title { get; set; }
        public string Paragraph { get; set; }
    }
}
