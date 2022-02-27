using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.WebPages;
using Chat.Data;
using Chat.Models;

namespace Chat.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly ApplicationContext _context;

        public ChatController()
        {
            _context = new ApplicationContext();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> getData(DateTime? startDate, DateTime? endDate, string user)
        {
            startDate = (startDate == null) ? new DateTime(1970, 1, 1) : startDate;
            endDate = (endDate == null) ? new DateTime(3000, 1, 1) : endDate;
            var msgs = await (user.IsEmpty()
                ? GetAllMessages().Where(m => m.DateTime >= startDate && m.DateTime <= endDate).ToListAsync()
                : GetAllMessages()
                    .Where(m => m.DateTime >= startDate && m.DateTime <= endDate && m.UserName.Equals(user))
                    .ToListAsync());
            return Json(user.IsEmpty() ? msgs : msgs.Where(m => m.UserName.Equals(user)));
        }

        public async Task<JsonResult> getUsers()
        {
            var users = await GetAllUsers().Select(u => u.UserName).ToListAsync();
            return Json(users, JsonRequestBehavior.AllowGet);
        }

        public IQueryable<ApplicationUser> GetAllUsers()
        {
            return _context.Users.AsQueryable();
        }

        public IQueryable<ChatMessage> GetAllMessages()
        {
            return _context.ChatMessages.AsQueryable();
        }
    }
}