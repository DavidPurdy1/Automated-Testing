﻿using log4net;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Winium;
using System;
using System.Configuration;

namespace ConsoleTests.src {
    /// <summary>
    /// Entry point for writing tests
    /// </summary>
    public class UserMethods {
        #region fields
        readonly DesktopOptions options = new DesktopOptions();
        readonly WiniumDriver driver;
        readonly WiniumMethods m;
        readonly ILog debugLog;
        readonly Actions action;
        #endregion

        #region Setup
        public UserMethods(ILog log) {
            debugLog = log;
            options.ApplicationPath = ConfigurationManager.AppSettings.Get("IntactPath");
            driver = new WiniumDriver(ConfigurationManager.AppSettings.Get("DriverPath"), options);
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(double.Parse(ConfigurationManager.AppSettings.Get("ImplicitWait"))));
            action = new Actions(driver);
            m = new WiniumMethods(driver, debugLog);
        }
        #endregion

        #region
        /// <summary>
        /// Methods that happen at the beginning of a test
        /// </summary>
        public IntactSetup Setup() {
            return new IntactSetup(m); 
        }
        /// <summary>
        /// Class contains methods to create new document, definition, or type
        /// </summary>
        public Create Create() {
            return new Create(m, action, debugLog); 
        }
        /// <summary>
        /// Class containing methods used at the end of a test case or test run.
        /// </summary>
        public Cleanup Cleanup() {
            return new Cleanup(m, debugLog); 
        }
        /// <summary>
        /// Methods that use the Document Collector, BatchReview and InZone
        /// </summary>
        public DocumentCollect DocumentCollect() {
            return new DocumentCollect(m, action, debugLog); 
        }
        /// <summary>
        /// Methods for Search and Recognize Feature
        /// </summary>
        public SearchRecognize SearchRecognize() {
            return new SearchRecognize(m, action, debugLog);
        }
        /// <summary>
        /// Methods that arent typically ran but are good for regression
        /// <para>Warning: Some of these are not entirely implemented</para>
        /// </summary>
        public Misc Misc() {
            return new Misc(m, action,debugLog); 
        }
        #endregion
        public void Print(string method, string toPrint) {
            debugLog.Info(method + " " + toPrint);
        }
    }
}