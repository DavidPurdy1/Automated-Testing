﻿using OpenQA.Selenium;
using OpenQA.Selenium.Winium;
using System;
using System.Configuration;
using System.Threading;

namespace ConsoleTests.src
{
    /// <summary>
    /// Methods that happen at the beginning of a test
    /// </summary>
    public class IntactSetup
    {

        IWebElement window;
        readonly WiniumMethods m;

        public IntactSetup(WiniumMethods m)
        {
            this.m = m;
        }
        /// <summary> Login to Intact: Has to be ran before any other testcase</summary>
        public void Login()
        {
            bool needToSetDB = ConfigurationManager.AppSettings.Get("setDataBase") == "true";
            Thread.Sleep(10000);
            m.SendKeys(By.Name(""), "admin");
            if (needToSetDB)
            {
                SetDatabaseInformation();
            }
            m.Click(By.Name("&Logon"));
            Thread.Sleep(2000);
        }
        public void Logout()
        {
            window = m.Locate(By.Name("&Intact"), m.Locate(By.Name("radMenu1")));
            m.Click(By.Name("Log Out"), window);
        }
        private void SetDatabaseInformation()
        {
            throw new NotImplementedException();
            // m.Click(By.Name("&Settings.."));
            // m.SendKeys(By.Name(""), @"(local)\INTACT");
            // m.SendKeys(By.Name(""), "{TAB}");
            // m.SendKeys(By.Name(""), "{TAB}");
            // m.SendKeys(By.Name(""), "{ENTER}");
        }
        private void ConnectToRemoteDesktop(DesktopOptions options, string serverName)
        {
            throw new NotImplementedException();
            //method = MethodBase.GetCurrentMethod().Name;
            //Print(method, "Started");

            //options.ApplicationPath = ConfigurationManager.AppSettings.Get("RemoteDesktop");
            //driver = new WiniumDriver(driverPath, options);
            //m.sendKeysByName("Remote Desktop Connection", serverName);
            //m.clickByName("Connect");

            //print(method, "Finished");
        }
    }
}
