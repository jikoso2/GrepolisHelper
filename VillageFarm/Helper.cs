using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillageFarm
{
	class Helper
	{
		public static void Log(LogLevels logLevel, string information)
		{
			Console.WriteLine($"{DateTime.Now} - [{logLevel}] - {information}");
		}

		public enum LogLevels
		{
			ERROR,
			INFO,
			WARNING,
			CHGW
		}

		public class PremiumAddons
		{
			public PremiumAddons(bool curator, bool trader, bool priest, bool commander, bool captain)
			{
				Curator = curator;
				Trader = trader;
				Priest = priest;
				Commander = commander;
				Captain = captain;
			}

			public bool Curator { get; set; }
			public bool Trader { get; set; }
			public bool Priest { get; set; }
			public bool Commander { get; set; }
			public bool Captain { get; set; }
		}

	}
}
