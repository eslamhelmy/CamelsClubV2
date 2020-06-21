using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class PagingViewModel<T>
    {
        public int PageSize { set; get; }
        public int PageIndex { set; get; }
        public int Records { set; get; }
        public int Pages { set; get; }
        public IEnumerable<T> Result { set; get; }
    }
    public class NotificationPagingViewModel<T> : PagingViewModel<T>
    {
        public int UnSeenMessagesCount { get; set; }
    }
}
