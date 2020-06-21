using CamelsClub.Data.Extentions;
using CamelsClub.Data.Helpers;
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
    public class CamelService : ICamelService
    {
        private readonly IUnitOfWork _unit;
        private readonly ICamelRepository _repo;
        private readonly ICamelDocumentRepository _camelDocumentRepository;
        private readonly IGenderConfigRepository _genderConfigRepository;

        public CamelService(IUnitOfWork unit,
                            ICamelRepository repo ,
                            ICamelDocumentRepository camelDocumentRepository,
                            IGenderConfigRepository genderConfigRepository)
        {
            _unit = unit;
            _repo = repo;
            _camelDocumentRepository = camelDocumentRepository;
            _genderConfigRepository = genderConfigRepository;
        }
        public PagingViewModel<CamelViewModel> Search(int userID=0,string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var query = _repo.GetAll().Where(post => !post.IsDeleted)
                .Where(x=>userID==0 || x.UserID == userID);



            int records = query.Count();
            if (records <= pageSize || pageIndex <= 0) pageIndex = 1;
            int pages = (int)Math.Ceiling((double)records / pageSize);
            int excludedRows = (pageIndex - 1) * pageSize;

            List<CamelViewModel> result = new List<CamelViewModel>();

            var posts = query.Select(obj => new CamelViewModel
            {
                ID = obj.ID,
                BirthDate = obj.BirthDate,
                CategoryArabicName = obj.Category.NameArabic,
                CategoryEnglishName = obj.Category.NameEnglish,
                CategoryID = obj.CategoryID,
                UserName = obj.User.UserName,
                DisplayName = obj.User.DisplayName,
                UserImage = obj.User.UserProfile.MainImage != null ? protocol + "://" + hostName + "/uploads/User-Document/" + obj.User.UserProfile.MainImage : "",
                Details = obj.Details,
                FatherName = obj.FatherName,
                Location = obj.Location,
                MotherName = obj.MotherName,
                Name = obj.Name,
                GenderID = obj.GenderConfigDetailID,
                GenderName = obj.GenderConfigDetail.NameArabic,
                camelDocuments = obj.CamelDocuments.Where(doc => !doc.IsDeleted).Select(doc => new CamelDocumentViewModel
                {
                    FilePath = protocol + "://" + hostName + "/uploads/Camel-Document/" + doc.FileName,
                    UploadedDate = doc.CreatedDate,
                    FileType = doc.Type
                }).ToList()

            }).OrderByPropertyName(orderBy, isAscending);

            result = posts.Skip(excludedRows).Take(pageSize).ToList();
            return new PagingViewModel<CamelViewModel>() { PageIndex = pageIndex, PageSize = pageSize, Result = result, Records = records, Pages = pages };
        }


        public void Add(CamelCreateViewModel viewModel)
        {
            var verificationCode = SecurityHelper.GetRandomNumber().ToString();

            while (_repo.GetAll()
                .Any(x => x.Code == verificationCode && !x.IsDeleted))
            {
                verificationCode = SecurityHelper.GetRandomNumber().ToString();
            }

            var insertedObj = _repo.Add(viewModel.ToModel());
            insertedObj.Code = verificationCode;
            if (viewModel.Files != null && viewModel.Files.Count > 0)
            {
                foreach (var file in viewModel.Files)
                {
                    _camelDocumentRepository.Add(new CamelDocument
                    {

                        FileName = file.FileName,
                        Type = file.FileType,
                        CamelID = file.CamelID,

                    });
                }
            }
        }

        public void Edit(CamelCreateViewModel viewModel)
        {
            _repo.SaveExcluded(viewModel.ToModel(), "Code");

            _camelDocumentRepository.RemoveMany(doc => doc.CamelID == viewModel.ID);

            if (viewModel.Files != null && viewModel.Files.Count > 0)
                {
                    foreach (var f in viewModel.Files)
                    {
                        _camelDocumentRepository.Add(new CamelDocument
                        {
                            FileName = f.FileName,
                            Type = f.FileType,
                            CamelID = viewModel.ID
                        });
                    }
                }
                

            }

            public CamelViewModel GetByID(int id)
        {

            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var postUserAction = _repo.GetAll().Where(postAction => postAction.ID == id)
                .Select(obj => new CamelViewModel
                {
                    ID = obj.ID,
                    BirthDate = obj.BirthDate,
                    CategoryArabicName = obj.Category.NameArabic,
                    CategoryEnglishName = obj.Category.NameEnglish,
                    UserImage = obj.User.UserProfile.MainImage != null ? protocol + "://" + hostName + "/uploads/User-Document/" + obj.User.UserProfile.MainImage : "",
                    UserName = obj.User.UserName,
                    DisplayName = obj.User.DisplayName,
                    CategoryID = obj.CategoryID,
                    Details = obj.Details,
                    FatherName = obj.FatherName,
                    Location = obj.Location,
                    MotherName = obj.MotherName,
                    Name = obj.Name,
                    Code = obj.Code,
                    GenderID = obj.GenderConfigDetailID,
                    GenderName = obj.GenderConfigDetail.NameArabic,
                    camelDocuments = obj.CamelDocuments.Where(doc => !doc.IsDeleted).Select(doc => new CamelDocumentViewModel
                    {
                        FilePath = doc.FileName != null ? protocol + "://" + hostName + "/uploads/Camel-Document/" + doc.FileName : "",
                        UploadedDate =  doc.CreatedDate,
                        FileType = doc.Type
                    }).ToList()

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

        public IEnumerable<GenderConfigDetailViewModel> GetAllowdCamelGenderTypes(DateTime birthDate)
        {
            var age =  CalculateAge(birthDate);

            IEnumerable<GenderConfigDetail> allowedGenderTypes =
                      _genderConfigRepository.GetCamelGender(age);

            return allowedGenderTypes.Select(x => new GenderConfigDetailViewModel
            {
                ID = x.ID,
                Name = x.NameArabic
            });
        }
        public int CalculateAge(DateTime birthDate)
        {
            int age = 0;
            age = DateTime.Now.Year - birthDate.Year;
            if(DateTime.Now.DayOfYear < birthDate.DayOfYear)
            {
                age = age - 1;
            }
            return age;
        }
    }
}

