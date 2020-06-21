using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class CamelGroupCreateViewModel
    {
        public int ID { get; set; }
        public int CamelID { get; set; }
        public int GroupID { get; set; }
     
    }

    public static partial class CamelGroupExtensions
    {
        public static CamelGroup ToModel(this CamelGroupCreateViewModel viewModel)
        {
            return new CamelGroup
            {
                ID = viewModel.ID,
                CamelID = viewModel.CamelID ,
                GroupID = viewModel.GroupID
            };
        }
    }
}
