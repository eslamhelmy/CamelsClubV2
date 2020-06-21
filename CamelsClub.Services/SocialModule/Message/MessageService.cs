using CamelsClub.Data.Extentions;
using CamelsClub.Data.UnitOfWork;
using CamelsClub.Localization.Shared;
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
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;
      
        public MessageService(IUnitOfWork unit,
                              IMessageRepository messageRepository,
                              INotificationService notificationService,
                              IUserRepository userRepository)
        {
            _unit = unit;
            _messageRepository = messageRepository;
            _notificationService = notificationService;
            _userRepository = userRepository;
        }
        // get my Messages
        public PagingViewModel<MessageViewModel> GetReceivedMessage(int FromUserID , int LoggedUserID , string orderBy = "ID", bool isAscending = false, int pageIndex = 1, int pageSize = 20, Languages language = Languages.Arabic)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var query = _messageRepository.GetAll()
                .Where(x => !x.IsDeleted)
                .Where(x=> x.ToUserID == LoggedUserID || x.ToUserID == FromUserID)
                .Where(x => x.FromUserID == FromUserID || x.FromUserID == LoggedUserID);
            
            //make them seen

            int records = query.Count();
            if (records <= pageSize || pageIndex <= 0) pageIndex = 1;
            int pages = (int)Math.Ceiling((double)records / pageSize);
            int excludedRows = (pageIndex - 1) * pageSize;

            List<MessageViewModel> result = new List<MessageViewModel>();

            var requests = query.Select(obj => new MessageViewModel
            {
                ID = obj.ID,
                Text = obj.Text,
                Received = obj.FromUserID == FromUserID ? true : false ,
                CreatedDate = obj.CreatedDate
            })
            .OrderByPropertyName(orderBy, isAscending);
  
            result = requests.Skip(excludedRows).Take(pageSize).ToList();
            result.ForEach(x => x.CreatedDate = x.CreatedDate.AddHours(7));
            result.ForEach(x => x.FormattedDateTime = x.CreatedDate.ToString("yyyy'/'MM'/'dd' 'HH':'mm"));
            var allmessages = query.ToList();
            allmessages
                .Where(m => m.ToUserID == LoggedUserID)
                .ToList()
                .ForEach(m => m.IsSeen = true);
            
            _unit.Save();
            
            return new PagingViewModel<MessageViewModel>() { PageIndex = pageIndex, PageSize = pageSize, Result = result, Records = records, Pages = pages };
        }
        
        //specific for logged user
        public int GetUnSeenMessagesCount(int loggedUserID)
        {
            var count = 
            _messageRepository.GetAll()
                .Where(x => !x.IsDeleted)
                .Where(x => x.ToUserID == loggedUserID)
                .Where(x => !x.IsSeen)
                .Count();
            return count;
        }
        //get users that has messages with logged user
        public List<UserChatViewModel> GetUsersHasChatWithLoggedUser(int loggedUserID)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            var users =
            _messageRepository.GetAll()
                .Where(x => !x.IsDeleted)
                .Where(x => x.ToUserID == loggedUserID || x.FromUserID == loggedUserID)
                .Select(x => new
                {
                    ID = x.ID,
                    CreatedDate = x.CreatedDate,
                    Text = x.Text,
                    IsSeen = x.IsSeen,
                    Received = x.ToUserID == loggedUserID ? true : false,
                    UserID = x.ToUserID == loggedUserID ? x.FromUserID : x.ToUserID,
                    UserName = x.ToUserID == loggedUserID ? x.FromUser.UserName : x.ToUser.UserName,
                    DisplayName = x.ToUserID == loggedUserID ? x.FromUser.DisplayName : x.ToUser.DisplayName,
                    UserImage = x.ToUserID == loggedUserID ?
                        protocol + "://" + hostName + "/uploads/User-Document/" + x.FromUser.UserProfile.MainImage :
                        protocol + "://" + hostName + "/uploads/User-Document/" + x.ToUser.UserProfile.MainImage 
                }).GroupBy(x => new { x.UserID, x.UserName })
                .Select(g => new UserChatViewModel
                {
                    UserID = g.Key.UserID,
                    UserName = g.Key.UserName,
                    DisplayName = g.FirstOrDefault().DisplayName,
                    UserImage = g.FirstOrDefault().UserImage,
                    LastMessageID = g.OrderByDescending(m => m.ID).FirstOrDefault().ID,
                    LastMessage = g.OrderByDescending(m=>m.ID).FirstOrDefault().Text,
                    CreatedDate = g.OrderByDescending(m => m.ID).FirstOrDefault().CreatedDate,
                    UnSeenMessagesCount = g.Where(m => m.Received).Where(m => !m.IsSeen).Count()
                }).OrderByDescending(x=>x.LastMessageID).ToList();
            users.ForEach(x => x.CreatedDate = x.CreatedDate.AddHours(7));
            return users;
        }
        public bool Send(MessageCreateViewModel viewModel)
        {
            if(viewModel.FromUserID == viewModel.ToUserID)
            {
               throw new Exception(Resource.SendMessageToYourself);
            }
            var users = _userRepository.GetAll()
                            .Where(x=> !x.IsDeleted)
                            .Where(x => x.ID == viewModel.FromUserID || x.ID == viewModel.ToUserID)
                            .ToList();
            var message = _messageRepository.Add(viewModel.ToModel());
            _unit.Save();
            //send notification to that user
            //_notificationService.SendNotifictionForUser(new NotificationCreateViewModel
            //{
            //    ContentArabic = $"تم ارسال رسالة اليك من قبل {users.FirstOrDefault(x => x.ID == viewModel.FromUserID).Name}",
            //    SourceID = viewModel.FromUserID,
            //    DestinationID = viewModel.ToUserID,
            //    MessageID = message.ID,
            //    NotificationTypeID = 14,
            //    DestinationName = users.FirstOrDefault(x=>x.ID == viewModel.ToUserID).Name,
            //    SourceName = users.FirstOrDefault(x => x.ID == viewModel.FromUserID).Name,
            //    ContentEnglish = $"a message has been sent to you from {users.FirstOrDefault(x => x.ID == viewModel.FromUserID).Name}"
                
            //});

            return true;
        }


    }


}

