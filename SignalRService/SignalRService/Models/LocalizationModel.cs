using SignalRService.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SignalRService.Models
{
    [Serializable]
    public class LocalizationModel : BaseModel
    {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int ID { get; set; }
            [Index("IX_Localization_Culture_Key", 1, IsUnique = true)]
            [StringLength(10)]
            public string Culture { get; set; }
            [Index("IX_Localization_Culture_Key", 2, IsUnique = true)]
            [StringLength(40)]
            public string Key { get; set; }
            public string Value { get; set; }
            public DateTime LastModDate { get; set; }
            public string ModUser { get; set; }
            public bool WasHit { get; set; }
            public EnumTranslationStatus TranslationStatus { get; set; }
    }
}