using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Hackaton.CrmDbModel.Model;
using Hackaton.CrmDbModel.ModelDto.AiWord;
using Microsoft.EntityFrameworkCore;
using Monica.Core.DataBaseUtils;
using MySql.Data.MySqlClient;
using System.Linq;


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

        public async Task<IEnumerable<PredicatDto>> GetPredicatDtos()
        {
            using (var connection = new MySqlConnection(_dataBaseMain.ConntectionString))
            {
                return await connection.QueryAsync<PredicatDto>(SqlQueryHelper.QueryGlagol);
            }
        }


        public async Task<IEnumerable<PredicatEntityDto>> GetPredicatEntities(string paramQuery)
        {
            try
            {
                CrmDbModel.Model.Ai.AiWord word = await _wordDbContext.AiWord.FirstOrDefaultAsync(f => f.NormalizeText == paramQuery);
                if (word == null)
                    throw new System.Exception("Не удалось получить слово");
                CrmDbModel.Model.Ai.AiSentence aiSentence = word.AiSentence;
                var aiGroupWordNowSentences = _wordDbContext.AiGroup.Where(f => f.AiSentence == aiSentence).
                    Where(f => f.Last - f.Start < 3).
                    Distinct();
                var aiGroupWordParentSentences = _wordDbContext.AiSentence.Where(f => f.DocumentItem.Parent == aiSentence.DocumentItem);
                foreach (var sentences in aiGroupWordParentSentences)
                {
                    aiGroupWordNowSentences.Union(_wordDbContext.AiGroup.Where(f => f.AiSentence == sentences).
                        Where(f => f.Last - f.Start < 3).
                        Distinct());
                }
                aiGroupWordNowSentences = aiGroupWordNowSentences.Where(f => f.AiDescription.Name == "генит_иг" ||
                                                                           f.AiDescription.Name == "однор_иг" ||
                                                                           f.AiDescription.Name == "прил_сущ");
                List<PredicatEntityDto> result = new List<PredicatEntityDto>();
                foreach(var item in aiGroupWordNowSentences)
                {
                    PredicatEntityDto predicatEntityDto = new PredicatEntityDto();
                    var listWord = _wordDbContext.AiGroupWord.Where(f => f.AiGroup == item);
                    foreach(var w in listWord)
                    {
                        predicatEntityDto.Title += w + " ";
                    }
                    result.Add(predicatEntityDto);
                }
                return result;
            }
            catch
            {
                throw new System.Exception("Не удалось получить сущности");
            }





            return null;
        }



    }


    public class SqlQueryHelper
    {
        public const string QueryGlagol = @"SELECT qq.NormalizeText , MAX(qq.Descript) as Descript, MAX(qq.Id) FROM 
                                          (SELECT w.NormalizeText, MAX(a2.Name) AS Descript, MAX(a1.Id) AS Id FROM aigroup a   
                                          JOIN aigroupword a1 ON a.Id = a1.AiGroupId
                                          JOIN aisentence s ON s.id = a.AiSentenceId
                                          JOIN aiword w ON w.Id = a1.AiWordId
                                          JOIN aidescription a2 ON a.AiDescriptionId = a2.Id
                                          JOIN (SELECT ww.Grm FROM aiword ww 
                                          WHERE ww.Grm LIKE 'С %' OR ww.Grm LIKE 'Г%' OR ww.Grm LIKE 'Н%' OR ww.Grm LIKE 'ПРИЧ%' OR ww.Grm LIKE 'КР_ПРИЛ%' GROUP BY ww.Grm) grm ON grm.Grm = w.Grm
                                          WHERE a2.Name = 'sp' GROUP BY w.NormalizeText
                                        UNION ALL
                                        SELECT w.NormalizeText, MAX(a2.Name) AS Descript, MAX(a1.Id) AS Id FROM aigroup a   
                                          JOIN aigroupword a1 ON a.Id = a1.AiGroupId
                                          JOIN aisentence s ON s.id = a.AiSentenceId
                                          JOIN aiword w ON w.Id = a1.AiWordId
                                          JOIN aidescription a2 ON a.AiDescriptionId = a2.Id
                                          JOIN (SELECT ww.Grm FROM aiword ww 
                                          WHERE ww.Grm LIKE 'Г%' OR ww.Grm LIKE 'КР_ПРИЛ%' GROUP BY ww.Grm) grm ON grm.Grm = w.Grm
                                          WHERE a2.Name IN ('гл_личн', 'кр_прил')  GROUP BY w.NormalizeText  ORDER BY 3) AS qq  GROUP BY 1";


        public const string Query = @"MaxSELECT w.NormalizeText, a.Last, a.Start FROM aigroup a
                                            JOIN aigroupword a1 ON a.Id = a1.AiGroupId
                                            JOIN aisentence s ON s.id = a.AiSentenceId
                                            JOIN aiword w ON w.Id = a1.AiWordId
                                            JOIN (SELECT IFNULL(sCh.Id, ss.Id) AS IdCh, ss.Id FROM aisentence ss
                                            JOIN aigroup gg ON gg.AiSentenceId = ss.Id
                                            JOIN aigroupword gw ON gw.AiGroupId = gg.Id
                                            JOIN documentitem d ON d.Id = ss.DocumentItemId
                                            JOIN aiword ww ON ww.Id = gw.AiWordId
                                            LEFT JOIN documentitem dd ON dd.ParentId = d.Id
                                            LEFT JOIN aisentence sCh ON sCh.DocumentItemId = dd.Id
                                            LEFT JOIN aigroup ggCh ON ggCh.AiSentenceId = sCh.Id
                                            LEFT JOIN aigroupword gwCh ON gwCh.AiGroupId = ggCh.Id
                                            LEFT JOIN aiword wwCh ON wwCh.Id = gwCh.AiWordId
                                            WHERE ww.NormalizeText = @paramQuery GROUP BY 1,2) AS ss ON s.Id in (ss.IdCh)
                                            JOIN aidescription a2 ON a.AiDescriptionId = a2.Id AND a2.Name IN ('генит_иг', 'однор_иг','прил_сущ')
                                            JOIN (SELECT ww.Grm FROM aiword ww
                                            WHERE ww.Grm NOT LIKE 'СОЮЗ%' and ww.Grm NOT LIKE 'ПРЕДЛ%' and ww.Grm NOT LIKE 'МС %' GROUP BY ww.Grm) grm ON grm.Grm = w.Grm GROUP BY w.id ORDER BY a.Id, w.id;";


    }
}
