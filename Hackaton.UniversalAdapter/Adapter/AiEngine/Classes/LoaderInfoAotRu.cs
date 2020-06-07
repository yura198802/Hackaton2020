using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hackaton.UniversalAdapter.Adapter.AiEngine.Interface;
using Hackaton.UniversalAdapter.Adapter.AiEngine.Model;
using RestSharp;

namespace Hackaton.UniversalAdapter.Adapter.AiEngine.Classes
{
    /// <summary>
    /// Загрузчик данных для морфологического разбора
    /// </summary>
    public class LoaderInfoAotRu : ILoaderInfoAotRu
    {
        private string _host = "http://185.237.97.32:8080";
        
        /// <summary>
        /// Загрузить данные с сервиса Aot.ru
        /// </summary>
        /// <param name="content">Текст</param>
        /// <returns>Модель</returns>
        public Task<List<AotModel>> LoaderAotModel(string content)
        {
            try
            {
                RestClient client = new RestClient(_host);
                var request = new RestRequest(client.BaseUrl, Method.GET, DataFormat.Json);
                CreateParams(request, content);
                var response = client.Get(request);
                var models = AotModel.FromJson(response.Content);
                return Task.FromResult(models[0].ToList());
            }
            catch (Exception e)
            {
                return null;
            }
            
        }


        private void CreateParams(RestRequest request, string text)
        {
            request.AddParameter("dummy", "1");
            request.AddParameter("action", TypeLoader.syntax.ToString());
            request.AddParameter("langua", "Russian");
            request.AddParameter("query", text);
        }
    }

    public enum TypeLoader { syntax }
}
