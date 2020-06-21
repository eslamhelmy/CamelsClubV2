namespace CamelsClub.Data.Migrations
{
    using CamelsClub.Data.Context;
    using CamelsClub.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CamelsClub.Data.Context.CamelsClubContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CamelsClub.Data.Context.CamelsClubContext context)
        {
          //  AddPages(context);
          //  AddPermissions(context);
            AddPagePermissions(context);
           // AddRoles(context);
           // AddUsers(context);
           // AddActions(context);
           // AddCategories(context);
           //// AddGenderConfig(context);
           // AddNotificationTypes(context);
           // AddCamelSpecification(context);
           // AddNotificationSetting(context);

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }

        private void AddPages(CamelsClubContext context)
        {
            context.Pages.AddOrUpdate(new Models.Page
            {
                ID = (int) CamelsClub.Models.Enums.Page.Camels,
                NameArabic = "الايل",
                NameEnglish = "Camels ",
                CreatedBy = "1",
                CreatedDate = DateTime.UtcNow,

            });
            context.Pages.AddOrUpdate(new Models.Page
            {
                ID = (int)CamelsClub.Models.Enums.Page.Users,
                NameArabic = "المستخدمين",
                NameEnglish = "Users",
                CreatedBy = "1",
                CreatedDate = DateTime.UtcNow,
            });

            context.Pages.AddOrUpdate(new Models.Page
            {
                ID = (int)CamelsClub.Models.Enums.Page.Competitions,
                NameArabic = "المسابقات",
                NameEnglish = "Competitions",
                CreatedBy = "1",
                CreatedDate = DateTime.UtcNow,
            });
        }

        private void AddPermissions(CamelsClubContext context)
        {
            context.Permissions.AddOrUpdate(new Models.Permission
            {
                ID = (int)CamelsClub.Models.Enums.Permission.View,
                NameArabic = "مشاهدة",
                NameEnglish = "View ",
                CreatedBy = "1",
                CreatedDate = DateTime.UtcNow,
            });
            context.Permissions.AddOrUpdate(new Models.Permission
            {
                ID = (int)CamelsClub.Models.Enums.Permission.Edit,
                NameArabic = "تعديل",
                NameEnglish = "Edit ",
                CreatedBy = "1",
                CreatedDate = DateTime.UtcNow,
            });
            context.Permissions.AddOrUpdate(new Models.Permission
            {
                ID = (int)CamelsClub.Models.Enums.Permission.Print,
                NameArabic = "طباعة",
                NameEnglish = "Print ",
                CreatedBy = "1",
                CreatedDate = DateTime.UtcNow,
            });
        }

        private void AddPagePermissions(CamelsClubContext context)
        {
            context.PagePermissions.AddOrUpdate(new Models.PagePermission
            {
                PageID = (int)CamelsClub.Models.Enums.Page.Camels,
                PermissionID = (int)CamelsClub.Models.Enums.Permission.View,
                CreatedBy = "1",
                CreatedDate = DateTime.UtcNow,
            });
            context.PagePermissions.AddOrUpdate(new Models.PagePermission
            {
                PageID = (int)CamelsClub.Models.Enums.Page.Camels,
                PermissionID = (int)CamelsClub.Models.Enums.Permission.Edit,
                CreatedBy = "1",
                CreatedDate = DateTime.UtcNow,
            });
            context.PagePermissions.AddOrUpdate(new Models.PagePermission
            {
                PageID = (int)CamelsClub.Models.Enums.Page.Camels,
                PermissionID = (int)CamelsClub.Models.Enums.Permission.Print,
                CreatedBy = "1",
                CreatedDate = DateTime.UtcNow,
            });

            context.PagePermissions.AddOrUpdate(new Models.PagePermission
            {
                PageID = (int)CamelsClub.Models.Enums.Page.Users,
                PermissionID = (int)CamelsClub.Models.Enums.Permission.View,
                CreatedBy = "1",
                CreatedDate = DateTime.UtcNow,
            });
            context.PagePermissions.AddOrUpdate(new Models.PagePermission
            {
                PageID = (int)CamelsClub.Models.Enums.Page.Users,
                PermissionID = (int)CamelsClub.Models.Enums.Permission.Edit,
                CreatedBy = "1",
                CreatedDate = DateTime.UtcNow,
            });
            context.PagePermissions.AddOrUpdate(new Models.PagePermission
            {
                PageID = (int)CamelsClub.Models.Enums.Page.Users,
                PermissionID = (int)CamelsClub.Models.Enums.Permission.Print,
                CreatedBy = "1",
                CreatedDate = DateTime.UtcNow,
            });


            context.PagePermissions.AddOrUpdate(new Models.PagePermission
            {
                PageID = (int)CamelsClub.Models.Enums.Page.Competitions,
                PermissionID = (int)CamelsClub.Models.Enums.Permission.View,
                CreatedBy = "1",
                CreatedDate = DateTime.UtcNow,
            });
            context.PagePermissions.AddOrUpdate(new Models.PagePermission
            {
                PageID = (int)CamelsClub.Models.Enums.Page.Competitions,
                PermissionID = (int)CamelsClub.Models.Enums.Permission.Edit,
                CreatedBy = "1",
                CreatedDate = DateTime.UtcNow,
            });
            context.PagePermissions.AddOrUpdate(new Models.PagePermission
            {
                PageID = (int)CamelsClub.Models.Enums.Page.Competitions,
                PermissionID = (int)CamelsClub.Models.Enums.Permission.Print,
                CreatedBy = "1",
                CreatedDate = DateTime.UtcNow,
            });
        }
        private void AddNotificationSetting(CamelsClubContext context)
        {
            context.NotificationSettings.AddOrUpdate(new Models.NotificationSetting
            {
                ID = 1,
                TextArabic = "من يستطيع مشاهدة المقالات الخاصة بك",
                TextEnglish = "من يستطيع مشاهدة المقالات الخاصة بك",
                CreatedBy = "1",
                CreatedDate = DateTime.UtcNow,
                Values = new List<NotificationSettingValue>
                {
                    new NotificationSettingValue
                    {
                        ID = 1,
                        TextArabic = "خاص",
                        TextEnglish = "خاص",
                        CreatedBy = "1",
                        CreatedDate = DateTime.UtcNow
                    },
                    new NotificationSettingValue
                    {
                        ID = 2,
                        TextArabic = "عام",
                        TextEnglish = "عام",
                        CreatedBy = "1",
                        CreatedDate = DateTime.UtcNow
                    },
                    new NotificationSettingValue
                    {
                        ID = 3,
                        TextArabic = "لا أحد",
                        TextEnglish = "لا أحد",
                        CreatedBy = "1",
                        CreatedDate = DateTime.UtcNow
                    }
                }

            });
            context.NotificationSettings.AddOrUpdate(new Models.NotificationSetting
            {
                ID = 2,
                TextArabic = "من يستطيع مشاهدة الفيديوهات الخاصة بك",
                TextEnglish = "من يستطيع مشاهدة الفيديوهات الخاصة بك",
                CreatedBy = "1",
                CreatedDate = DateTime.UtcNow,
                Values = new List<NotificationSettingValue>
                {
                    new NotificationSettingValue
                    {
                        ID = 1,
                        TextArabic = "خاص",
                        TextEnglish = "خاص",
                        CreatedBy = "1",
                        CreatedDate = DateTime.UtcNow
                    },
                    new NotificationSettingValue
                    {
                        ID = 2,
                        TextArabic = "عام",
                        TextEnglish = "عام",
                        CreatedBy = "1",
                        CreatedDate = DateTime.UtcNow
                    },
                    new NotificationSettingValue
                    {
                        ID = 3,
                        TextArabic = "لا أحد",
                        TextEnglish = "لا أحد",
                        CreatedBy = "1",
                        CreatedDate = DateTime.UtcNow
                    }
                }

            });
            context.NotificationSettings.AddOrUpdate(new Models.NotificationSetting
            {
                ID = 3,
                TextArabic = "من يستطيع مشاهدة المنشور الخاصة بك",
                TextEnglish = "من يستطيع مشاهدة الفيديوهات الخاصة بك",
                CreatedBy = "1",
                CreatedDate = DateTime.UtcNow,
                Values = new List<NotificationSettingValue>
                {
                    new NotificationSettingValue
                    {
                        ID = 1,
                        TextArabic = "خاص",
                        TextEnglish = "خاص",
                        CreatedBy = "1",
                        CreatedDate = DateTime.UtcNow
                    },
                    new NotificationSettingValue
                    {
                        ID = 2,
                        TextArabic = "عام",
                        TextEnglish = "عام",
                        CreatedBy = "1",
                        CreatedDate = DateTime.UtcNow
                    },
                    new NotificationSettingValue
                    {
                        ID = 3,
                        TextArabic = "لا أحد",
                        TextEnglish = "لا أحد",
                        CreatedBy = "1",
                        CreatedDate = DateTime.UtcNow
                    }
                }

            });
            context.NotificationSettings.AddOrUpdate(new Models.NotificationSetting
            {
                ID = 4,
                TextArabic = "تحديد خاصية ارسال دعوات الدخول الي المسابقات الخاصه",
                TextEnglish = "تحديد خاصية ارسال دعوات الدخول الي المسابقات الخاصه",
                CreatedBy = "1",
                CreatedDate = DateTime.UtcNow,
                Values = new List<NotificationSettingValue>
                {
                    new NotificationSettingValue
                    {
                        ID = 1,
                        TextArabic = "أرغب",
                        TextEnglish = "أرغب",
                        CreatedBy = "1",
                        CreatedDate = DateTime.UtcNow
                    },
                    new NotificationSettingValue
                    {
                        ID = 2,
                        TextArabic = "لا أرغب",
                        TextEnglish = "لا أرغب",
                        CreatedBy = "1",
                        CreatedDate = DateTime.UtcNow
                    },
                    new NotificationSettingValue
                    {
                        ID = 3,
                        TextArabic = "فقط مشاهدة",
                        TextEnglish = "فقط مشاهدة",
                        CreatedBy = "1",
                        CreatedDate = DateTime.UtcNow
                    }
                }

            });
            

        }

        private void AddRoles(CamelsClub.Data.Context.CamelsClubContext context)
        {
            if (context.Roles.Count() == 0)
            {
                context.Roles.AddOrUpdate(new Models.Role() { ID = 1, NameEnglish = "User", NameArabic = "مستخدم" });
                context.SaveChanges();
            }
        }
        private void AddUsers(CamelsClub.Data.Context.CamelsClubContext context)
        {
            if (context.Users.Count() == 0)
            {
                
            }
        }
        private void AddActions(CamelsClub.Data.Context.CamelsClubContext context)
        {
            context.Actions.AddOrUpdate(new Models.Action
            {
                ID = 1,
                NameArabic = "أعجبني",
                NameEnglish = "Like ",
                CreatedBy = "1",
                CreatedDate = DateTime.UtcNow,

            });
            context.Actions.AddOrUpdate(new Models.Action
            {
                ID = 2,
                NameArabic = "أحببته",
                NameEnglish = "Love ",
                CreatedBy = "1",
                CreatedDate = DateTime.UtcNow,
            });
            context.Actions.AddOrUpdate(new Models.Action
            {
                ID = 3,
                NameArabic = "مشاركة",
                NameEnglish = "Share",
                CreatedBy = "1",
                CreatedDate = DateTime.UtcNow,
            });
        }

        private void AddCategories(CamelsClub.Data.Context.CamelsClubContext context)
        {
            context.Categories.AddOrUpdate(new Models.Category
            {
                ID = 1,
                NameArabic = "مجاهيم",
                NameEnglish = "مجاهيم ",
                CreatedBy = "1",
                CreatedDate = DateTime.UtcNow,

            });
            context.Categories.AddOrUpdate(new Models.Category
            {
                ID = 2,
                NameArabic = "وضح",
                NameEnglish = "وضح ",
                CreatedBy = "1",
                CreatedDate = DateTime.UtcNow,
            });
            context.Categories.AddOrUpdate(new Models.Category
            {
                ID = 3,
                NameArabic = "الصفر",
                NameEnglish = "الصفر",
                CreatedBy = "1",
                CreatedDate = DateTime.UtcNow,
            });
            context.Categories.AddOrUpdate(new Models.Category
            {
                ID = 3,
                NameArabic = "الحمر",
                NameEnglish = "الحمر",
                CreatedBy = "1",
                CreatedDate = DateTime.UtcNow,
            });
            context.Categories.AddOrUpdate(new Models.Category
            {
                ID = 3,
                NameArabic = "شعل",
                NameEnglish = "شعل",
                CreatedBy = "1",
                CreatedDate = DateTime.UtcNow,
            });
            context.Categories.AddOrUpdate(new Models.Category
            {
                ID = 3,
                NameArabic = "شقح",
                NameEnglish = "شقح",
                CreatedBy = "1",
                CreatedDate = DateTime.UtcNow,
            });
          
        }

        private void AddGenderConfig(CamelsClub.Data.Context.CamelsClubContext context)
        {

            context.GenderConfigs.AddOrUpdate(new Models.GenderConfig
            {
                FromAge = 0,
                ToAge = 4,
                CreatedBy = "1",
                CreatedDate = DateTime.UtcNow,
                GenderConfigDetails = new List<GenderConfigDetail>
                {
                    new GenderConfigDetail
                    {
                        NameArabic = "بكرة",
                        NameEnglish = "بكرة",
                        CreatedBy = "1",
                        CreatedDate = DateTime.UtcNow
                    },
                    new GenderConfigDetail
                    {
                        NameArabic = "قعود",
                        NameEnglish = "قعود",
                        CreatedBy = "1",
                        CreatedDate = DateTime.UtcNow
                    }
                }

            });
            context.GenderConfigs.AddOrUpdate(new Models.GenderConfig
            {
                FromAge= 5,
                ToAge = 7,
                CreatedBy = "1",
                CreatedDate = DateTime.UtcNow,
                GenderConfigDetails = new List<GenderConfigDetail>
                {
                    new GenderConfigDetail
                    {
                        NameArabic = "ناقة",
                        NameEnglish = "ناقة",
                        CreatedBy = "1",
                        CreatedDate = DateTime.UtcNow
                    },
                    new GenderConfigDetail
                    {
                        NameArabic = "بعير",
                        NameEnglish = "بعير",
                        CreatedBy = "1",
                        CreatedDate = DateTime.UtcNow
                    }
                }
            });
            context.GenderConfigs.AddOrUpdate(new Models.GenderConfig
            {
                FromAge = 8,
                ToAge = 1000,
                CreatedBy = "1",
                CreatedDate = DateTime.UtcNow,
                GenderConfigDetails = new List<GenderConfigDetail>
                {
                    new GenderConfigDetail
                    {
                        NameArabic = "ناقة",
                        NameEnglish = "ناقة",
                        CreatedBy = "1",
                        CreatedDate = DateTime.UtcNow
                    },
                    new GenderConfigDetail
                    {
                        NameArabic = "بعير",
                        NameEnglish = "بعير",
                        CreatedBy = "1",
                        CreatedDate = DateTime.UtcNow
                    }
                }
            });
          
        }


        private void AddNotificationTypes(CamelsClub.Data.Context.CamelsClubContext context)
        {
            context.NotificationTypes.AddOrUpdate(new Models.NotificationType
            {
                ID = 1,
                NameArabic=" مسابقة جديدة",
                NameEnglish= "New competition",
                CreatedBy = "5",
                CreatedDate = DateTime.UtcNow,

            });
            context.NotificationTypes.AddOrUpdate(new Models.NotificationType
            {
                ID = 2,
                NameArabic = "منشور جديد",
                NameEnglish = "New Post ",
                CreatedBy = "5",  /// you should enter vaild user same server database
                CreatedDate = DateTime.UtcNow,

            });

            context.NotificationTypes.AddOrUpdate(new Models.NotificationType
            {
                ID = 3,
                NameArabic = " تعليق جديد",
                NameEnglish = "New Comment ",
                CreatedBy = "5",
                CreatedDate = DateTime.UtcNow,

            });

            context.NotificationTypes.AddOrUpdate(new Models.NotificationType
            {
                ID = 4,
                NameArabic = "طلب صداقة جديد",
                NameEnglish = "New friend request",
                CreatedBy = "5",
                CreatedDate = DateTime.UtcNow,

            });
            context.NotificationTypes.AddOrUpdate(new Models.NotificationType
            {
                ID = 5,
                NameArabic = "قبول طلب الصداقة",
                NameEnglish = "Approve Friend Request  ",
                CreatedBy = "5",
                CreatedDate = DateTime.UtcNow,

            });

            context.NotificationTypes.AddOrUpdate(new Models.NotificationType
            {
                ID = 6,
                NameArabic = "الانضمام الي المسايقة",
                NameEnglish = "Join the competition",
                CreatedBy = "5",
                CreatedDate = DateTime.UtcNow,

            });

            context.NotificationTypes.AddOrUpdate(new Models.NotificationType
            {
                ID = 7,
                NameArabic = "الاعجاب بالمنشور",
                NameEnglish = "Like a Post",
                CreatedBy = "5",
                CreatedDate = DateTime.UtcNow,

            });

            context.NotificationTypes.AddOrUpdate(new Models.NotificationType
            { 

                ID = 8,
                NameArabic = "الاعجاب بالتعليق",
                NameEnglish = "Like a Comment",
                CreatedBy = "5",
                CreatedDate = DateTime.UtcNow,

            });

            context.NotificationTypes.AddOrUpdate(new Models.NotificationType
            {
                ID = 9,
                NameArabic = "الانضمام الي لجنة تحكيم المسايقة",
                NameEnglish = "Join the competition jury",
                CreatedBy = "5",
                CreatedDate = DateTime.UtcNow,

            });

            context.NotificationTypes.AddOrUpdate(new Models.NotificationType
            {
                ID = 10,
                NameArabic = "الانضمام الي لجنة التمييز ",
                NameEnglish = "Join the Discrimination Committee",
                CreatedBy = "5",
                CreatedDate = DateTime.UtcNow,

            });
            context.NotificationTypes.AddOrUpdate(new Models.NotificationType
            {
                ID = 11,
                NameArabic = " تقييم الجمل بواسطة لجنة التحكيم",
                NameEnglish = "Camel Review by Referee",
                CreatedBy = "5",
                CreatedDate = DateTime.UtcNow,

            });

            context.NotificationTypes.AddOrUpdate(new Models.NotificationType
            {
                ID = 12,
                NameArabic = "اكتمال تقييم الجمال التابعة للمتسابق",
                NameEnglish = "Comelete Camels Review",
                CreatedBy = "5",
                CreatedDate = DateTime.UtcNow,

            });

            context.NotificationTypes.AddOrUpdate(new Models.NotificationType
            {
                ID = 13,
                NameArabic = "رفض طلب الصداقة",
                NameEnglish = "Reject Friend Request ",
                CreatedBy = "5",
                CreatedDate = DateTime.UtcNow,

            });

            context.NotificationTypes.AddOrUpdate(new Models.NotificationType
            {
                ID = 14,
                NameArabic = "استلام رسالة شات",
                NameEnglish = "Receive Chat Message ",
                CreatedBy = "5",
                CreatedDate = DateTime.UtcNow,

            });
            context.NotificationTypes.AddOrUpdate(new Models.NotificationType
            {
                ID = 15,
                NameArabic = "استبدال جمل",
                NameEnglish = "Receive Chat Message ",
                CreatedBy = "5",
                CreatedDate = DateTime.UtcNow,

            });
            context.NotificationTypes.AddOrUpdate(new Models.NotificationType
            {
                ID = 16,
                NameArabic = "تم استبدال الجمل من قبل المتسابق",
                NameEnglish = "Receive Chat Message ",
                CreatedBy = "5",
                CreatedDate = DateTime.UtcNow,

            });
            context.NotificationTypes.AddOrUpdate(new Models.NotificationType
            {
                ID = 17,
                NameArabic = "تم نشر نتائج المسابقة",
                NameEnglish = "publish competition ",
                CreatedBy = "5",
                CreatedDate = DateTime.UtcNow,

            });
            context.NotificationTypes.AddOrUpdate(new Models.NotificationType
            {
                ID = 18,
                NameArabic = "تم اعتماد المسابقة",
                NameEnglish = "competition is finihed by referee boss",
                CreatedBy = "5",
                CreatedDate = DateTime.UtcNow,

            });

        }


        private void AddCamelSpecification(CamelsClub.Data.Context.CamelsClubContext context)
        {
            context.CamelSpecifications.AddOrUpdate(new Models.CamelSpecification
            {
                ID = 1,
                SpecificationEnglish = "Head Size",
                SpecificationArabic = "كبر الراس",
                CreatedBy = "3",
                CreatedDate = DateTime.UtcNow,

            });
            context.CamelSpecifications.AddOrUpdate(new Models.CamelSpecification
            {
                ID = 2,
                SpecificationEnglish = "العرنون",
                SpecificationArabic = " العرنون",
                CreatedBy = "3",
                CreatedDate = DateTime.UtcNow,

            });
            context.CamelSpecifications.AddOrUpdate(new Models.CamelSpecification
            {
                ID = 3,
                SpecificationEnglish = "عرض الخد",
                SpecificationArabic = "عرض الخد",
                CreatedBy = "3",
                CreatedDate = DateTime.UtcNow,

            });

            context.CamelSpecifications.AddOrUpdate(new Models.CamelSpecification
            {
                ID = 4,
                SpecificationEnglish = "سيف اللحي",
                SpecificationArabic = "سيف اللحي",
                CreatedBy = "3",
                CreatedDate = DateTime.UtcNow,

            });

            context.CamelSpecifications.AddOrUpdate(new Models.CamelSpecification
            {
                ID = 5,
                SpecificationEnglish = "طول السبال",
                SpecificationArabic = "طول السبال",
                CreatedBy = "3",
                CreatedDate = DateTime.UtcNow,

            });

            context.CamelSpecifications.AddOrUpdate(new Models.CamelSpecification
            {
                ID = 6,
                SpecificationEnglish = "الأذن",
                SpecificationArabic = "الأذن",
                CreatedBy = "3",
                CreatedDate = DateTime.UtcNow,

            });

            context.CamelSpecifications.AddOrUpdate(new Models.CamelSpecification
            {
                ID = 7,
                SpecificationEnglish = "الرقبة",
                SpecificationArabic = "الرقبة",
                CreatedBy = "3",
                CreatedDate = DateTime.UtcNow,

            });
            context.CamelSpecifications.AddOrUpdate(new Models.CamelSpecification
            {
                ID = 8,
                SpecificationEnglish = "الغارب",
                SpecificationArabic = "الغارب",
                CreatedBy = "3",
                CreatedDate = DateTime.UtcNow,

            });
            context.CamelSpecifications.AddOrUpdate(new Models.CamelSpecification
            {
                ID = 9,
                SpecificationEnglish = "الجنب",
                SpecificationArabic = "الجنب",
                CreatedBy = "3",
                CreatedDate = DateTime.UtcNow,

            });

            context.CamelSpecifications.AddOrUpdate(new Models.CamelSpecification
            {
                ID = 10,
                SpecificationEnglish = "السنام",
                SpecificationArabic = "السنام",
                CreatedBy = "3",
                CreatedDate = DateTime.UtcNow,

            });
            context.CamelSpecifications.AddOrUpdate(new Models.CamelSpecification
            {
                ID = 11,
                SpecificationEnglish = "العيز",
                SpecificationArabic = "العيز",
                CreatedBy = "3",
                CreatedDate = DateTime.UtcNow,

            });
            context.CamelSpecifications.AddOrUpdate(new Models.CamelSpecification
            {
                ID = 12,
                SpecificationEnglish = "الذنب",
                SpecificationArabic = "الذنب",
                CreatedBy = "3",
                CreatedDate = DateTime.UtcNow,

            });
            context.CamelSpecifications.AddOrUpdate(new Models.CamelSpecification
            {
                ID = 13,
                SpecificationEnglish = "العرقوب",
                SpecificationArabic = "العرقوب",
                CreatedBy = "3",
                CreatedDate = DateTime.UtcNow,

            });
            context.CamelSpecifications.AddOrUpdate(new Models.CamelSpecification
            {
                ID = 14,
                SpecificationEnglish = "Color consistency",
                SpecificationArabic = "تناسق اللون",
                CreatedBy = "3",
                CreatedDate = DateTime.UtcNow,

            });

        }
    }
}
