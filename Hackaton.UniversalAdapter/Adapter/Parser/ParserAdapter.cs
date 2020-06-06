using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Hackaton.CrmDbModel.ModelDto.Parser;

namespace Hackaton.UniversalAdapter.Adapter.Parser
{

    public class ParserAdapterAdapter : IParserAdapter
    {
        /// <summary>
        /// Строки, содержимое файла
        /// </summary>
        private string _fileString = string.Empty;

        /// <summary>
        /// Список Итемов
        /// </summary>
        List<ItemDto> items = new List<ItemDto>();
        
        /// <summary>
        /// список строк в файле(разделённых переносом строки)
        /// </summary>
        List<string> _lines = new List<string>();

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
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _fileString = Encoding.GetEncoding("windows-1251").GetString(fileContent);
            foreach (var line in _fileString.Split('\n'))
            {
                _lines.Add(line);
            } 
            return Task.FromResult(CreateRtf());
        }

        /// <summary>
        /// Создание объекта документа
        /// </summary>
        /// <returns></returns>
        private FileRtf CreateRtf()
        {
            var result = new FileRtf();
            InitDictionaries(_lines);
            result.Caption = GetCellValue(1);
            result.VidDoc = GetCellValue(3);
            result.Category = GetCellValue(8);
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
                if (i < _par.Count-1)
                    contents.Add(new ParagraphContent(_lines, par, _par[++i].IndexOfList));
                else
                    contents.Add(new ParagraphContent(_lines, par, _lines.Count));
            }
            return contents;
        }
        private List<ItemDto> CreateItem(List<ParagraphContent> content)
        {
            List<ItemDto> items = new List<ItemDto>();
            foreach (var p in content)
            {
                var firstItem = new ItemDto();
                var parent = new ItemDto();
                var indexIter = 0;
                var counter = 0;
                foreach (var item in p.Content)
                {
                    var i = new ItemDto();
                    i.Number = item.Key;
                    i.TextContent = item.Value;
                    if (string.IsNullOrWhiteSpace(i.TextContent))
                        continue;
                    //параграф
                    if (indexIter == 0/*item.Key == p.Content[0].Key && p.Content[0].Value == item.Value*/)
                    {
                        indexIter++;
                        i.ParentId = null;
                        i.IsRoot = true;
                        firstItem = i;
                        items.Add(i);
                        continue;
                    }
                    if (!int.TryParse(item.Key.Replace(".", ""), out _))
                        continue;
                    if (item.Key.Contains("."))
                    {
                        //item.Key.Remove(item.Key.LastIndexOf("."));
                        var group = item.Key.Split('.');
                        //первый после параграфа
                        if (counter == 0)
                        {
                            counter = group.Length;
                            i.ParentId = firstItem;
                            parent = i;
                            indexIter++;
                            items.Add(i);
                            continue;
                        }
                        if (group.Length > counter)
                        {
                            i.ParentId = parent;
                            counter = group.Length;
                            indexIter++;
                            continue;
                        }
                        if (group.Length == counter && group.Length == 2)
                        {
                            i.ParentId = firstItem;
                            counter = group.Length;
                            parent = i;
                            items.Add(i);
                            indexIter++;
                            continue;
                        }
                        if (group.Length == counter && group.Length > 2)
                        {
                            i.ParentId = parent;
                            counter = group.Length;
                            //parent = i;
                            items.Add(i);
                            indexIter++;
                            continue;
                        }
                        if (group.Length < counter)
                        {
                            i.ParentId = firstItem;
                            counter = group.Length;
                            parent = i;
                            items.Add(i);
                            indexIter++;
                            continue;
                        }

                        indexIter++;
                        continue;
                    }
                    i.ParentId = firstItem;
                    items.Add(i);
                    indexIter++;
                    continue;
                }
                indexIter = 0;
                counter = 0;
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
                if (_lines[i].StartsWith("\\"))
                    continue;
                if (_lines[i].IndexOf(".") == 0)
                    continue;
                var arr = new string[2];
                if (_lines[i].StartsWith(' '))
                {
                    _lines[i] = _lines[i].Replace(" ", "").Replace("\\cell\r", "");
                    if(_lines[i].Contains(" "))
                        arr[0] = _lines[i].Split(' ')[0];
                    arr[0] = _lines[i];
                    arr[1] = "";
                }

                else
                {
                    if (_lines[i].Contains(" "))
                    {
                        arr[0] = _lines[i].Split(' ')[0];
                        arr[1] = _lines[i].Remove(0, arr[0].Length + 1);
                        arr[1] = arr[1].Remove(arr[1].IndexOf("\\"));
                    }
                    else
                    {
                        arr[0] = _lines[i];
                        arr[1] = "";
                    }
                }
                Content.Add(new KeyValuePair<string, string>(arr[0],arr[1]));
            }
        }
    }
}
