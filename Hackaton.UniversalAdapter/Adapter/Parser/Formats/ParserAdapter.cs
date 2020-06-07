using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Hackaton.CrmDbModel.Model;
using Hackaton.CrmDbModel.Model.LoadDocument;
using Hackaton.CrmDbModel.ModelDto.Parser;
using Hackaton.UniversalAdapter.Adapter.Parser.Helper;
using Hackaton.UniversalAdapter.Adapter.Parser.Helper.Creator;

namespace Hackaton.UniversalAdapter.Adapter.Parser.Formats
{

    public class ParserAdapter : IParserAdapter
    {
        /// <summary>
        /// Список Итемов
        /// </summary>
        List<ItemDto> items = new List<ItemDto>();

        public List<string> _lines = new List<string>();

        /// <summary>
        /// словарь стартовых и конечных индексов ячеек
        /// </summary>
        List<KeyValuePair<int, int>> _cells = new List<KeyValuePair<int, int>>();

        /// <summary>
        /// список всех параграфов документа
        /// </summary>
        List<Paragraph> _par = new List<Paragraph>();

        /// <summary>
        /// 
        /// </summary>
        List<ParagraphContent> _cotent = new List<ParagraphContent>();


        /// <summary>
        /// Метод парса документов
        /// </summary>
        /// <param name="fileContent">массив байтов</param>
        /// <returns></returns>
        public Task<FileRtf> ParseDocument(byte[] fileContent)
        {
            var reader = new Reader(fileContent);
            _lines = reader.Lines;
            return Task.FromResult(CreateRtf(fileContent.Length));
        }

        /// <summary>
        /// Создание объекта документа
        /// </summary>
        /// <returns></returns>
        private FileRtf CreateRtf(int fileSize)
        {
            var result = new FileRtf();
            InitDictionaries(_lines);
            result.FileSize = fileSize;
            result.Caption = GetCellValue(1);
            result.VidDoc = GetCellValue(3);
            result.Category = GetCellValue(8);
            result.ProfName = GetCellValue(5);
            result.Items = CreateItem(_cotent);
            return result;
        }

        private void InitDictionaries(List<string> lines)
        {
            var startIndex = 0;
            for (var i = 0; i < lines.Count; i++)
            {
                if (lines[i].Contains("\\ql\r"))
                    startIndex = i;
                else if (lines[i].Contains("\\qc\r"))
                    startIndex = i;
                else if (lines[i].Contains("\\cell\r"))
                    _cells.Add(new KeyValuePair<int, int>(++startIndex, i));
                if (lines[i].Contains(@"\par\pard\keep\plain\cf4\f4\fs25\qc\ri0\b"))
                    _par.Add(new Paragraph(new KeyValuePair<int, string>(++i, lines[i])));
            }
            _cotent = GetContent();
        }
        /// <summary>
        /// получение значения ячейки по её индексу
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private string GetCellValue(int index)
        {
            var result = string.Empty;
            for (var i = 0; i < _cells.Count; i++)
            {
                if (i == index && _cells[i].Key == _cells[i].Value)
                {
                    result = _lines[_cells[i].Key];
                    break;
                }
            }
            result = RemoveTag(result);
            return result;
        }

        private List<ParagraphContent> GetContent()
        {
            List<ParagraphContent> contents = new List<ParagraphContent>();
            int i = 0;
            foreach (var par in _par)
            {
                if (i < _par.Count - 1)
                    contents.Add(new ParagraphContent(_lines, par, _par[++i].IndexOfList));
                else
                    contents.Add(new ParagraphContent(_lines, par, _lines.Count));
            }
            return contents;
        }

        private List<ItemDto> CreateItem(List<ParagraphContent> content)
        {var seek = 0;
            List<ItemDto> items = new List<ItemDto>();
            foreach (var p in content)
            {
                ParNode node = null;
                var ParagraphId = new ItemDto();
                
                foreach (var item in p.Content)
                {
                    
                    var i = new ItemDto();
                    node = new ParNode(item.Key,node);
                    i.TextContent = item.Value;
                    i.Number = node.Number;
                    if (node.Parent.CurrentParentNode < 0)
                    {
                        node.Parent.Seek = seek;
                        i.ParentId = null;
                        i.IsRoot = true;
                        ParagraphId = i;
                    }
                    else
                    {
                        i.ParagraphId = ParagraphId;
                        i.ParentId = items[node.Parent.CurrentParentNode+seek ];
                        i.IsRoot = false;
                    }
                    items.Add(i);
                }
                seek += p.Content.Count;
            }
            return items;
        }

        private string RemoveTag(string line)
        {
            return line.Remove(line.IndexOf("\\cell\r"));
        }
    }

    public class Paragraph
    {
        string pattern = @"\par\pard\plain\cf1\f1\fs21\qj\ri0";
        private KeyValuePair<int, string> _paragraph;
        public Paragraph(KeyValuePair<int,string> paragraph)
        {
            _paragraph = paragraph;
            IndexOfList = paragraph.Key;
            Tag = paragraph.Value.Remove(0, paragraph.Value.IndexOf("\\"));
            Value = paragraph.Value.Remove(paragraph.Value.IndexOf("\\"));
        }
        public int IndexOfList { get;private set; }
        public string Value { get; private set; }
        public string Tag { get; private set; }
    }

    public class ParagraphContent
    {
        private List<string> _lines=new List<string>();
        private int _indexEndPar;
        private Paragraph _paragraph;
        public List<KeyValuePair<string, string>> Content { get; private set; } = new List<KeyValuePair<string, string>>();
        public ParagraphContent(List<string> list,Paragraph paragraph,int indexEndPar)
        {
            _lines = list;
            _paragraph = paragraph;
            _indexEndPar = indexEndPar;
            ParContentInit();
        }
        public void ParContentInit()
        {
            for (var i = _paragraph.IndexOfList; i < _indexEndPar; i++)
            {
                var arr = new string[2];
                if (_lines[i].StartsWith(" "))
                    _lines[i]=_lines[i].Remove(0, 1);
                if (!_lines[i].StartsWith(" ") && _lines[i].Contains(" "))
                {

                    arr[0] = _lines[i].Split(' ')[0];
                    var firstChar = arr[0].ToCharArray()[0];
                    if (firstChar == 'I' || firstChar == 'V' || firstChar == 'X')
                    {
                        arr[1] = _lines[i].Remove(0, arr[0].Length + 1);
                        arr[1] = arr[1].Remove(arr[1].IndexOf("\\"));
                        Content.Add(new KeyValuePair<string, string>(arr[0], arr[1]));
                        continue;
                    }
                    if (!int.TryParse(firstChar.ToString(), out _))
                            continue;
                    arr[1] = _lines[i].Remove(0, arr[0].Length + 1);
                    arr[1] = arr[1].Remove(arr[1].IndexOf("\\"));
                    Content.Add(new KeyValuePair<string, string>(arr[0], arr[1]));
                }
                else
                    continue;
            }
        }
    }
}
