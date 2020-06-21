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
    public class GroupService : IGroupService
    {
        private readonly IUnitOfWork _unit;
        private readonly IGroupRepository _repo;
        private readonly ICamelRepository _camelRepository;
        private readonly ICamelGroupRepository _camelGroupRepository;

        public GroupService(IUnitOfWork unit,
                            IGroupRepository repo,
                            ICamelRepository camelRepository,
                            ICamelGroupRepository camelGroupRepository)
        {
            _unit = unit;
            _repo = repo;
            _camelRepository = camelRepository;
            _camelGroupRepository = camelGroupRepository;
        }
        public PagingViewModel<GroupViewModel> Search(int userID , string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var query = _repo
                        .GetAll()
                        .Where(post => !post.IsDeleted)
                        .Where(x=>x.UserID == userID);
            
            int records = query.Count();
            if (records <= pageSize || pageIndex <= 0) pageIndex = 1;
            int pages = (int)Math.Ceiling((double)records / pageSize);
            int excludedRows = (pageIndex - 1) * pageSize;

            List<GroupViewModel> result = new List<GroupViewModel>();

            var posts = query.Select(obj => new GroupViewModel
            {
                ID = obj.ID,
                NameArabic = obj.NameArabic ,
                NameEnglish = obj.NameEnglish ,
                UserName  = obj.User.UserName,
                ImagePath = protocol + "://" + hostName + "/uploads/Camel-Document/" + obj.Image,
                CategoryNameArabic = obj.Category.NameArabic,
                CategoryNameEnglish = obj.Category.NameEnglish,              
                camelsGroup = obj.CamelGroups
                                    .Where(x => !x.IsDeleted).
                                     Select(x => new CamelGroupViewModel
                                     {
                                         ID = x.ID,
                                         CamelID = x.CamelID,
                                         GroupID = x.GroupID,
                                         CamelName = x.Camel.Name
                                     }).ToList()
        }).OrderByPropertyName(orderBy, isAscending);

            result = posts.Skip(excludedRows).Take(pageSize).ToList();
            return new PagingViewModel<GroupViewModel>() { PageIndex = pageIndex, PageSize = pageSize, Result = result, Records = records, Pages = pages };
        }


        public List<GroupViewModel> GetUserGroups(int userID)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var query = _repo
                        .GetAll()
                        .Where(post => !post.IsDeleted)
                        .Where(x => x.UserID == userID);

            
            List<GroupViewModel> result = new List<GroupViewModel>();

            var posts = query.Select(obj => new GroupViewModel
            {
                ID = obj.ID,
                NameArabic = obj.NameArabic,
                NameEnglish = obj.NameEnglish,
                UserName = obj.User.UserName,
                ImagePath = protocol + "://" + hostName + "/uploads/Camel-Document/" + obj.Image,
                CategoryNameArabic = obj.Category.NameArabic,
                CategoryNameEnglish = obj.Category.NameEnglish,
                camelsGroup = obj.CamelGroups
                                    .Where(x => !x.IsDeleted).
                                     Select(x => new CamelGroupViewModel
                                     {
                                         ID = x.ID,
                                         CamelID = x.CamelID,
                                         GroupID = x.GroupID,
                                         CamelName = x.Camel.Name,
                                         camelImages = x.Camel.CamelDocuments.Where(doc => !doc.IsDeleted && doc.FileName != null).Select(doc => new CamelDocumentViewModel
                                         {
                                             FilePath =  protocol + "://" + hostName + "/uploads/Camel-Document/" + doc.FileName,
                                             UploadedDate = doc.CreatedDate
                                         }).ToList()
                                     }).ToList()
            }).OrderByDescending(x => x.ID).ToList();

            return posts;
        }

        public GroupViewModel Add(GroupCreateViewModel viewModel)
        {

            var addedGroup =  _repo.Add(viewModel.ToModel());
            //check for the category of each camel in the group
            var ids = viewModel.Camels.Select(c => c.CamelID);
            var camels = _camelRepository.GetAll().Where(x => ids.Contains(x.ID)).ToList();
            foreach (var camel in camels)
            {
                if (camel.UserID != viewModel.UserID)
                {
                    throw new Exception($"this camel {camel.Name} does not belong to logged user");
                }
                if(camel.CategoryID != viewModel.CategoryID)
                {
                    throw new Exception($"الفئة التي قمت باختيارها غير متطابقة مع فئة الابل");
                }
            }
            if(viewModel.Camels != null && viewModel.Camels.Count() > 0)
            {
                foreach (var item in viewModel.Camels)
                {
                    _camelGroupRepository.Add(new CamelGroup
                    {
                        CamelID = item.CamelID,
                        GroupID = addedGroup.ID
                    });
                }
            }
            _unit.Save();
            return new GroupViewModel
            {
                ID = addedGroup.ID,
                NameArabic = addedGroup.NameArabic,
                NameEnglish = addedGroup.NameEnglish
            };
            
        }

        public GroupViewModel Edit(GroupCreateViewModel viewModel)
        {
            var editedGroup = _repo.Edit(viewModel.ToModel());

            var oldGroupCamelIds =
                   _camelGroupRepository.GetAll()
                               .Where(x => !x.IsDeleted)
                               .Select(x => x.ID)
                               .ToList();

            var newGroupCamels =
                   viewModel.Camels.Where(x => x.ID == 0).ToList();
            newGroupCamels.ForEach(x => x.GroupID = editedGroup.ID);

            var editedGroupCamels =
                  viewModel.Camels.Where(x => x.ID != 0).ToList();

            var deletedGroupCamelIDs =
                  oldGroupCamelIds.
                       Where(o =>
                       !editedGroupCamels.Select(x => x.ID).Contains(o));

            _camelGroupRepository.
                 AddRange(newGroupCamels.Select(x => x.ToModel()));
            _camelGroupRepository.
                EditRange(editedGroupCamels.Select(x => x.ToModel()));

            _camelGroupRepository.
                RemoveRange(deletedGroupCamelIDs);

            _unit.Save();

            return new GroupViewModel
            {
                ID = editedGroup.ID,
                NameArabic = editedGroup.NameArabic,
                NameEnglish = editedGroup.NameEnglish
            };
        }
        public GroupCreateViewModel GetEditableByID(int id)
        {
            var group = _repo.GetAll().Where(g => g.ID == id)
                .Select(obj => new GroupCreateViewModel
                {
                    ID = obj.ID,
                    NameArabic = obj.NameArabic,
                    NameEnglish = obj.NameEnglish,
                    CategoryID = obj.CategoryID
                }).FirstOrDefault();
            var camelsGroup =
            _camelGroupRepository.
                              GetAll().
                                  Where(x => x.GroupID == group.ID).
                                  Where(x => !x.IsDeleted).
                                     Select(x => new CamelGroupCreateViewModel
                                     {
                                         ID = x.ID,
                                         CamelID = x.CamelID,
                                         GroupID = x.GroupID
                                     }).ToList();
            group.Camels = camelsGroup;
            return group;

        }

        public GroupViewModel GetByID(int id)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var group = _repo.GetAll().Where(postAction => postAction.ID == id)
                .Select(obj => new GroupViewModel
            {
                ID = obj.ID,
                NameArabic = obj.NameArabic,
                NameEnglish = obj.NameEnglish ,
                CategoryNameArabic = obj.Category.NameArabic,
                CategoryNameEnglish = obj.Category.NameEnglish,
                UserName = obj.User.UserName,
                ImagePath = obj.Image != null ? protocol + "://" + hostName + "/uploads/Camel-Document/" + obj.Image : ""
                }).FirstOrDefault();

            var camelsGroup =
         _camelGroupRepository.
                           GetAll().
                               Where(x => x.GroupID == group.ID).
                               Where(x => !x.IsDeleted).
                                  Select(x => new CamelGroupViewModel
                                  {
                                      ID = x.ID,
                                      CamelID = x.CamelID,
                                      CamelName = x.Camel.Name,
                                      GroupID = x.GroupID,
                                      camelImages = x.Camel.CamelDocuments.Where(doc => !doc.IsDeleted).Select(doc => new CamelDocumentViewModel
                                      {
                                          FilePath = protocol + "://" + hostName + "/uploads/Camel-Document/" + doc.FileName,
                                          UploadedDate = doc.CreatedDate
                                      }).ToList()
                                  }).ToList();

            group.camelsGroup = camelsGroup;
            return group;

        }


        public bool IsExists(int id)
        {
            return _repo.GetAll().Where(x => x.ID == id).Any();
        }
        public void Delete(int id)
        {
            _repo.Remove(id);
        }

        public bool RemoveCamel(int userID, int camelID, int groupID)
        {
            //check if user has group with groupID
           var data = 
            _repo.GetAll().Where(x => x.ID == groupID).Select(x => new
            {
                UserID = x.UserID,
                HasCurrentCompetitions = x.CamelCompetitions.Select(cc => cc.Competition).Any(c => c.Published != null)
            }).FirstOrDefault();
            //check if group is in competition
            if(data == null)
            {
                throw new Exception($"No Group with this ID: {groupID}");
            }
            if(data.UserID != userID)
            {
                throw new Exception($"group with ID: {groupID} doesn't belong to logged user");
            }
            if (data.HasCurrentCompetitions)
            {
                throw new Exception($"group with ID: {groupID} has involved in current competitions and you are not allowed to remove camels");
            }
            //remove camel from groupID
            var camel = _camelGroupRepository.GetAll().Where(x => x.GroupID == groupID && x.CamelID == camelID && x.Camel.UserID == userID).FirstOrDefault();
            if(camel == null)
            {
                throw new Exception($"camel with ID: {camelID} not in group with ID: {groupID}");
            }
            camel.IsDeleted = true;
            _unit.Save();
            return true;
        }


    }
}

