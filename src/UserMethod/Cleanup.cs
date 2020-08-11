using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConsoleTests.src
{
    /// <summary>
    /// Class containing methods used at the end of a test case or test run.
    /// </summary>
    public class Cleanup
    {
        readonly WiniumMethods m;
        string method = "";
        readonly ILog debugLog;

        public Cleanup(WiniumMethods m, ILog debugLog)
        {
            this.m = m;
            this.debugLog = debugLog;
        }
        public string TakeScreenshot(string testName, string folderPath = "")
        {
            if (folderPath.Length < 2) { folderPath = ConfigurationManager.AppSettings.Get("AutomationScreenshots"); }

            //YYYY-MM-DD__HH-MM-SS
            string dateAndTime = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + "__"
                + DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + DateTime.Now.Second.ToString();

            //creates file, stores screenshot in path
            string path = Path.Combine(folderPath, testName + "_" + dateAndTime);
            m.GetScreenshot().SaveAsFile(path + ".PNG", ImageFormat.Png);
            return path;
        }
        ///<summary>
        ///<para>Writes .txt for the DataExporter to parse</para>
        ///<para>Edits to this must be reflected in the data exporter class</para>
        ///</summary>
        public void WriteFailFile(List<string> testsFailedNames, List<string> testsPassedNames, List<string> testsInconclusiveNames, List<string> imagePaths)
        {
            method = MethodBase.GetCurrentMethod().Name;
            Print(method, "Started");

            //YYYY-MM-DD__HH-MM-SS
            string dateAndTime = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + "__"
                + DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + DateTime.Now.Second.ToString();

            //writes the result file information
            using (StreamWriter file =
            new StreamWriter(ConfigurationManager.AppSettings.Get("TestResultFiles") + dateAndTime + ".txt", false))
            {
                var versionInfo = FileVersionInfo.GetVersionInfo(ConfigurationManager.AppSettings.Get("IntactPath"));
                string version = versionInfo.FileVersion;
                file.WriteLine("");
                file.WriteLine(DateTime.Now.ToString());
                file.WriteLine("Tests failed| " + testsFailedNames.Count.ToString());
                file.WriteLine("Tests passed| " + testsPassedNames.Count.ToString());
                file.WriteLine("Tester: " + System.Security.Principal.WindowsIdentity.GetCurrent().Name);
                file.WriteLine("App Version: " + version);
                file.WriteLine("App Name: " + ConfigurationManager.AppSettings.Get("IntactPath"));

                //tests failed and passed on New Line
                int i = 0;
                foreach (string name in testsFailedNames)
                {
                    file.WriteLine("failed| " + name);
                    file.WriteLine(imagePaths[i]);
                    i++;
                }
                foreach (string name in testsPassedNames)
                {
                    file.WriteLine("passed| " + name);
                }
                foreach (string name in testsInconclusiveNames)
                {
                    file.WriteLine("cancel| " + name);
                }
            }
        }
        private void CheckForInterruptions(bool toggleInterruptCheck)
        {
            method = MethodBase.GetCurrentMethod().Name;
            if (toggleInterruptCheck)
            {
                if (m.GetTopLevelWindowInformation("process") != "Intact")
                {
                    Print(method, m.GetTopLevelWindowInformation("process") + " || " + m.GetTopLevelWindowInformation(""));
                    Print(method, "The current top window isn't intact, test interrupted");
                    throw new AssertInconclusiveException("The current top window is not intact, test interrupted");
                }
            }
        }
        private void CheckForIntactErrorMessage(bool toggleErrorCheck)
        {
            if (toggleErrorCheck)
            {
                method = MethodBase.GetCurrentMethod().Name;

                foreach (Process process in Process.GetProcesses())
                {
                    if (!String.IsNullOrEmpty(process.MainWindowTitle))
                    {
                        if (process.MainWindowTitle.Contains("Error") || process.MainWindowTitle.Contains("Exception"))
                        {
                            m.setTopLevelWindow(process);
                            Print(method, "Cleanup detects an error message:");
                            throw new AssertFailedException("Cleanup detects an error message");
                        }
                    }
                }
                Print(method, "There is no error present");
            }
        }
        ///<summary>
        ///<para>Default checks on call for both interruptions and for errors at end of testcase</para>
        ///<para>Throws AssertFail if there is an error present, Throws AssertInconclusive if Intact is not top window</para>
        ///</summary>
        public void EndOfTestCheck(bool errorcheck = true, bool interuptcheck = true)
        {
            CheckForIntactErrorMessage(errorcheck);
            CheckForInterruptions(interuptcheck);
        }
        public void SendToDB()
        {
            string connectionString = ConfigurationManager.AppSettings.Get("DBConnection");
            DataExporter exporter = new DataExporter(connectionString);
            exporter.ParseFile(new TestData());
        }
        public void DisposeErrorMessages()
        {
            foreach (Process process in Process.GetProcesses())
            {
                if (!String.IsNullOrEmpty(process.MainWindowTitle))
                {
                    var title = process.MainWindowTitle;
                    if (title.Contains("Error") || title.Contains("Exception") || title.Contains("exception") || title.Contains("error"))
                    {
                        process.Kill();
                        Print(method, "error killed");
                    }
                }
            }
        }
        public void CloseDriver()
        {
            m.CloseDriver();
        }
        public void WriteFailToWord()
        {
            throw new NotImplementedException();
            //method = MethodBase.GetCurrentMethod().Name;
            //Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
            //Document wordDoc = word.Documents.Add();
            //Range range = wordDoc.Range();

            //string folderPath = ConfigurationManager.AppSettings.Get("AutomationScreenshots");
            //foreach (string image in Directory.GetFiles(folderPath)) {

            //    string imagePath = Path.Combine(folderPath, Path.GetFileName(image));
            //    Print(method, imagePath);

            //    InlineShape autoScaledInlineShape = range.InlineShapes.AddPicture(imagePath);
            //    float scaledWidth = autoScaledInlineShape.Width;
            //    float scaledHeight = autoScaledInlineShape.Height;
            //    autoScaledInlineShape.Delete();

            //    // Create a new Shape and fill it with the picture
            //    Shape newShape = wordDoc.Shapes.AddShape(1, 0, 0, scaledWidth, scaledHeight);
            //    newShape.Fill.UserPicture(imagePath);

            //    // Convert the Shape to an InlineShape and optional disable Border
            //    InlineShape finalInlineShape = newShape.ConvertToInlineShape();
            //    finalInlineShape.Line.Visible = Microsoft.Office.Core.MsoTriState.msoFalse;

            //    // Cut the range of the InlineShape to clipboard
            //    finalInlineShape.Range.Cut();

            //    // And paste it to the target Range
            //    range.Paste();
            //}
            //wordDoc.SaveAs2(ConfigurationManager.AppSettings.Get("TestFailedDocs"));
            //word.Quit();
        }
        public void Print(string method, string toPrint)
        {
            debugLog.Info(method + " " + toPrint);
        }
    }
}
