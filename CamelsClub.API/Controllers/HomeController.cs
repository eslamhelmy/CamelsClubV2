using CamelsClub.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CamelsClub.API.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var phone = SecurityHelper.Encrypt("admin@camelsclub.com");
            var nid = SecurityHelper.Encrypt("1234567891");
            var pass = SecurityHelper.GetHashedString("Test@123456");
            var user10 = SecurityHelper.Decrypt("9ON5aieGnuJJd8E+DYopUA==");
           // var khateeb = SecurityHelper.Decrypt("FrKA+xonfXDvN4eJqsgfFg==");
           // var mirza = SecurityHelper.Decrypt("MNAvXV+eNZrj3mvC+09+aw==");
           //var phone29 = SecurityHelper.Encrypt("alMzVldlUEpSYUx4MXRoQmZrYTdHNEpENnhVKzZYSXd6d1VjemsrYXlVYz06MjA6NjM3MjAxMTI2NDk2Mjk5MjI1");
           //var phone40 = SecurityHelper.Decrypt("zSE5RpoJ7IMclfFxSSRs0g==");
            ViewBag.Title = "Home Page";
            var dateToday = DateTime.Now;
            var x = dateToday.ToString("yyyy'/'MM'/'dd' 'HH':'mm");
            return View();
        }
    }
}
