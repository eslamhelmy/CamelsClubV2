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
    public class ReportReasonService : IReportReasonService
    {
        private readonly IUnitOfWork _unit;
        private readonly IReportReasonRepository _repo;

        public ReportReasonService(IUnitOfWork unit, IReportReasonRepository repo)
        {
            _unit = unit;
            _repo = repo;
        }
        public PagingViewModel<ReportReasonViewModel> Search(string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var query = _repo.GetAll().Where(post => !post.IsDeleted);



            int records = query.Count();
            if (records <= pageSize || pageIndex <= 0) pageIndex = 1;
            int pages = (int)Math.Ceiling((double)records / pageSize);
            int excludedRows = (pageIndex - 1) * pageSize;

            List<ReportReasonViewModel> result = new List<ReportReasonViewModel>();

            var posts = query.Select(obj => new ReportReasonViewModel
            {
                ID = obj.ID,
                TextArabic = obj.TextArabic ,
                TextEnglish = obj.TextEnglish
            }).OrderByPropertyName(orderBy, isAscending);

            result = posts.Skip(excludedRows).Take(pageSize).ToList();
            return new PagingViewModel<ReportReasonViewModel>() { PageIndex = pageIndex, PageSize = pageSize, Result = result, Records = records, Pages = pages };
        }


        public void Add(ReportReasonCreateViewModel viewModel)
        {

             _repo.Add(viewModel.ToModel());
            
        }

        public void Edit(ReportReasonCreateViewModel viewModel)
        {
            _repo.Edit(viewModel.ToModel());   
        }

        public ReportReasonViewModel GetByID(int id)
        {

            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var postUserAction = _repo.GetAll().Where(postAction => postAction.ID == id)
                .Select(obj => new ReportReasonViewModel
            {
                ID = obj.ID,
                TextArabic = obj.TextArabic,
                TextEnglish = obj.TextEnglish
            }).FirstOrDefault();
            
            return postUserAction;

        }


        public bool IsExists(int id)
        {
            return _repo.GetAll().Where(x => x.ID == id).Any();
        }
        public void Delete(int id)
        {
            _repo.Remove(id);
        }

      }
}

