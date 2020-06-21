using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class CamelCreateViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string MotherName { get; set; }
        public string FatherName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Location { get; set; }
        public string Details { get; set; }
        [IgnoreDataMember]
        public int UserID { get; set; }

        public int CategoryID { get; set; }
        //it inserts in GenderConfigDetailID but i wrote it GenderID cause of simplicity for Client Side
        public int GenderID { get; set; }
        public List<CamelDocumentCreateViewModel> Files { get; set; }
    }

    public static partial class CamelExtensions
    {
        public static Camel ToModel(this CamelCreateViewModel viewModel)
        {
            return new Camel
            {
                ID = viewModel.ID,
                Name = viewModel.Name,
                MotherName = viewModel.MotherName,
                FatherName = viewModel.FatherName,
                BirthDate = viewModel.BirthDate,
                CategoryID = viewModel.CategoryID ,
                Details = viewModel.Details,
                Location = viewModel.Location ,
                UserID = viewModel.UserID ,
                GenderConfigDetailID = viewModel.GenderID
                
            };
        }
    }
}
