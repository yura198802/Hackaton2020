﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Hackaton.CrmDbModel.ModelCrm.Core
{
    /// <summary>
    /// Базовая модель для EnitityCore
    /// </summary>
    public class BaseModel
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public BaseModel()
        {
            CreateDate = DateTime.Now;
            IsDeleted = false;
        }

        /// <summary>
        /// Первичный ключ модели
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Дата добавления записи
        /// </summary>
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// Признак удаленной записи
        /// </summary>
        public bool? IsDeleted { get; set; }
    }
}
