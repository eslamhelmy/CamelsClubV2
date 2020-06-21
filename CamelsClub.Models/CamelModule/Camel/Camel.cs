using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class Camel : BaseModel
    {
        public string Name { get; set; }
        public string MotherName { get; set; }
        public string FatherName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Location { get; set; }
        public string Details { get; set; }
        public int UserID { get; set; }
        public virtual User User { get; set; }
        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }
        public int GenderConfigDetailID { get; set; }
        public string Code { get; set; }
        public virtual GenderConfigDetail GenderConfigDetail { get; set; }

        public virtual ICollection<CamelDocument> CamelDocuments { get; set; }
        public virtual ICollection<CamelCompetition> CamelCompetitions { get; set; }
    
        //circle
        //    public virtual ICollection<CamelGroup> CamelGroups { get; set; }
    }
}
