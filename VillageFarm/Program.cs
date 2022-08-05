using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace VillageFarm
{

	class Program
	{

		static void Main(string[] args)
		{
			new Core().Main();
		}
	}
}
