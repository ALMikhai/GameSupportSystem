using DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Services.Redis;

namespace Server.Controllers
{
	[Authorize(Roles = "Operator, Observer")]
    public class ChatController : Controller
    {
		private readonly Models.AppContext context;
		private readonly IRedisCacheService redisCache;

		public ChatController(Models.AppContext context, IRedisCacheService redisCache) {
			this.context = context;
			this.redisCache = redisCache;
		}

        public IActionResult Index() {
			var uniqueChats = context.Messages.AsEnumerable().DistinctBy(m => m.ChatId).ToList();
			return View(uniqueChats.Select(c => new Tuple<Message, int>(c, NumOfUnreadMessages(c.ChatId, Message.SourceType.Player))));
		}

		public IActionResult Chat(Guid chatId) {
			var messages = context.Messages.Where(m => m.ChatId == chatId);
			var chatModel = new Tuple<Guid, IEnumerable<Message>>(chatId, messages);
			if (User.IsInRole("Observer")) {
				return View("OnlyViewChat", chatModel);
			} else {
				return base.View(chatModel);
			}
		}

		public IActionResult ChatHistory(Guid chatId) {
			return Json(context.Messages.AsEnumerable().Where(m => m.ChatId == chatId).ToArray());
		}

		public ActionResult<int> NumOfUnreadOperatorMessages(Guid chatId) {
			int result;
			if (redisCache.Get<bool>($"{chatId}-numUnreadMessagesCached")) {
				result = redisCache.Get<int>($"{chatId}-numUnreadMessages");
			} else {
				var numUnreadMessage = NumOfUnreadMessages(chatId, Message.SourceType.Operator);
				redisCache.Set<int>($"{chatId}-numUnreadMessages", numUnreadMessage);
				redisCache.Set<bool>($"{chatId}-numUnreadMessagesCached", true);
				result = numUnreadMessage;
			}
			return result;
		}

		private int NumOfUnreadMessages(Guid chatId, Message.SourceType sourceType) {
			var messages = context.Messages.Where(m => m.ChatId == chatId);
			return messages.Count(m => m.Type == sourceType && m.IsReaded == false);
		}
	}
}
