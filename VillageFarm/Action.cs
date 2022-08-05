using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillageFarm
{
	public enum ActionType
	{
		Farm,
		InstantBuild,
		HandleSession
	}
	public class Action
	{
		public DateTime StartTime { get; set; }
		public ActionType ActionBot { get; set; }
	}
}
