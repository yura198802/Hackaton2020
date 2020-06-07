using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hackaton.CrmDbModel.Model;
using Hackaton.CrmDbModel.Model.LoadDocument;
using Hackaton.CrmDbModel.ModelDto.AiWord;
using Monica.Core.DataBaseUtils;
using MySql.Data.MySqlClient;

namespace Hackaton.UniversalAdapter.Adapter.AiWord
{
    public class AiWordAdapter : IAiWordAdapter
    {
        private IDataBaseMain _dataBaseMain;
        private WordDbContext _wordDbContext;

        public AiWordAdapter(IDataBaseMain dataBaseMain, WordDbContext wordDbContext)
        {
            _dataBaseMain = dataBaseMain;
            _wordDbContext = wordDbContext;
        }

        public async Task<IEnumerable<InfoDocument>> GetInfoDocument(int userId)
        {
            return (await GetDocumentCategoryDol(userId)).Union(await GetDocumentCategoryOtv(userId));
        }

        public List<InfoDocument> GetDocumentInfo()
        {
            try
            {

                var result = new List<InfoDocument>();
                var loaders = _wordDbContext.DocumentItem;
                foreach (var loader in loaders.ToList())
                {
                    if (loader.TextContent != "Общие положения")
                        continue;
                    result.Add(CreateInfoDocument(loader));
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Не удалось распрасить инструкцию, возможно этот формат не описан.");
            }
        }
        public InfoDocument CreateInfoDocument(DocumentItem rootDocItem)
        {
            InfoDocument infoDocument = new InfoDocument();
            infoDocument.Title = rootDocItem.TextContent;


            InfoCategoty infoCategoty1 = CreateCategory(rootDocItem, "Категория", "1");
            InfoCategoty infoCategoty2 = CreateCategory(rootDocItem, "Образование и стаж", "2");
            InfoCategoty infoCategoty3 = CreateCategory(rootDocItem, "Назначение на должность", "3");
            InfoCategoty infoCategoty4 = CreateCategory(rootDocItem, "Требования к знаниям", "4");
            InfoCategoty infoCategoty5 = CreateCategory(rootDocItem, "Подчинение", "5");
            InfoCategoty infoCategoty6 = CreateCategory(rootDocItem, "Правила замещения", "6");

            infoDocument.Items = new List<InfoCategoty>();
            infoDocument.Items.Add(infoCategoty1);
            infoDocument.Items.Add(infoCategoty2);
            infoDocument.Items.Add(infoCategoty3);
            infoDocument.Items.Add(infoCategoty4);
            infoDocument.Items.Add(infoCategoty5);
            infoDocument.Items.Add(infoCategoty6);

            return infoDocument;

        }
        protected InfoCategoty CreateCategory(DocumentItem rootDocItem, string title, string number)
        {
            try
            {
                InfoCategoty infoCategoty = new InfoCategoty();
                infoCategoty.Title = title; //"Категория";
                infoCategoty.Items = new List<InfoItem>();
                var potentialRoot = _wordDbContext.DocumentItem.FirstOrDefault(f => f.Number == number && f.Parent == rootDocItem);
                infoCategoty.Items.Add(CreateItem(rootDocItem, number));
                var innersCategory = _wordDbContext.DocumentItem.Where(f => f.Parent == potentialRoot);
                if (innersCategory != null && innersCategory.Count() > 0)
                {
                    int index = 0;
                    foreach (var item in innersCategory)
                    {
                        infoCategoty.Items.Add(CreateItem(potentialRoot, number + "." + (++index).ToString()));
                    }
                }
                return infoCategoty;
            }
            catch { return new InfoCategoty() { Items = new List<InfoItem>(), Title = title }; }
        }

        protected InfoItem CreateItem(DocumentItem rootDocItem, string number)
        {
            InfoItem infoItem = new InfoItem();
            if (_wordDbContext.DocumentItem.FirstOrDefault(f => f.Number == number && f.Parent == rootDocItem) == null)
            {
                infoItem.Title = "Тут должно быть что-то, но в инструкции ничего нет";
                infoItem.ProfName = new List<string>();
                if (_wordDbContext.DocumentLoader.FirstOrDefault(f => f.Id == rootDocItem.DocumentLoaderId) == null)
                {
                    infoItem.ProfName.Add("Не определилось должность");
                }
                else
                    infoItem.ProfName.Add(_wordDbContext.DocumentLoader.FirstOrDefault(f => f.Id == rootDocItem.DocumentLoaderId).ProfName);
            }
            else
            {
                infoItem.Title = _wordDbContext.DocumentItem.FirstOrDefault(f => f.Number == number && f.Parent == rootDocItem).TextContent;
                infoItem.ProfName = new List<string>();

                if (_wordDbContext.DocumentLoader.FirstOrDefault(f => f.Id == rootDocItem.DocumentLoaderId) == null)
                {
                    infoItem.ProfName.Add("Не определилось должность");
                }
                else
                    infoItem.ProfName.Add(_wordDbContext.DocumentLoader.FirstOrDefault(f => f.Id == rootDocItem.DocumentLoaderId).ProfName);
            }
            return infoItem;
        }

        private async Task<IEnumerable<InfoDocument>> GetDocumentCategoryDol(int userId)
        {
            using (var connection = new MySqlConnection(_dataBaseMain.ConntectionString))
            {
                var docs = new List<InfoDocument>();
                var queryRoot = await connection.QueryAsync<InfoRoot>(SqlQueryHelper.QueryGlagol, new { userId });
                foreach (var item in queryRoot.GroupBy(g => g.Razdel))
                {
                    var doc = new InfoDocument();
                    doc.Title = item.Key;
                    doc.Items = new List<InfoCategoty>();
                    foreach (var root in item.GroupBy(g => g.Category))
                    {
                        var itemCategory = new InfoCategoty();
                        itemCategory.Title = root.Key;
                        itemCategory.Items = new List<InfoItem>();
                        foreach (var infoRoot in root.GroupBy(g => g.Text))
                        {
                            var itemLine = new InfoItem();
                            itemLine.Title = FirstLetterToUpper(infoRoot.Key.Trim(',', ' '));
                            itemLine.ProfName = infoRoot.Select(s => s.ProfName).ToList();
                            itemCategory.Items.Add(itemLine);
                        }
                        doc.Items.Add(itemCategory);
                    }
                    docs.Add(doc);
                }

                return docs;
            }
        }

        public string FirstLetterToUpper(string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }

        private async Task<IEnumerable<InfoDocument>> GetDocumentCategoryOtv(int userId)
        {
            using (var connection = new MySqlConnection(_dataBaseMain.ConntectionString))
            {
                var docs = new List<InfoDocument>();
                var queryRoot = await connection.QueryAsync<InfoRoot>(SqlQueryHelper.Query, new { userId });
                foreach (var item in queryRoot.GroupBy(g => g.Razdel))
                {
                    var doc = new InfoDocument();
                    doc.Title = item.Key;
                    doc.Items = new List<InfoCategoty>();
                    foreach (var root in item.GroupBy(g => g.Text.ToLower().Contains("уголов") ? "Уголовня ответственность" : "Иная ответственность"))
                    {
                        var itemCategory = new InfoCategoty();
                        itemCategory.Title = root.Key;
                        itemCategory.Items = new List<InfoItem>();
                        foreach (var infoRoot in root.GroupBy(g => g.Text))
                        {
                            var itemLine = new InfoItem();
                            itemLine.Title = infoRoot.Key;
                            itemLine.ProfName = infoRoot.Select(s => s.ProfName).ToList();
                            itemCategory.Items.Add(itemLine);
                        }
                        doc.Items.Add(itemCategory);
                    }
                    docs.Add(doc);
                }

                return docs;
            }
        }



        class InfoRoot
        {
            public string Category { get; set; }
            public string Razdel { get; set; }
            public string Text { get; set; }
            public string ProfName { get; set; }
        }

    }




    public class SqlQueryHelper
    {
        public const string QueryGlagol = @"SELECT w.NormalizeText AS Category, item.TextContent AS Razdel, s.Text, doc.ProfName FROM aigroup a   
  JOIN aigroupword a1 ON a.Id = a1.AiGroupId
  JOIN aisentence s ON s.id = a.AiSentenceId
  JOIN aiword w ON w.Id = a1.AiWordId
  JOIN aidescription a2 ON a.AiDescriptionId = a2.Id
  JOIN (SELECT u.DocumentLoaderId, ddd.ProfName FROM userdocument u 
                JOIN documentloader ddd ON ddd.Id = u.DocumentLoaderId
                WHERE u.UserId = @userId GROUP BY 1) AS doc ON doc.DocumentLoaderId = s.DocumentLoaderId
  JOIN (SELECT dChild.Id, d.TextContent FROM documentitem d 
          JOIN documentitem dChild ON dChild.ParagraphId = d.Id WHERE d.IsRootItem AND TRIM(d.TextContent) IN ('Должностные обязанности','Права')) AS item ON item.Id = s.DocumentItemId
  JOIN (SELECT ww.Grm FROM aiword ww 
  WHERE ww.Grm LIKE 'Г%' OR ww.Grm LIKE 'КР_ПРИЛ%' OR ww.Grm LIKE 'С%' GROUP BY ww.Grm) grm ON grm.Grm = w.Grm
  WHERE a2.Name IN ('гл_личн', 'кр_прил', 'инф' )  GROUP BY s.Id,doc.ProfName  ORDER BY w.NormalizeText;";


        public const string Query = @"SELECT  item.TextContent AS Razdel, s.Text, doc.ProfName FROM aisentence s
  JOIN (SELECT u.DocumentLoaderId, ddd.ProfName FROM userdocument u 
                JOIN documentloader ddd ON ddd.Id = u.DocumentLoaderId
                WHERE u.UserId = @userId GROUP BY 1) AS doc ON doc.DocumentLoaderId = s.DocumentLoaderId
  JOIN (SELECT dChild.Id, d.TextContent FROM documentitem d 
          JOIN documentitem dChild ON dChild.ParagraphId = d.Id WHERE d.IsRootItem AND TRIM(d.TextContent) IN ('Ответственность')) AS item ON item.Id = s.DocumentItemId
  GROUP BY s.Id,doc.ProfName ";


    }
}
