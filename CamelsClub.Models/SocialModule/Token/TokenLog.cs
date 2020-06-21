using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class TokenLog : BaseModel
    {
        [ForeignKey("Token")]
        public int TokenID { get; set; }
        public virtual Token Token { get; set; }

        [Required]
        [StringLength(250)]
        public string IP { get; set; }

        [Required]
        [StringLength(250)]
        public string URL { get; set; }

        public bool IsAuthorized { get; set; }
    }
}
