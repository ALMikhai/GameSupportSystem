using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels
{
	public class RegistrationResponse
	{
		public enum ResponseType
		{
			Error,
			Succes,
		}

		public ResponseType Type { get; set; }
		public string ErrorMessage { get; set; } = string.Empty;
		public Guid AccessToken { get; set; }
	}

	public class LoginResponse
	{
		public enum ResponseType
		{
			Error,
			Succes,
		}

		public ResponseType Type { get; set; }
		public string ErrorMessage { get; set; } = string.Empty;
		public Guid AccessToken { get; set; }
	}
}
