using DataModels;
using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
	public class Chat
	{
		[Key]
		public Guid PlayerTokenId { get; set; }
		public List<Message> Messages { get; set; } = new List<Message>();
	}
}
