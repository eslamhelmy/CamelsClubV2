using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class CamelGroupViewModel
    {
      
        public int ID { get; set; }
        public int CamelID { get; set; }
        public string CamelName { get; set; }
        public List<CamelDocumentViewModel> camelImages { get; set; }
        public int GroupID { get; set; }
    }
    public  static class CamelGroupExtension
    {

        public static CamelGroupViewModel ToViewModel(this CamelGroup model)
        {
            return new CamelGroupViewModel
            {
                ID = model.ID,
                CamelID = model.CamelID,
                GroupID = model.GroupID
            };
        }
    }
}

