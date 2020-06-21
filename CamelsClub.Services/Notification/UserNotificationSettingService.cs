using CamelsClub.Data.Extentions;
using CamelsClub.Data.UnitOfWork;
using CamelsClub.Localization.Shared;
using CamelsClub.Models;
using CamelsClub.Repositories;
using CamelsClub.Services.Helpers;
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
    public class UserNotificationSettingService : IUserNotificationSettingService
    {

        private readonly IUnitOfWork _unit;
        private readonly IUserNotificationSettingRepository _repo;
        private readonly INotificationSettingRepository _notificationSettingRepository;
      
        public UserNotificationSettingService(IUnitOfWork unitOfWork,
                                    IUserNotificationSettingRepository repo,
                                    INotificationSettingRepository notificationSettingRepository)

                                         
        {
            _unit = unitOfWork;
            _repo = repo;
            _notificationSettingRepository = notificationSettingRepository; 
        }
        
        //for logged user
        //public List<UserNotificationSettingEditViewModel> GetUserNotificationSettings(int loggedUserID)
        //{
        //    var isUserExist =  _repo.GetAll().Any(x => x.UserID == loggedUserID && !x.IsDeleted);
        
        //    if(isUserExist)
        //    {
        //        var AllNotificationSettings = _repo.GetAll()
        //                                            .Where(x => x.UserID == loggedUserID)
        //                                            .Where(x => !x.IsDeleted)
        //                                            .Select(x => new UserNotificationSettingEditViewModel
        //                                            {
        //                                                ID = x.ID,
        //                                                NotificationSettingID = x.NotificationSettingID,
        //                                                NotificationSettingValueID = x.NotificationSettingValueID,
        //                                                NotificationSettingText = x.NotificationSetting.TextArabic   
        //                                            }).ToList();
        //        //select all notification setting putted for that logged user
        //        var userNotificationSettingIDs = AllNotificationSettings.Select(x => x.NotificationSettingID).ToList();
        //        //get all notification setting that not put to that logged user
        //        var newNotificationSettingIDs = 
        //                _notificationSettingRepository
        //                                        .GetAll()
        //                                        .Where(x => !x.IsDeleted)
        //                                        .Select(x => x.ID)
        //                                        .Where(x => !userNotificationSettingIDs.Contains(x))
        //                                        .ToList();
        //        if (newNotificationSettingIDs != null && newNotificationSettingIDs.Count() > 0)
        //        {
        //            List<UserNotificationSetting> list = new List<UserNotificationSetting>();
        //            foreach (var newNotificationSettingID in newNotificationSettingIDs)
        //            {
        //                var addedObj =
        //                     _repo.Add(new Models.UserNotificationSetting
        //                     {
        //                         UserID = loggedUserID,
        //                         NotificationSettingID = newNotificationSettingID,
        //                         NotificationSettingValueID = (int)NotificationSettingValue.NotSet
        //                     });
        //                list.Add(addedObj);
        //            }
        //            _unit.Save();
        //            foreach (var item in list)
        //            {
        //                AllNotificationSettings.Add(new UserNotificationSettingEditViewModel
        //                {
        //                    ID = item.ID,
        //                    NotificationSettingID = item.NotificationSettingID,
        //                    Value = item.Value,
        //                    NotificationSettingText = item.NotificationSetting?.TextArabic
        //                });
        //            }
        //        } 
        //            return AllNotificationSettings;

                

        //    }
        //    else
        //    {
        //        //get all notification setting that not put to that logged user
        //        var newNotificationSettingIDs =
        //                _notificationSettingRepository
        //                                        .GetAll()
        //                                        .Where(x => !x.IsDeleted)
        //                                        .Select(x => x.ID)
        //                                        .ToList();
        //        List<UserNotificationSetting> list = new List<UserNotificationSetting>();

        //        if (newNotificationSettingIDs != null && newNotificationSettingIDs.Count() > 0)
        //        {
        //            foreach (var newNotificationSettingID in newNotificationSettingIDs)
        //            {
        //                var addedObj =
        //                     _repo.Add(new Models.UserNotificationSetting
        //                     {
        //                         UserID = loggedUserID,
        //                         NotificationSettingID = newNotificationSettingID,
        //                         Value = (int)NotificationSettingValue.NotSet
        //                     });
        //                list.Add(addedObj);
        //            }
        //            _unit.Save();
        //        }
        //        return list.Select(x => new UserNotificationSettingEditViewModel
        //        {
        //            ID = x.ID,
        //            NotificationSettingID = x.NotificationSettingID,
        //            Value = x.Value,
        //            NotificationSettingText = x.NotificationSetting?.TextArabic

        //        }).ToList();

        //    }

        //}
    
        ////edit user notification setting 
        //public bool EditUserNotificationSettings(List<UserNotificationSettingEditViewModel> viewModels)
        //{
        //    foreach (var item in viewModels)
        //    {
        //        _repo.SaveIncluded(new UserNotificationSetting { ID = item.ID, Value = item.Value }, "Value");
        //    }
        //    return true;
        //}
        public bool EditNotificationSetting(int loggedUserID, List<NotificationSettingEditViewModel> list)
        {
            foreach (var item in list)
            {
                var associatedUserNotification = 
                        _repo.GetAll()
                            .Where(x => x.UserID == loggedUserID && x.NotificationSettingID == item.ID)
                            .Where(x => !x.IsDeleted)
                            .FirstOrDefault();
                if(associatedUserNotification == null)
                {
                    _repo.Add(new UserNotificationSetting
                    {
                        UserID = loggedUserID,
                        NotificationSettingID = item.ID,
                        NotificationSettingValueID = item.Values.FirstOrDefault(v => v.IsPicked).ID

                    });
                }
                else
                {
                    associatedUserNotification.NotificationSettingValueID = item.Values.FirstOrDefault(v => v.IsPicked).ID;
                }
            }
            _unit.Save();
            return true;
        }
        public List<NotificationSettingEditViewModel> GetNotificationSetting(int loggedUserID, Languages language)
        {
            var settings = 
            _notificationSettingRepository
                                    .GetAll()
                                    .Where(x => !x.IsDeleted)
                                    .Select(x => new NotificationSettingEditViewModel
                                    {
                                        ID = x.ID,
                                        Question = language == Languages.Arabic ? x.TextArabic : x.TextEnglish,
                                        Values = x.Values.Select(v => new NotificationValueEditViewModel
                                        {
                                            ID = v.ID,
                                            Value = language == Languages.Arabic ? v.TextArabic : v.TextEnglish,
                                            IsPicked = v.Users
                                                            .Where(u => u.UserID == loggedUserID && u.NotificationSettingID == x.ID)
                                                            .Where(u => !u.IsDeleted).Any(),
                                            IsDefault = v.IsDefault
                                        }).ToList()

                                    }).ToList();

            settings.ForEach(x =>
            {
                if (x.Values.All(v => !v.IsPicked))
                {
                    //get default value for setting
                    var value = x.Values.FirstOrDefault(v => v.IsDefault);
                    value.IsPicked = true;
                }
            });



            return settings;
        }
    }        

}
