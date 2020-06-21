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
    public class PageService : IPageService
    {
        private readonly IUnitOfWork _unit;
        private readonly IPageRepository _repo;
      
        public PageService(IUnitOfWork unit,
                            IPageRepository repo)
        {
            _unit = unit;
            _repo = repo;
        }
        public List<PageViewModel> GetList()
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var pages = _repo
                        .GetAll()
                        .Where(post => !post.IsDeleted)
                        .Select(x => new PageViewModel
                        {
                            ID = x.ID,
                            NameArabic = x.NameArabic,
                            NameEnglish = x.NameEnglish,
                            Permissions = x.PagePermissions.Select(p => new PermissionViewModel
                            { 
                                ID = p.ID,
                                NameArabic = p.Permission.NameArabic,
                                NameEnglish = p.Permission.NameEnglish
                            }).ToList()
                        }).ToList();

            return pages;
        }

      }
}

