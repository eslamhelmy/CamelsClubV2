using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class NotificationReceiverTokensViewModels
    {
        public NotificationReceiverTokensViewModels()
        {
            DevicesIDs = new List<string>();
            //ConnectionIDs = new List<string>();
        }
        public IEnumerable<string> DevicesIDs { get; set; }
        //public IEnumerable<string> ConnectionIDs { get; set; }
    }
}
