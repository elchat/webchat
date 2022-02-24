using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Chat.Models;

namespace Chat.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        public static List<MessageViewModel> Messages = new List<MessageViewModel>();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult getData(DateTime? startDate, DateTime? endDate, string user)
        {
            startDate = (startDate == null) ? new DateTime(1970, 1, 1) : startDate;
            endDate = (endDate == null) ? new DateTime(3000, 1, 1) : endDate;
            var msgs = Messages.Where(m => m.DateTime >= startDate && m.DateTime <= endDate);
            return Json(user.IsEmpty() ? msgs : msgs.Where(m => m.UserName.Equals(user)));
        }

        public JsonResult getUsers()
        {
            return Json(Messages.Select(u => u.UserName).Distinct().ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}