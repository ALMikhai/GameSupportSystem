using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels
{
	public class Message
	{
		public enum SourceType {
			Player,
			Operator,
		}

		public int Id { get; set; }
		public SourceType Type { get; set; }
		public string Name { get; set; }
		public DateTime CreationDate { get; set; } = DateTime.Now;
		public bool IsReaded { get; set; } = false;
		public string Text { get; set; } = string.Empty;
	}
}
