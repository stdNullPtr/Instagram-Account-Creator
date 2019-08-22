using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Threading;

namespace InstagramNameChecker
{
    static class SeleniumHelper
    {
        internal static void CreateInstagramProfiles(List<string> freeUsernames)
        {
            string[] arguments =
                {
                "--disable-extensions",
                "--profile-directory=Default",
                "--incognito",
                "--disable-plugins-discovery",
                "--start-maximized"
            };

            foreach (string username in freeUsernames)
            {
                Console.WriteLine($"Creating account: '{username}' with password: '{Utils.DefaultPassForNewAccounts}' ...");

                ChromeOptions options = new ChromeOptions();

                ChromeDriverService service = null;
                options.AddArguments(arguments);
                try
                {
                    service = ChromeDriverService.CreateDefaultService(@"chromedriver_win32");
                }
                catch (Exception)
                {
                    Console.WriteLine("ERROR: 'chromedriver_win32/chrome_driver.exe' not found!");
                    Console.WriteLine("Aborting...");
                    Thread.Sleep(6000);
                    Environment.Exit(2);
                }
                service.SuppressInitialDiagnosticInformation = true;
                service.HideCommandPromptWindow = true;

                ChromeDriver cd = new ChromeDriver(service, options);
                cd.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                cd.Url = @"https://www.instagram.com/accounts/emailsignup/";
                cd.Navigate();

                int uniqueNums = Utils.GetRandomInt(10, 99);
                int uniqueNums2 = Utils.GetRandomInt(24, 54);
                string fullName = $"{Utils.GetRandomString(5)} {Utils.GetRandomString(7)}";

                IWebElement e = cd.FindElementByName("emailOrPhone");
                e.Click();
                e.SendKeys($"define{uniqueNums}telynotfa{uniqueNums2}ke@gmail.com");
                Thread.Sleep(1000);

                e = cd.FindElementByName("fullName");
                e.Click();
                e.SendKeys(fullName);
                Thread.Sleep(1200);

                e = cd.FindElementByName("username");
                e.Click();
                e.SendKeys(username);
                Thread.Sleep(1300);

                e = cd.FindElementByName("password");
                e.Click();
                e.SendKeys(Utils.DefaultPassForNewAccounts);
                Thread.Sleep(1000);

                e = cd.FindElementByXPath(@"//*[@id=""react-root""]/section/main/div/article/div/div[1]/div/form/div[2]/div[2]");
                e.Click();
                Thread.Sleep(1100);

                e = cd.FindElementByXPath(@"//*[@id=""react-root""]/section/main/div/article/div/div[1]/div/form/div[7]/div/button");
                e.Click();
                Thread.Sleep(1200);

                // if we didnt pass the creation field by any reason, these ones will not exists
                try
                {
                    e = cd.FindElementByName("ageRadio");
                    e.Click();
                    Thread.Sleep(1300);

                    e = cd.FindElementByXPath(@"//html/body/div[3]/div/div[3]/div/button");
                    e.Click();
                }
                catch (Exception)
                {
                    Console.WriteLine($"{username} cannot be created! Try creating it manually.");
                    cd.Quit();
                    continue;
                }

                string[] lines =
                {
                    $"User: {username}",
                    $"Password: {Utils.DefaultPassForNewAccounts}",
                    $"Full name: {fullName}",
                    $"Email(fake): define{uniqueNums}telynotfa{uniqueNums2}ke@gmail.com"
                };
                System.IO.File.WriteAllLines($"USER_{username}.txt", lines);

                Thread.Sleep(5000);

                cd.Quit();

                /// TODO create only 1 account for now
                break;
            }
        }
    }
}
