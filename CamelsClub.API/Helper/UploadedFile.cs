using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CamelsClub.API.Helpers
{
    public class UploadedFile
    {
        public string OriginalFileName { get; set; }
        public string NewFileName { get; set; }
        public string Path { get; set; }
        public string Extension { get; set; }
    }
}