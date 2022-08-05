using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VillageFarm
{
	class InstantBuilder : BasicAction
	{
		private List<Action> Actions;
		public InstantBuilder(ChromeDriver driver, List<Action> actions)
		{
			Driver = driver;
			Actions = actions;
		}

		internal void InstantBuildAction()
		{
			CheckFastClick();
		}
		private void CheckFastClick()
		{
			Actions action = new Actions(Driver);
			try
			{
				var div = Driver.FindElements(By.XPath("//div[@class='construction_queue_sprite frame']"));
				action.MoveToElement(div.FirstOrDefault()).Build().Perform();
				var time = TimeSpan.Parse(Driver.FindElement(By.XPath("//div[@class='cell']")).Text);
				Helper.Log(Helper.LogLevels.INFO, $"Time to instant build: {time}");
				if (TimeSpan.Compare(time, new TimeSpan(0, 5, 0)) == -1)
				{
					Driver.FindElement(By.XPath("//div[@class='btn_time_reduction button_new js-item-btn-premium-action js-tutorial-queue-item-btn-premium-action type_building_queue type_instant_buy instant_buy type_free']")).Click();
					CheckFastClick();
				}
				else
				{
					Actions.Add(new Action { StartTime = DateTime.Now.Add(time).AddMinutes(-5), ActionBot = ActionType.InstantBuild });
				}
				Thread.Sleep(RandomSleepLength());
			}
			catch (NoSuchElementException)
			{
				ExceptionProcedure(true);
			}
			catch (ArgumentException)
			{
				ExceptionProcedure(false);
			}
		}
		private void ExceptionProcedure(bool queueIsEmpty)
		{
			if (queueIsEmpty)
			{
				var nextCheck = DateTime.Now.AddMinutes(1);
				Helper.Log(Helper.LogLevels.INFO, $"Queue is empty. Next check: {nextCheck}");
				Actions.Add(new Action { StartTime = nextCheck, ActionBot = ActionType.InstantBuild });
			}
			else
				Helper.Log(Helper.LogLevels.ERROR, $"Error in Instant Builder.");
		}
	}
}
