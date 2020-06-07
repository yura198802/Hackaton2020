using System.Collections.Generic;

namespace Hackaton.CrmDbModel.ModelDto.AiWord
{
    public class InfoDocument
    {
        public string Title { get; set; } //TODO например общее положение
        public List<InfoCategoty> Items { get; set; } //Список категорий, например что нужно сделать
    }

    public class InfoCategoty
    {
        public string Title { get; set; }
        public List<InfoItem> Items { get; set; }

    }
    
    public class InfoItem
    {
        public string Title { get; set; }
        public List<string> ProfName { get; set; }
    }
}
