using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace VillageFarm
{
	class SessionHandler : BasicAction
	{
		private string Username;
		private string Password;
		private string World;
		private int errorCounter = 0;

		public SessionHandler(string username, string password, string world, ChromeDriver driver)
		{
			Username = username;
			Password = password;
			World = world;
			Driver = driver;
		}

		public void HandleSessionAction()
		{
			if (isGoodSession())
			{
				return;
			}
			else
			{
				ReOpenChromeDriver();
			}
		}
		private bool isGoodSession()
		{
			return IsElementPresent(By.XPath("//div[@class='construction_queue_frame']")); //aby zawsze być na ekranie z miastem 
		}

		private void ReOpenChromeDriver()
		{
			Driver.Navigate().GoToUrl("https://pl.grepolis.com/");
			try
			{
				LoginToGame(Username, Password);
				SelectWorld(World);
				ClickCloseAllButton();
			}
			catch (WebDriverException)
			{

			}
			catch (Exception)
			{
				if (errorCounter <= 200)
				{
					errorCounter += 1;
					HandleSessionAction();
				}
			}
		}
	}
}
