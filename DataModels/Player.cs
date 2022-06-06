using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace DataModels
{
	public class Player
	{
		[Key]
		public string DeviceId { get; set; }
		public string Nickname { get; set; } = string.Empty;
	}
}
