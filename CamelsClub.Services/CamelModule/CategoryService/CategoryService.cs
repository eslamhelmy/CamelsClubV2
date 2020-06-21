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
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unit;
        private readonly ICategoryRepository _repo;

        public CategoryService(IUnitOfWork unit, ICategoryRepository repo)
        {
            _unit = unit;
            _repo = repo;
        }
        public PagingViewModel<CategoryViewModel> Search(string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var query = _repo.GetAll().Where(post => !post.IsDeleted);



            int records = query.Count();
            if (records <= pageSize || pageIndex <= 0) pageIndex = 1;
            int pages = (int)Math.Ceiling((double)records / pageSize);
            int excludedRows = (pageIndex - 1) * pageSize;

            List<CategoryViewModel> result = new List<CategoryViewModel>();

            var posts = query.Select(obj => new CategoryViewModel
            {
                ID = obj.ID,
                NameArabic = obj.NameArabic ,
                NameEnglish = obj.NameEnglish
            }).OrderByPropertyName(orderBy, isAscending);

            result = posts.Skip(excludedRows).Take(pageSize).ToList();
            return new PagingViewModel<CategoryViewModel>() { PageIndex = pageIndex, PageSize = pageSize, Result = result, Records = records, Pages = pages };
        }


        public void Add(CategoryCreateViewModel viewModel)
        {

             _repo.Add(viewModel.ToModel());
            
        }

        public void Edit(CategoryCreateViewModel viewModel)
        {
            _repo.SaveIncluded(viewModel.ToModel(), "ActionID");
            
        }

        public CategoryViewModel GetByID(int id)
        {

            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var postUserAction = _repo.GetAll().Where(postAction => postAction.ID == id)
                .Select(obj => new CategoryViewModel
            {
                ID = obj.ID,
                NameArabic = obj.NameArabic,
                NameEnglish = obj.NameEnglish
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

