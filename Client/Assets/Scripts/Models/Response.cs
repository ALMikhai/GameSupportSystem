using System;

namespace Models
{
	/// <summary>
	/// Result of registration request.
	/// </summary>
	public class RegistrationResponse
	{
		/// <summary>
		/// Type of registration response.
		/// </summary>
		public enum ResponseType
		{
			Error,
			Success,
		}

		/// <summary>
		/// Type of registration response.
		/// </summary>
		public ResponseType Type { get; set; }

		/// <summary>
		/// Error message. Empty if Type is equal Success.
		/// </summary>
		public string ErrorMessage { get; set; } = string.Empty;

		/// <summary>
		/// Player access token. Not valid if Type is Error.
		/// </summary>
		public Guid AccessToken { get; set; }
	}


	/// <summary>
	/// Result of login request.
	/// </summary>
	public class LoginResponse
	{
		/// <summary>
		/// Type of login response.
		/// </summary>
		public enum ResponseType
		{
			Error,
			Succes,
		}

		/// <summary>
		/// Type of login response.
		/// </summary>
		public ResponseType Type { get; set; }

		/// <summary>
		/// Error message. Empty if Type is equal Success.
		/// </summary>
		public string ErrorMessage { get; set; } = string.Empty;

		/// <summary>
		/// Player access token. Not valid if Type is Error.
		/// </summary>
		public Guid AccessToken { get; set; }
	}
}
