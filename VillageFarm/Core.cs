using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace VillageFarm
{
	class Core
	{
		public static AccountConfiguration programConfig = GetAccountConfiguration();
		public static ChromeDriver driver = GetChromeDriver(true);
		public void Main()
		{
			var actions = new List<Action>
			{
				new Action{ StartTime=DateTime.Now, ActionBot = ActionType.HandleSession},
				new Action{ StartTime=DateTime.Now.AddSeconds(1), ActionBot = ActionType.Farm},
				new Action{ StartTime=DateTime.Now.AddSeconds(2), ActionBot = ActionType.InstantBuild}
			};

			do
			{
				Action action;
				if (actions.Any())
				{
					action = actions.OrderBy(a => a.StartTime).First();
					if (action.StartTime <= DateTime.Now)
					{
						DoAction(action, actions);
					}
				}

				Thread.Sleep(2000);
			}
			while (true);
		}

		private void DoAction(Action action, List<Action> actions)
		{
			try
			{
				switch (action.ActionBot)
				{
					case ActionType.Farm:
						var farm = new VillageFarmer(driver);
						Task.Run(() => farm.FarmAction()).Wait();
						actions.Add(new Action { StartTime = DateTime.Now.AddMinutes(4.5), ActionBot = ActionType.HandleSession }); ;
						actions.Add(new Action { StartTime = DateTime.Now.AddMinutes(5), ActionBot = ActionType.Farm });
						actions.Remove(action);
						break;
					case ActionType.InstantBuild:
						var instantBuild = new InstantBuilder(driver, actions);
						Task.Run(() => instantBuild.InstantBuildAction()).Wait();
						actions.Remove(action);
						break;
					case ActionType.HandleSession:
						var handleSession = new SessionHandler(programConfig.Username, programConfig.Password, programConfig.World, driver);
						Task.Run(() => handleSession.HandleSessionAction()).Wait();
						actions.Remove(action);
						break;
					default:
						Helper.Log(Helper.LogLevels.ERROR, "Anything action doesn't exist.");
						break;
				}
			}
			catch (WebDriverException)
			{
				driver = GetChromeDriver(true);
			}
		}

		private static AccountConfiguration GetAccountConfiguration()
		{
			try
			{
				IConfiguration configuration = new ConfigurationBuilder()
								.SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
								.AddJsonFile("AccountConfiguration.json")
								.Build();
				return configuration.GetSection("AccountConfig").Get<AccountConfiguration>();
			}
			catch (Exception)
			{
				Helper.Log(Helper.LogLevels.ERROR, "Error in initialize account configuration");
				throw;
			}
		}

		public class AccountConfiguration
		{
			public string Username { get; set; }
			public string Password { get; set; }
			public string World { get; set; }
			public string WebDriverPath { get; set; }
		}

		public static ChromeDriver GetChromeDriver(bool visible)
		{
			try
			{
				var config = GetAccountConfiguration();
				var chromeDriverService = ChromeDriverService.CreateDefaultService(config.WebDriverPath);
				chromeDriverService.HideCommandPromptWindow = true;
				var options = new ChromeOptions();
				options.AddExcludedArgument("enable-logging");
				if (!visible)
					options.AddArgument("--window-position=-32000,-32000");

				var driver = new ChromeDriver(config.WebDriverPath, options);
				driver.Navigate().GoToUrl("https://pl.grepolis.com/");
				return driver;
			}
			catch (Exception ex)
			{
				Helper.Log(Helper.LogLevels.ERROR, $"Error during create chrome driver. Details: {ex}");
				return null;
			}
		}
	}
}
