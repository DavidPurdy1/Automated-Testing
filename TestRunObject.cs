using System.Diagnostics;
using System.Collections.Generic;
using System;
using System.Configuration;
namespace ConsoleTests
{
    /// <summary>
    /// Test Run containing Data at the Test Run level and functions of a Test Run
    /// <para>Each Test Run can contain multiple Test Cases.</para>
    /// </summary>
    public class TestRunObject
    {
        public int TestRunId { get; set; }
        public int TestsFailed { get; set; }
        public int TestsPassed { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ApplicationName { get; set; }
        public string ApplicationVersion { get; set; }
        public List<TestCaseObject> TestCases { get; set; }
        public TestRunObject()
        {
            CreatedDate = DateTime.Now;
            CreatedBy = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

            var AppInfo = FileVersionInfo.GetVersionInfo(ConfigurationManager.AppSettings.Get("IntactPath"));
            ApplicationVersion = AppInfo.FileVersion;
            ApplicationName = AppInfo.ProductName;
        }
    }
}