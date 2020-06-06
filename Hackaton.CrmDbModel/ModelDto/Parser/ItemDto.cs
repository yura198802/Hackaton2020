using System;
using System.Collections.Generic;
using System.Text;
using Hackaton.CrmDbModel.Model.LoadDocument;

namespace Hackaton.CrmDbModel.ModelDto.Parser
{
    public class FileRtf
    {
        /// <summary>
        /// наименование док-та
        /// </summary>
        public string Caption { get; set; }
        /// <summary>
        /// вид документа
        /// </summary>
        public string VidDoc { get; set; }
        /// <summary>
        /// Уточнение вида док-та
        /// </summary>
        public string Category { get; set; }
        //public string FileSize { get; set; }
        //TODO и другие свойства

        public List<ItemDto> Items { get; set; }
        public int FileSize { get; set; }
    }

    public class ItemDto
    {
        public ItemDto ParentId { get; set; }
        public string TextContent { get; set; }
        public string  Number { get; set; }
        public bool IsRoot { get; set; }
        public DocumentItem DocumentItem { get; set; }

        public void Parent()
        {
            var item = new ItemDto() ;
            item.Number = "I.";
            item.TextContent = "Общие положения";
            var item2 = new ItemDto()
            {
                TextContent = "Коммерческий агент должен знать:",
                Number = "4",
                ParentId =  item
            };
            var item3 = new ItemDto()
            {
                Number = "4.1",
                TextContent = "Нормативные правовые акты, положения, инструкции, другие руководящие материалы и документы, касающиеся ведения бизнеса",
                ParentId = item2
            };
        }
    }



}
