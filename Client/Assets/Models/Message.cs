using System;

namespace Models
{
	/// <summary>
	/// Model describing the message from the server.
	/// </summary>
	public class Message
	{
		/// <summary>
		/// Enumeration of message sources.
		/// </summary>
		public enum SourceType {
			Player,
			Operator,
		}

		/// <summary>
		/// Unique message id.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Unique chat id.
		/// </summary>
		public Guid ChatId { get; set; }

		/// <summary>
		/// Type of source.
		/// </summary>
		public SourceType Type { get; set; }

		/// <summary>
		/// The name of the message creator.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Creation date.
		/// </summary>
		public DateTime CreationDate { get; set; } = DateTime.Now;

		/// <summary>
		/// True if message readed.
		/// </summary>
		public bool IsReaded { get; set; } = false;

		/// <summary>
		/// Message text.
		/// </summary>
		public string Text { get; set; } = string.Empty;
	}
}
