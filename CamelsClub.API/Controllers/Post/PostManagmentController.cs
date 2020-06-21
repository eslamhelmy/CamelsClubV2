using CamelsClub.API.Filters;
using CamelsClub.API.Helpers;
using CamelsClub.Data.UnitOfWork;
using CamelsClub.Localization.Shared;
using CamelsClub.Services;
using CamelsClub.Services.Helpers;
using CamelsClub.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CamelsClub.API.Controllers.Post
{
    public class PostManagmentController : BaseController
    {
        private readonly IUnitOfWork _unit;
        private readonly IPostService _postService;

        public PostManagmentController(IUnitOfWork unit, IPostService postService)
        {
            _unit = unit;
            _postService = postService;

        }

        [HttpGet]
        [AuthorizeUserFilter(Role = "User") ]
        [Route("api/UserPost/GetList")]
        public ResultViewModel<PagingViewModel<PostDetailsViewModel>> GetList(string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20)
        {
            ResultViewModel<PagingViewModel<PostDetailsViewModel>> resultViewModel = new ResultViewModel<PagingViewModel<PostDetailsViewModel>>();
                var userID = int.Parse(UserID);
                resultViewModel.Data = _postService.Search(userID, orderBy, isAscending, pageIndex, pageSize, Language);
                resultViewModel.Success = true;
                resultViewModel.Message = Resource.DataLoaded;
                return resultViewModel;

           
        }

        [HttpGet]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/UserPost/GetHomePosts")]
        public ResultViewModel<PagingViewModel<PostDetailsViewModel>> GetHomePosts(string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20)
        {
            ResultViewModel<PagingViewModel<PostDetailsViewModel>> resultViewModel = new ResultViewModel<PagingViewModel<PostDetailsViewModel>>();
                var userID = int.Parse(UserID);
                resultViewModel.Data = _postService.GetHomePosts(userID, orderBy, isAscending, pageIndex, pageSize, Language);
                resultViewModel.Success = true;
                resultViewModel.Message = Resource.DataLoaded;
                return resultViewModel;
        }

        [HttpPost]
        [AuthorizeUserFilter(Role = "User") ]
        [Route("api/UserPost/Add")]
        //[ValidateViewModel]
        public ResultViewModel<int> CreatePost(CreatePostViewModel view)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var res = new ResultViewModel<int>
                    {
                        Errors = APIHelper.ValidationMessages(ModelState),
                        Success = false,
                        Message = "Invalid Request"
                    };

                    return res;
                }

                ResultViewModel<int> resultViewModel = new ResultViewModel<int>();

                var userID = int.Parse(UserID);
                if (userID != 0)
                {
                    view.UserID = userID;
                    var post = _postService.CreatePost(view);
                    if (view.Files != null && view.Files.Count > 0)
                    {
                        foreach (var file in view.Files)
                        {
                            FileHelper.MoveFileFromTempPathToAnotherFolder(file.FileName, "Post-Document");
                        }

                    }
                    _unit.Save();
                    resultViewModel.Success = true;
                    resultViewModel.Data = post.ID;
                    resultViewModel.Message = Resource.PostAddedSuccessfully;
                }
                else
                {
                    resultViewModel.Success = true;
                    resultViewModel.Data = 0;
                    resultViewModel.Message = Resource.UserNotFound;
                }

                return resultViewModel;

            }
            catch (Exception ex)
            {
                return new ResultViewModel<int>(ex.Message);
            }
           }

        [HttpGet]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/UserPost/GetByID")]
        public ResultViewModel<PostDetailsViewModel> GetByID(int PostId)
        {
            ResultViewModel<PostDetailsViewModel> resultViewModel = new ResultViewModel<PostDetailsViewModel>();
           
            if (_postService.IsPostExists(PostId))
            {
                var loggedUserID = int.Parse(UserID);
                var res = _postService.GetByID(loggedUserID,PostId);
                resultViewModel.Success = true;
                resultViewModel.Data = res;
                resultViewModel.Message = Resource.DataLoaded;

            }
            else
            {
                resultViewModel.Success = false;
                resultViewModel.Message = Resource.PostNotFound;
            }
            return resultViewModel;
        }


        [HttpPut]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/UserPost/Update")]
        [ValidateViewModel]
        public ResultViewModel<bool> Update(CreatePostViewModel view)
        {
            ResultViewModel<bool> resultViewModel = new ResultViewModel<bool>();
                view.UserID = int.Parse(UserID);
                _postService.UpdatePost(view);
                _unit.Save();
                resultViewModel.Success = true;
                resultViewModel.Message = Resource.PostUpdatededSuccessfully;
            resultViewModel.Data = true;
                return resultViewModel;
            }

        [HttpDelete]
        [AuthorizeUserFilter(Role = "User")]
        [Route("api/UserPost/Delete")]
        public ResultViewModel<bool> DeletePost(int id)
      {
            ResultViewModel<bool> resultViewModel = new ResultViewModel<bool>();

            if (_postService.IsPostExists(id))
            {
                _postService.DeletePost(id);
                _unit.Save();
                resultViewModel.Success = true;
                resultViewModel.Data =true;
                resultViewModel.Message = Resource.PostDeletededSuccessfully;

            }
            else
            {
                resultViewModel.Success = false;
                resultViewModel.Data = false;

                resultViewModel.Message = Resource.PostNotFound;
            }
            return resultViewModel;
            
        }
    }
}
