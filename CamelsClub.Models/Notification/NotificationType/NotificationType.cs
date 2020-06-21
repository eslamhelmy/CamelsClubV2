
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
   
    public class NotificationType : BaseModel
    {
        public string NameArabic { get; set; }
        public string NameEnglish { get; set; }

      
    }
}
