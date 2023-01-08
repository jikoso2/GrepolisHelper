# Grepolis Helper
> 
This application allows you to automate boring processes in grepolis game. It makes game easier.

## Table of contents
* [General info](#general-info)
* [Technologies](#technologies)
* [Features](#features)
* [Status](#status)
* [Console App](#Console App)
* [Contact](#contact)

## General info

Grepolis helper is application supporting grepolis gameplay
It saves time. 
I can grow my empire much faster. 

## Technologies
* .NET Core 5.0
* ChromeDriver 109.0.5414.25
* Selenium 4.0.0

## Code Examples
LoginToGame using Selenium:
```
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
			catch (Exception)
			{
				Helper.Log(Helper.LogLevels.INFO, "Error in login to game");
			}
		}
```
Main loop of program, performing activities such as village farm, instant building, handle session
```
private void DoAction(Action action, List<Action> actions)
		{
			try
			{
				switch (action.ActionBot)
				{
					case ActionType.Farm:
						var farm = new VillageFarmer(driver);
						Task.Run(() => farm.FarmAction()).Wait();
						actions.Add(new Action { StartTime = DateTime.Now.AddMinutes(7.5), ActionBot = ActionType.HandleSession }); ;
						actions.Add(new Action { StartTime = DateTime.Now.AddMinutes(8), ActionBot = ActionType.Farm });
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
```

## Features
List of features ready and TODOs for future development
* Instant building
* Automatic village farm
* Handle session
* Configuration helper using .json file

To-do list:
* Automatic building
* Military recruitment

## Status
Project is: in progress (current version can be used)
Possibility of modification for the user.

## Console App
User can monitor activities in console logs.
![image](https://user-images.githubusercontent.com/69644118/211190782-0ea6787d-1dfa-4251-a55b-48cf4656580c.png)

## Contact
Created by Jarosław Czerniak [@jikoso2](https://github.com/jikoso2) - See my GitHub!
