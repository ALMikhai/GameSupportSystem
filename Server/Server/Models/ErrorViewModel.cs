namespace Server.Models
{
	/// <summary>
	/// Model for error page.
	/// </summary>
	public class ErrorViewModel
	{
		/// <summary>
		/// Text of error.
		/// </summary>
		public string? RequestId { get; set; }

		/// <summary>
		/// True if RequestId is valid.
		/// </summary>
		public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
	}
}