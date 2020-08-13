using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using log4net.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsoleTests.src;

namespace ConsoleTests
{
    /// <summary>
    /// This has all the test methods for Intact.
    /// This project is intended to be started from the web application.
    /// </summary>
    [TestClass]
    public class AllTests
    {
        #region Test Fields
        static readonly ILog debugLog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.FullName);
        public string method;
        public static UserMethods user;
        public TestContext Context { get; set; }
        public static TestRunObject TestRun;
        public static TestCaseObject TestCase;
        #endregion

        #region Test Attributes
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            user = new UserMethods(debugLog);
            XmlConfigurator.Configure();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            TestRun = new TestRunObject();
        }
        [TestInitialize]
        public void TestInit()
        {
            TestCase = new TestCaseObject(debugLog);
            TestRun.TestCases.Add(TestCase);
            user = new UserMethods(debugLog);
        }
        [TestCleanup]
        public void TestCleanup()
        {
            Context.WriteLine("THIS IS TESTING THE PRINTING OF CONTEXT IN THE TRACE");
            TestCase.AddTestCaseResult(Context.CurrentTestOutcome, Context.TestName);
            user.Cleanup().DisposeErrorMessages();
            user.Cleanup().CloseDriver();
        }
        [ClassCleanup]
        public static void Cleanup()
        {
            user.Cleanup().WriteFailFile();
            user.Cleanup().SendToDB();
            user.Cleanup().CloseExtraDriverInstances();
        }
        #endregion

        #region Print
        private void Print(string method, string toPrint)
        {
            debugLog.Info(method + " " + toPrint);
        }
        #endregion

        #region All Test Methods
        [TestMethod]
        public void TEST1_1_LOGIN()
        {
            user.Setup().Login();
            user.Cleanup().EndOfTestCheck();
        }
        [TestMethod]
        public void TEST1_2_INZONE()
        {
            user.Setup().Login();
            user.DocumentCollect().InZone();
            user.Cleanup().EndOfTestCheck();
        }
        [TestMethod]
        public void TEST1_3_BATCHREVIEW()
        { //Batch review runs slow      
            user.Setup().Login();
            user.DocumentCollect().BatchReview();
            user.Cleanup().EndOfTestCheck();
        }
        [TestMethod]
        public void TEST1_4_DEFINITIONS()
        {
            user.Setup().Login();
            user.Create().CreateNewDefinition();
            user.Cleanup().EndOfTestCheck();
        }
        [TestMethod]
        public void TEST1_5_TYPES()
        {
            user.Setup().Login();
            user.Create().CreateNewType();
            user.Cleanup().EndOfTestCheck();
        }
        [TestMethod]
        public void TEST1_6_DOCUMENTS()
        {
            user.Setup().Login();
            foreach (var s in user.Create().CreateDocumentWithCheck(2))
            {
                documentIds.Add(s);
            }
            user.Cleanup().EndOfTestCheck();
        }
        [TestMethod]
        public void TEST1_7_SEARCH()
        {
            user.Setup().Login();
            user.SearchRecognize().Search("Default");
            user.Cleanup().EndOfTestCheck();
        }
        [TestMethod]
        public void TEST1_8_RECOGNITION()
        {
            user.Setup().Login();
            user.SearchRecognize().Recognition("DEFAULT DOCUMENT OPTIONS", "DEFAULT DOCUMENT", "lorem");
            user.Cleanup().EndOfTestCheck();
        }
        [TestMethod]
        public void TEST2_1_IPACK()
        { //unfinished
            user.Setup().Login();
            user.Misc().AddToIPack();
            user.Cleanup().EndOfTestCheck();
        }
        [TestMethod]
        public void TEST2_2_LOGOUT()
        {
            user.Setup().Login();
            user.Setup().Logout();
            user.Cleanup().EndOfTestCheck();
        }
        [TestMethod]
        public void TEST2_3_AUDITTRAIL()
        {
            user.Setup().Login();
            user.Misc().AuditTrail();
            user.Cleanup().EndOfTestCheck();
        }
        #endregion
    }
}
