using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Models
{
    public class PagePermission : BaseModel
    {
        public int PageID { get; set; }
        public virtual Page Page { get; set; }
        public int PermissionID { get; set; }
        public virtual Permission Permission { get; set; }
    }
}
