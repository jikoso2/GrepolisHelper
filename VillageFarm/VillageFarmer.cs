using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;

using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using static VillageFarm.Helper;

namespace VillageFarm
{
	class VillageFarmer : BasicAction
	{

		public VillageFarmer(ChromeDriver driver)
		{
			Driver = driver;
		}

		internal void FarmAction()
		{
			PremiumAddons advisors = GetPremiumAddons();

			if (advisors.Curator)
			{
				PremiumFarmProcedure();
			}
			else
			{
				PremiumFarmProcedure();
				//NormalFarmProcedure();
			}
			ClickCloseAllButton();
		}

		private void PremiumFarmProcedure()
		{
			SelectPremiumToolBar();
			SelectPremiumFarmer();
			SelectAllVilages();
			SubmitFarm();
		}
		private void NormalFarmProcedure()
		{
			Driver.FindElement(By.XPath("//div[@name='island_view']")).Click();

			var firstTown = GetCurrentTownName();

			do
			{
				try
				{
					var farms = Driver.FindElements(By.XPath("//a[@data-same_island='true']"));
					foreach (var farm in farms)
					{
						farm.Click();
						Driver.FindElements(By.XPath("//div[@class='card_click_area']")).FirstOrDefault().Click();
						if (IsElementPresent(By.XPath("//div[@class='btn_confirm button_new']")))
							Driver.FindElement(By.XPath("//div[@class='btn_confirm button_new']")).Click();
					}
					ClickCloseAllButton();
				}
				catch (Exception)
				{
					ClickCloseAllButton();
					Driver.FindElement(By.XPath("//div[@class='btn_next_town button_arrow right']")).Click();
				}
			}
			while (firstTown != GetCurrentTownName());

			Helper.Log(Helper.LogLevels.WARNING, "NormalFarmProcedure not implementation");
			//Driver.Close();
		}


		private void CheckPremiumExchange()
		{
			try
			{
				Driver.FindElement(By.XPath("//div[@data-option_id='3']")).Click();
				Driver.FindElement(By.XPath("//div[@details='1']")).Click();
				var count = Driver.FindElements(By.XPath("//tr[@class='premium_exchange']")).Count;

				if (count > 3)
				{
					Helper.Log(Helper.LogLevels.INFO, "GOLD COINS AVAILABLE");
				}
			}
			catch (Exception)
			{
				HandleException(MethodBase.GetCurrentMethod().Name);
			}

		}

		private void SelectPremiumToolBar()
		{
			Actions action = new Actions(Driver);
			try
			{
				if (IsElementPresent(XPaths.SelectPremiumToolBar))
				{
					IWebElement we = Driver.FindElement(XPaths.SelectPremiumToolBar);
					action.MoveToElement(we).Build().Perform();
					Thread.Sleep(RandomSleepLength());
				}
			}
			catch (Exception)
			{
				HandleException(MethodBase.GetCurrentMethod().Name);
			}
		}
		internal class XPaths
		{
			internal static By SelectPremiumToolBar = By.XPath("//div[@class='toolbar_button premium']");
			internal static By SelectPremiumFarmer = By.XPath("//li[@class='farm_town_overview']");
			internal static By SelectAllVillages = By.XPath("//a[@class='checkbox select_all href=']");
			internal static By SubmitFarmWithFullStorages = By.XPath("//div[@class='btn_confirm button_new']");
		}

		private void SelectPremiumFarmer()
		{
			try
			{
				if (IsElementPresent(XPaths.SelectPremiumToolBar))
				{
					Driver.FindElement(XPaths.SelectPremiumFarmer).Click();
					Thread.Sleep(RandomSleepLength());
				}
			}
			catch (Exception)
			{
				HandleException(MethodBase.GetCurrentMethod().Name);
			}
		}

		private void SelectAllVilages()
		{
			try
			{
				if (IsElementPresent(XPaths.SelectAllVillages))
				{
					Driver.FindElement(XPaths.SelectAllVillages).Click(); // tu się wyjeboł :(
					Thread.Sleep(RandomSleepLength());
				}
			}
			catch (Exception)
			{
				HandleException(MethodBase.GetCurrentMethod().Name);
			}
		}

		private void SubmitFarm()
		{
			try
			{
				if (IsElementPresent(By.Id("fto_claim_button")))
				{
					Driver.FindElement(By.Id("fto_claim_button")).Click();
					Thread.Sleep(RandomSleepLength());
					SubmitFarmWithFullStorages();
				}
			}
			catch (Exception)
			{
				HandleException(MethodBase.GetCurrentMethod().Name);
			}
		}

		private void SubmitFarmWithFullStorages()
		{
			try
			{
				if (IsElementPresent(XPaths.SubmitFarmWithFullStorages)) 
				{
					Driver.FindElement(XPaths.SubmitFarmWithFullStorages).Click();
				}
			}
			catch (Exception)
			{
			}
			Helper.Log(Helper.LogLevels.INFO, $"Successful village farm");
			Thread.Sleep(RandomSleepLength());
		}

		private void HandleException(string fcnName)
		{
			Thread.Sleep(RandomSleepLength());
			Helper.Log(Helper.LogLevels.ERROR, $"Error in village farm");
		}


	}
}
