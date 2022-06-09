using DataModels;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    public class ChatController : Controller
    {
		private readonly Models.AppContext context;

		public ChatController(Models.AppContext context) {
			this.context = context;
		}

        public IActionResult Index() {
			return View(context.Messages.AsEnumerable().DistinctBy(m => m.ChatId));
		}

		public IActionResult Chat(Guid chatId) {
			return View(new Tuple<Guid, IEnumerable<Message>>(chatId, context.Messages.AsEnumerable().Where(m => m.ChatId == chatId)));
		}

		public IActionResult PlayerChat() {
			return View();
		}

		public IActionResult ChatHistory(Guid chatId) {
			return Json(context.Messages.AsEnumerable().Where(m => m.ChatId == chatId).ToArray());
		}
	}
}
