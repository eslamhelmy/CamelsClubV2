using CamelsClub.Data.Extentions;
using CamelsClub.Data.UnitOfWork;
using CamelsClub.Models;
using CamelsClub.Repositories;
using CamelsClub.ViewModels;
using CamelsClub.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CamelsClub.Services
{
    public class ConditionsAndTermsService : IConditionsAndTermsService
    {
        private readonly IUnitOfWork _unit;
        private readonly IConditionsAndTermsRepository _repo;

        public ConditionsAndTermsService(IUnitOfWork unit, IConditionsAndTermsRepository repo)
        {
            _unit = unit;
            _repo = repo;
        }
        public ConditionsAndTermsViewModel Get(Languages language = Languages.Arabic)
        {

            var data = _repo.GetAll().Where(x => !x.IsDeleted)
                .Select(x => new ConditionsAndTermsViewModel
            {

                    Text = language == Languages.Arabic ?  x.TextArabic : x.TextEnglish 
                }).FirstOrDefault();
            
            return data;

        }


    }
}

