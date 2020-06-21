﻿using CamelsClub.ViewModels;
using CamelsClub.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Services
{
    public interface ICommentService
    {
        //test
        PagingViewModel<CommentViewModel> Search(int postID = 0 ,string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic);
        void Add(CommentCreateViewModel view);
        CommentViewModel GetByID(int commentId);
        bool IsExists(int CoomentId);
        void Edit(CommentCreateViewModel viewModel);
        void Delete(int commentId);
        IEnumerable<CommentViewModel> GetReplies(int commentId);

    }
}
