using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static VillageFarm.Helper;

namespace VillageFarm
{
	class BasicAction
	{
		protected static IWebDriver Driver;
		protected bool IsElementPresent(By by)
		{
			try
			{
				Driver.FindElement(by);
				return true;
			}
			catch (NoSuchElementException)
			{
				//Helper.Log(Helper.LogLevels.ERROR, $"Can't find: {by}");
				return false;
			}
			catch (WebDriverException)
			{
				return false;
			}
		}

		protected void LoginToGame(string username, string password)
		{
			try
			{
				IWebElement usernameInput = Driver.FindElement(By.Id("login_userid"));
				IWebElement passwordInput = Driver.FindElement(By.Id("login_password"));
				IWebElement loginButton = Driver.FindElement(By.Id("login_Login"));

				usernameInput.SendKeys(username);
				passwordInput.SendKeys(password);
				loginButton.Click();
				Thread.Sleep(RandomSleepLength());
				Helper.Log(Helper.LogLevels.INFO, "Successful Login");
			}
			catch(Exception)
			{
				Helper.Log(Helper.LogLevels.INFO, "Error in login to game");
			}
		}

		protected void SelectWorld(string world)
		{
			try
			{
				Driver.FindElement(By.XPath($"//*[text()='{world}']")).Click();
				Thread.Sleep(RandomSleepLength());
				//int index = driver.PageSource.IndexOf("csrfToken");
				//idSession = driver.PageSource.Substring(index + 12, 40);
				Helper.Log(Helper.LogLevels.INFO, $"Successful Select World");
				ShowInformationAboutTown();
			}
			catch (Exception ex)
			{
				Helper.Log(Helper.LogLevels.CHGW, $"Error in SelectWorld. Details: {ex}");
				throw;
			}
		}

		protected void ShowInformationAboutTown()
		{
			var wood = Driver.FindElement(By.XPath("//div[contains(@class,'indicator wood')]")).Text;
			var stone = Driver.FindElement(By.XPath("//div[contains(@class,'indicator stone')]")).Text;
			var iron = Driver.FindElement(By.XPath("//div[contains(@class,'indicator iron')]")).Text;
			var population = Driver.FindElement(By.XPath("//div[contains(@class,'indicator population')]")).Text;
			var town = GetCurrentTownName();
			Helper.Log(Helper.LogLevels.INFO, $"Town: {town}. Resources: Wood: {wood}, Stone: {stone}, Iron: {iron}, Population: {population}");
		}

		protected string GetCurrentTownName()
		{
			try
			{
				return Driver.FindElement(By.XPath("//div[contains(@class,'town_name')]")).Text;
			}
			catch (Exception)
			{
				return String.Empty;
			}
		}

		protected void ClickCloseAllButton()
		{
			try
			{
				Driver.FindElement(By.XPath("//div[@class='close_all']")).Click();
				Thread.Sleep(RandomSleepLength());
			}
			catch (Exception)
			{
			}
		}

		protected PremiumAddons GetPremiumAddons()
		{
			try
			{
				var isCuratorActive = IsElementPresent(By.XPath("//div[@class='advisor advisors22x22 curator_active']"));
				var isTraderActive = IsElementPresent(By.XPath("//div[@class='advisor advisors22x22 trader_active']"));
				var isPriestActive = IsElementPresent(By.XPath("//div[@class='advisor advisors22x22 priest_active']"));
				var isCommanderActive = IsElementPresent(By.XPath("//div[@class='advisor advisors22x22 commander_active']"));
				var isCaptainActive = IsElementPresent(By.XPath("//div[@class='advisor advisors22x22 captain_active']"));

				return new PremiumAddons(isCuratorActive, isTraderActive, isPriestActive, isCommanderActive, isCaptainActive);
			}
			catch (Exception)
			{
				return new PremiumAddons(false, false, false, false, false);
			}
		}

		protected static int RandomSleepLength() // different clicks time (protection)
		{
			var randGenerator = new Random();
			return 1000 + randGenerator.Next(100, 1200);
		}
	}
}
