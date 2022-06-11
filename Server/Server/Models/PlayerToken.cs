namespace Server.Models
{
	/// <summary>
	/// Player token model.
	/// </summary>
	public class PlayerToken
	{
		/// <summary>
		/// Unique player id. It's the chat id for sending messages.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Player device id.
		/// </summary>
		public string PlayerDeviceId { get; set; }
	}
}
