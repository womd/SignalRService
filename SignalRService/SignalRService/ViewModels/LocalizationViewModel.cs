using SignalRService.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SignalRService.ViewModels
{
    public class LocalizationViewModel
    {
        public int Id { get; set; }
        [StringLength(10)]
        public string Culture { get; set; }
        [StringLength(40)]
        public string Key { get; set; }
        public string Value { get; set; }
        public DateTime LastModDate { get; set; }
        public string ModUser { get; set; }
        public bool WasHit { get; set; }
        public EnumTranslationStatus TranslationStatus { get; set; }
    }
}