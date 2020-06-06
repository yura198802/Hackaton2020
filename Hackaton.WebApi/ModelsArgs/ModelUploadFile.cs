using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Hackaton.WebApi.ModelsArgs
{
    public class ModelUploadFile
    {
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public ModelUploadFile() { }

        /// <summary>
        /// Идя файла
        /// </summary>
        [Required(ErrorMessage = "FileName не может быть пустым")]
        public string FileName { get; set; }

        /// <summary>
        /// Содержимое документа
        /// </summary>
        [Required(ErrorMessage = "Files не может быть пустым")]
        public IFormFile Files { get; set; }


    }
}
