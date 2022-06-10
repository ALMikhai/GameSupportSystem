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
			var uniqueChats = context.Messages.AsEnumerable().DistinctBy(m => m.ChatId).ToList();
			return View(uniqueChats.Select(c => new Tuple<Message, int>(c, NumOfUnreadMessages(c.ChatId, Message.SourceType.Player))));
		}

		public IActionResult Chat(Guid chatId) {
			var messages = context.Messages.Where(m => m.ChatId == chatId);
			return base.View(new Tuple<Guid, IEnumerable<Message>>(chatId, messages));
		}

		public IActionResult PlayerChat() {
			return View();
		}

		public IActionResult ChatHistory(Guid chatId) {
			return Json(context.Messages.AsEnumerable().Where(m => m.ChatId == chatId).ToArray());
		}

		public ActionResult<int> NumOfUnreadOperatorMessages(Guid chatId) {
			return NumOfUnreadMessages(chatId, Message.SourceType.Operator);
		}

		private int NumOfUnreadMessages(Guid chatId, Message.SourceType sourceType) {
			var messages = context.Messages.Where(m => m.ChatId == chatId);
			return messages.Count(m => m.Type == sourceType && m.IsReaded == false);
		}
	}
}
