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
    public class IssueReportService : IIssueReportService
    {
        private readonly IUnitOfWork _unit;
        private readonly IIssueReportRepository _repo;

        public IssueReportService(IUnitOfWork unit, IIssueReportRepository repo)
        {
            _unit = unit;
            _repo = repo;
        }
        public PagingViewModel<IssueReportViewModel> Search(string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var query = _repo.GetAll().Where(post => !post.IsDeleted);



            int records = query.Count();
            if (records <= pageSize || pageIndex <= 0) pageIndex = 1;
            int pages = (int)Math.Ceiling((double)records / pageSize);
            int excludedRows = (pageIndex - 1) * pageSize;

            List<IssueReportViewModel> result = new List<IssueReportViewModel>();

            var posts = query.Select(obj => new IssueReportViewModel
            {
                ID = obj.ID,
                ReportReasonTextArabic = obj.ReportReason.TextArabic ,
                ReportReasonTextEnglish = obj.ReportReason.TextEnglish,
                UserName = obj.User.UserName,
                PostID = obj.PostID
            }).OrderByPropertyName(orderBy, isAscending);

            result = posts.Skip(excludedRows).Take(pageSize).ToList();
            return new PagingViewModel<IssueReportViewModel>() { PageIndex = pageIndex, PageSize = pageSize, Result = result, Records = records, Pages = pages };
        }


        public void Add(IssueReportCreateViewModel viewModel)
        {

             _repo.Add(viewModel.ToModel());
            
        }
        public IssueReportViewModel GetByID(int id)
        {

            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var postUserAction = _repo.GetAll().Where(postAction => postAction.ID == id)
                .Select(obj => new IssueReportViewModel
            {

                    ID = obj.ID,
                    ReportReasonTextArabic = obj.ReportReason.TextArabic,
                    ReportReasonTextEnglish = obj.ReportReason.TextEnglish,
                    UserName = obj.User.UserName,
                    PostID = obj.PostID
                }).FirstOrDefault();
            
            return postUserAction;

        }


        public bool IsExists(int postID , int userID)
        {
            return _repo.GetAll().Where(x => x.PostID == postID && x.UserID == userID).Any();
        }
        public void Delete(int id)
        {
            _repo.Remove(id);
        }

        public void Edit(IssueReportCreateViewModel viewModel)
        {
            _repo.Edit(viewModel.ToModel());
        }
    }
}

