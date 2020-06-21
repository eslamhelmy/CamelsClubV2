using CamelsClub.API.App_Start;
using CamelsClub.Data.UnitOfWork;
using CamelsClub.Repositories;
using CamelsClub.Services;
using CamelsClub.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CamelsClub.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Bootstrapper.Run();
            //var _competitionRepository = DependencyResolver.Current.GetService<ICompetitionRepository>();
            //var _competitionCheckerRepository = DependencyResolver.Current.GetService<ICompetitionCheckerRepository>();
            //var _competitionInviteRepository = DependencyResolver.Current.GetService<ICompetitionInviteRepository>();
            //var _notificationService = DependencyResolver.Current.GetService<INotificationService>();
            //Timer timer = new Timer((x) => {
            //    var dateToday = DateTime.Now.Date;
            //    //if date < endjoin date , send notification and if date = end join date + 1 make the assign
            //    var competitions = _competitionRepository.GetAll()
            //                            .Where(c => !c.IsDeleted && c.From > dateToday)
            //                            .ToList();
            //    foreach (var item in competitions)
            //    {
            //        if (DateTime.Now.Date < item.InvitedUsersEndJoinDate.Date)
            //        {
            //            //send notification to users who do not set their camels 
            //            var users =
            //            _competitionInviteRepository.GetAll()
            //                    .Where(c => c.CompetitionID == item.ID)
            //                    .Where(c => !c.SubmitDateTime.HasValue)
            //                    .Select(c => c.User);
            //            var notifcation = new NotificationCreateViewModel
            //            {
            //                ID = item.ID,
            //                ContentArabic = $"باقى على انتهاء موعد الالتحاق {(item.InvitedUsersEndJoinDate.Date - dateToday).TotalDays}يوم",
            //                ContentEnglish = $"End join Date of competition is {(item.InvitedUsersEndJoinDate.Date - dateToday).TotalDays} days",
            //                NotificationTypeID = 1,
            //                EngNotificationType = "Competition End Join Date",
            //                ArbNotificationType = "  اخر موعد للالتحاق بالمسابقة",
            //                SourceID = item.UserID,
            //                //  SourceName = insertedCompetition.User.Name,
            //                CompetitionImagePath = item.Image,
            //                CompetitionID = item.ID,

            //            };

            //            _notificationService.SendNotifictionForInvites(notifcation, users.Select(i => new CompetitionInviteCreateViewModel { UserID = i.ID }).ToList());

            //        }
            //        var alreadyAssigned =
            //          _competitionInviteRepository.GetAll()
            //                                      .Where(i => i.CompetitionID == item.ID)
            //                                      .Where(i => !i.IsDeleted)
            //                                      .Where(i => i.CheckerID.HasValue)
            //                                      .Any();
            //        if (!alreadyAssigned)
            //        {
            //            if (item.InvitedUsersEndJoinDate <= item.CheckersEndJoinDate && DateTime.Now.Date == item.CheckersEndJoinDate.Date.AddDays(1))
            //            {
            //                //make the assign
            //                var submittedCheckers =
            //           _competitionCheckerRepository.GetAll()
            //               .Where(c => c.CompetitionID == item.ID && !c.IsBoss && c.SubmitDateTime.HasValue)
            //               .ToList();
            //                var submittedCheckersCount = submittedCheckers.Count;

            //                //get number of submitted invites
            //                var submittedInvites = _competitionInviteRepository.GetAll()
            //                                            .Where(i => i.CompetitionID == item.ID && i.SubmitDateTime.HasValue)
            //                                            .ToList();
            //                var submittedInvitesCount = submittedInvites.Count;
            //                //start to assign
            //                var numberOfInvitesPerChecker = submittedInvitesCount / submittedCheckersCount;
            //                var invitedUserTempCounter = submittedInvitesCount;
            //                //start to assign
            //                foreach (var ch in submittedCheckers)
            //                {
            //                    int i = 0;
            //                    while (i < numberOfInvitesPerChecker)
            //                    {
            //                        submittedInvites[--invitedUserTempCounter].CheckerID = item.ID;
            //                        i++;
            //                    }
            //                }
            //                submittedInvites
            //                    .Where(i => i.CheckerID == null)
            //                    .ToList()
            //                    .ForEach(i => i.CheckerID = submittedCheckers[submittedCheckers.Count - 1].ID);


            //            }
            //            if (item.InvitedUsersEndJoinDate > item.CheckersEndJoinDate && DateTime.Now.Date == item.InvitedUsersEndJoinDate.Date.AddDays(1))
            //            {
            //                //make the assign
            //                var submittedCheckers =
            //                    _competitionCheckerRepository.GetAll()
            //                    .Where(c => c.CompetitionID == item.ID && !c.IsBoss && c.SubmitDateTime.HasValue)
            //                    .ToList();
            //                var submittedCheckersCount = submittedCheckers.Count;

            //                //get number of submitted invites
            //                var submittedInvites = _competitionInviteRepository.GetAll()
            //                                            .Where(i => i.CompetitionID == item.ID && i.SubmitDateTime.HasValue)
            //                                            .ToList();
            //                var submittedInvitesCount = submittedInvites.Count;
            //                //start to assign
            //                var numberOfInvitesPerChecker = submittedInvitesCount / submittedCheckersCount;
            //                var invitedUserTempCounter = submittedInvitesCount;
            //                //start to assign
            //                foreach (var ch in submittedCheckers)
            //                {
            //                    int i = 0;
            //                    while (i < numberOfInvitesPerChecker)
            //                    {
            //                        submittedInvites[--invitedUserTempCounter].CheckerID = item.ID;
            //                        i++;
            //                    }
            //                }
            //                submittedInvites
            //                    .Where(i => i.CheckerID == null)
            //                    .ToList()
            //                    .ForEach(i => i.CheckerID = submittedCheckers[submittedCheckers.Count - 1].ID);

            //            }

            //        }

            //    }
               
            //}, null, 86400000, 86400000);

        }
     
    }
}
