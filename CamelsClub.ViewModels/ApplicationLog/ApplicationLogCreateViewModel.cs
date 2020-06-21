using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class ApplicationLogCreateViewModel
    {
        public int ID { get; set; }
        public int LogTypeID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Data { get; set; }
        public string IP { get; set; }
        public string URL { get; set; }
        public int IsDeleted { get; set; } = 0;

    }
    public static class ApplicationLogExtension
    {
        public static ApplicationLog ToModel(this ApplicationLogCreateViewModel viewModel)
        {
            return new ApplicationLog()
            {
                ID = viewModel.ID,
                Data = viewModel.Data,
                Description = viewModel.Description,
                IP = viewModel.IP,
                LogTypeID = viewModel.LogTypeID,
                Title = viewModel.Title,
                URL = viewModel.URL

            };
        }
        public static ApplicationLogCreateViewModel ToViewModel(this ApplicationLog model)
        {
            return new ApplicationLogCreateViewModel()
            {
                ID = model.ID,
                Data = model.Data,
                Description = model.Description,
                IP = model.IP,
                LogTypeID = model.LogTypeID,
                Title = model.Title,
                URL = model.URL
            };
        }
    }
}
