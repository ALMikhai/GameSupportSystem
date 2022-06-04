using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace DataModels
{
	public class Player
	{
		public string DeviceId { get; set; } = GetDeviceMacId();

		// Validation - must be from 1 to 16 symbols length.
		public string Nickname { get; set; } = string.Empty;


		public static string GetDeviceMacId() {
			var networkInterface = NetworkInterface.GetAllNetworkInterfaces()
					.FirstOrDefault(q => q.OperationalStatus == OperationalStatus.Up);
			if (networkInterface == null) {
				return string.Empty;
			}
			return BitConverter.ToString(networkInterface.GetPhysicalAddress().GetAddressBytes());
		}
	}
}
