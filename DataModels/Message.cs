using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels
{
	public class Message
	{
		public enum MessageSource {
			Player,
			Operator,
		}

		public MessageSource Source { get; set; }
		public DateTime CreationDate { get; set; }
		public bool IsReaded { get; set; }
		public string Text { get; set; }
	}
}
