using System.Diagnostics;
using System.Collections.Generic;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using log4net;
namespace ConsoleTests
{
    /// <summary>
    /// Test Case containing Data at the Test Case level and functions of a Test Case
    /// </summary>
    public class TestCaseObject
    {
        private ILog debugLog;
        public int TestRunId { get; set; }
        public int TestCaseId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string TestName { get; set; }
        public int TestStatus { get; set; }
        public string ImagePath { get; set; }
        public List<DocumentObject> Documents { get; set; }

        public TestCaseObject(ILog debugLog)
        {
            this.debugLog = debugLog;
            foreach (Process p in Process.GetProcessesByName("Intact"))
            {
                p.Kill();
            }
            CreatedDate = DateTime.Now;
        }
        public void AddTestCaseResult(UnitTestOutcome result, string testName)
        {
            TestName = testName;
            if (result == UnitTestOutcome.Inconclusive)
            {
                TestStatus = 1;
                Print("Interrupted *****************************************");
            }
            else if (result == UnitTestOutcome.Passed)
            {
                TestStatus = 2;
                Print("PASSED *****************************************");
            }
            else
            {
                TestStatus = 3;
                //imagePaths.Add(user.Cleanup().TakeScreenshot(TestContext.TestName) + ".PNG");
                Print("FAILED *****************************************");
            }
        }

        public void Print(string toPrint, string method = "")
        {
            debugLog.Info(method + " " + toPrint);
        }
    }
}