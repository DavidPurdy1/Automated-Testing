﻿using log4net;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Data;

namespace ConsoleTests.src
{
    /// <summary>
    /// Class contains methods to create new document, definition, or type
    /// </summary>
    public class Create
    {

        #region fields
        IWebElement window;
        readonly WiniumMethods m;
        readonly Actions action;
        string method = "";
        readonly ILog debugLog;
        #endregion

        public Create(WiniumMethods m, Actions action, ILog debugLog)
        {
            this.m = m;
            this.action = action;
            this.debugLog = debugLog;
        }
        /// <summary>
        /// This is going to a specified amount of definitions with random name for each blank
        /// </summary>
        public void CreateNewDefinition(int? numberOfDefinitions = 1, string definitionName = "")
        {
            method = MethodBase.GetCurrentMethod().Name;
            Print(method, "Started");
            //check if maximized
            window = m.Locate(By.Id("frmIntactMain"));
            if (m.IsElementPresent(By.Name("Maximize"), window))
            {
                m.Click(By.Name("Maximize"), window);
            }
            window = m.Locate(By.Name("radMenu1"), window);
            m.Click(By.Name("&Administration"), window);
            window = m.Locate(By.Name("&Administration"), window);
            m.Click(By.Name("Definitions"), window);

            if (definitionName.Length < 2)
            {
                definitionName = "Test";
            }

            for (int i = 0; i <= numberOfDefinitions; i++)
            {
                var num = new Random().Next().ToString();
                window = m.Locate(By.Id("frmRulesList"), m.Locate(By.Id("frmIntactMain")));
                m.Click(By.Id("btnAdd"), window);
                window = m.Locate(By.Name("Add Definition"));
                Print(method, "Definition name is " + definitionName + num);
                foreach (IWebElement element in window.FindElements(By.Name("")))
                {
                    if (element.Enabled == true)
                    {
                        try { element.SendKeys(definitionName + " " + num); } catch (Exception) { }
                    }
                }
                m.Click(By.Name("&Save"), window);

            }

            m.Click(By.Name("&Close"));
            Print(method, "Finished");
        }
        /// <summary><para>This is going to a specified amount of definitions with random name for each blank </para>
        /// <para>numberOfTypes: how many to create, typeName: What to name the types </para>
        /// </summary>
        public void CreateNewType(int? numberOfTypes = 1, string typeName = "")
        {
            method = MethodBase.GetCurrentMethod().Name;
            Print(method, "Started");
            //check if maximized
            window = m.Locate(By.Id("frmIntactMain"));
            if (m.IsElementPresent(By.Name("Maximize"), window))
            {
                m.Click(By.Name("Maximize"), window);
            }
            window = m.Locate(By.Name("radMenu1"), window);
            m.Click(By.Name("&Administration"), window);

            m.Click(By.Name("Types"), m.Locate(By.Name("&Administration")));

            if (typeName.Length < 2)
            {
                typeName = "Test";
            }
            for (int i = 0; i < numberOfTypes; i++)
            {
                var temp = new Random().Next().ToString();
                window = m.Locate(By.Id("frmIntactMain"));
                Thread.Sleep(500);
                window = m.Locate(By.Id("frmAdminTypes"), window);
                Thread.Sleep(500);
                m.Click(By.Id("rbtnAdd"), window);
                Thread.Sleep(500);
                window = m.Locate(By.Id("frmAdminTypesInfo"));

                foreach (IWebElement element in window.FindElements(By.Name("")))
                {
                    if (element.Enabled == true)
                    {
                        try { element.SendKeys(typeName + temp); } catch (Exception) { }
                    }
                }
                m.Click(By.Name("&OK"));
            }
            m.Click(By.Name("&Close"));
            Print(method, "Finished");
        }
        /// <summary><para>Fast Creation of a document</para>
        /// <para>isPDF: Pdf or tif, docPath: specify where document is located, filenumber: which document in a certain directory </para>
        /// </summary>
        public void SimpleCreateDocument(bool isPDF = true, string docPath = "", int? fileNumber = 0)
        {
            method = MethodBase.GetCurrentMethod().Name;
            Print(method, "Started");

            m.Click(By.Name("Add Document"));

            //add document button (+ icon)
            Print(method, "x: " + Cursor.Position.X + " y: " + Cursor.Position.Y);
            m.Click(By.Id("lblType"));
            Print(method, "x: " + Cursor.Position.X + " y: " + Cursor.Position.Y);
            action.MoveByOffset(20, -40).Click().MoveByOffset(20, 60).Click().Build().Perform();
            Print(method, "x: " + Cursor.Position.X + " y: " + Cursor.Position.Y);

            //find the document to add in file explorer
            //configure docpath in app.config, takes arg of pdf or tif 
            if (docPath.Length < 1)
            {
                docPath = ConfigurationManager.AppSettings.Get("AddDocumentStorage");
            }
            m.SendKeys(By.Id("1001"), docPath);
            Print(method, "Go to \"" + docPath + "\"");
            m.Click(By.Name("Go to \"" + docPath + "\""));

            var rand = new Random();
            if (isPDF)
            {
                Winium.Elements.Desktop.ComboBox filesOfType = new Winium.Elements.Desktop.ComboBox(m.Locate(By.Name("Files of type:")));
                filesOfType.SendKeys("p");
                filesOfType.SendKeys(OpenQA.Selenium.Keys.Enter);
                Thread.Sleep(500);
                if (fileNumber == 0)
                {
                    action.MoveToElement(m.Locate(By.Id(rand.Next(Directory.GetFiles(docPath, "*.pdf").Length).ToString()))).DoubleClick().Build().Perform();
                }
                else
                {
                    action.MoveToElement(m.Locate(By.Id(fileNumber.ToString()))).DoubleClick().Build().Perform();
                }
                m.Click(By.Name("Open"));
            }
            else
            {
                if (fileNumber == 0)
                {
                    action.MoveToElement(m.Locate(By.Id(rand.Next(Directory.GetFiles(docPath, "*.tif").Length).ToString()))).DoubleClick().Build().Perform();
                }
                else
                {
                    action.MoveToElement(m.Locate(By.Id(fileNumber.ToString()))).DoubleClick().Build().Perform();
                }
                m.Click(By.Name("Open"));
            }

            Print(method, "save and quit");
            m.Click(By.Id("btnSave"));
            m.Click(By.Id("btnClose"));
            Print(method, "Finished");
        }
        public void CreateDocumentWithCheck(int? numOfDocs = 1, bool isPDF = true, string docPath = "", int? fileNumber = 0)
        {
            method = MethodBase.GetCurrentMethod().Name;
            Print(method, "Started");

            //check if maximized
            window = m.Locate(By.Id("frmIntactMain"));
            if (m.IsElementPresent(By.Name("Maximize"), window))
            {
                m.Click(By.Name("Maximize"), window);
            }

            for (int i = 0; i < numOfDocs; i++)
            {
                var guid = Guid.NewGuid().ToString();
                m.Click(By.Name("Add Document"));
                Thread.Sleep(3000);
                m.Click(By.Id("lblType"));
                action.MoveByOffset(30, 0).Click().SendKeys("test").Build().Perform();
                m.Click(By.Id("lblType"));
                action.MoveByOffset(30, 27).Click().SendKeys("(None)").Build().Perform();
                m.Click(By.Id("lblType"));
                action.MoveByOffset(30, 56).Click().SendKeys(guid).Build().Perform();


                //add document button (+ icon)
                m.Click(By.Id("lblType"));
                action.MoveByOffset(20, -40).Click().MoveByOffset(20, 60).Click().Build().Perform();

                FileExplorer(isPDF, docPath, fileNumber);

                //edit custom fields
                Print(method, "custom fields");
                m.Click(By.Id("lblType"));
                //get the test case number and that is going to be the metadata field for the number
                var date = DateTime.Now.Date;
                string number = "7";
                action.MoveByOffset(150, 240).Click().SendKeys(date.ToString()).
                    MoveByOffset(0, 20).Click().SendKeys(number).MoveByOffset(0, 20).Click().SendKeys(guid).Build().Perform();

                //add author as the tester name
                // m.Click(By.Id("lblType"));
                // action.MoveByOffset(170, 80).Click().SendKeys("1/1/2000").MoveByOffset(0, 20).Click().SendKeys("BATCH AUTHOR TEST").
                //     MoveByOffset(0, 40).Click().SendKeys("BATCH ADDING TO ANOTHER DOCUMENT TEST").Build().Perform();
                //save and quit
                Print(method, "save and quit");
                m.Click(By.Id("btnSave"));
                window = m.Locate(By.Name("File Document"));
                m.Click(By.Name("Yes"), window);
                m.Click(By.Name("DEFAULT DEF"));
                m.Click(By.Name("&OK"));
                m.Click(By.Id("btnClose"));
                m.Click(By.Id("frmIntactMain"));
                Print(method, "Finished the document addition");
                Thread.Sleep(5000);
                VerifyDocumentAdd(guid, date, number);
            }
        }

        /// <summary><para>Verifies that a document is filed correctly by testing name(guid) and metadata are going to be the same</para>
        /// <para>Throws an assertfail for the test if the information passed in and what is found in the sql table is not the same</para>
        /// </summary>
        private void VerifyDocumentAdd(string guid, DateTime date, string number)
        {
            string num = "";
            method = MethodBase.GetCurrentMethod().Name;
            SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings.Get("DBDocumentData"));
            connection.Open();
            SqlCommand command = new SqlCommand();
            command.CommandTimeout = 60;
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;

            command.CommandText = "spGetDocumentDataFromMostRecent";
            Print(method, "has started");
            using (SqlDataReader reader = command.ExecuteReader())
            {
                DataTable table = new DataTable();
                table.Load(reader);

                var index = table.Rows[0]["value"].ToString().IndexOf(".");
                num = table.Rows[0]["value"].ToString().Substring(0, index);
                if (!guid.Equals(table.Rows[0]["DOCUMENT_NAME"].ToString()))
                {
                    Print(method, "Document Name Data Does Not Match");
                    throw new AssertFailedException("Document Name Data Does Not Match");
                }
                else if (!guid.Equals(table.Rows[2]["value"].ToString()))
                {
                    Print(method, "predicted " + guid);
                    Print(method, "actual " + table.Rows[2]["value"].ToString());

                    Print(method, "Document guid string Data Does Not Match");
                    throw new AssertFailedException("Document guid string Data Does Not Match");
                }
                else if (!date.Equals(DateTime.Parse(table.Rows[1]["value"].ToString())))
                {
                    Print(method, "predicted " + date.ToString());
                    Print(method, "actual " + table.Rows[1]["value"].ToString());

                    Print(method, "Document date Data Does Not Match");
                    throw new AssertFailedException("Document date Data Does Not Match");
                }
                else if (!number.Equals(num))
                {
                    Print(method, "predicted " + number);
                    Print(method, "actual " + table.Rows[0]["value"].ToString());

                    Print(method, "Document number Data Does Not Match");
                    throw new AssertFailedException("Document number Data Does Not Match");
                }
                reader.Close();
                connection.Close();
            }
            Print(method, "Document Data Matches in Table");
        }
        //find the document to add in file explorer
        //configure docpath in app.config, takes arg of pdf or tif 
        private void FileExplorer(bool isPDF, string docPath, int? fileNumber)
        {
            if (docPath.Length < 1)
            {
                docPath = ConfigurationManager.AppSettings.Get("AddDocumentStorage");
            }
            m.SendKeys(By.Id("1001"), docPath);
            Print(method, "Go to \"" + docPath + "\"");
            m.Click(By.Name("Go to \"" + docPath + "\""));

            var rand = new Random();
            if (isPDF)
            {
                Winium.Elements.Desktop.ComboBox filesOfType = new Winium.Elements.Desktop.ComboBox(m.Locate(By.Name("Files of type:")));
                filesOfType.SendKeys("p");
                filesOfType.SendKeys(OpenQA.Selenium.Keys.Enter);
                Thread.Sleep(500);
                if (fileNumber == 0)
                {
                    action.MoveToElement(m.Locate(By.Id(rand.Next(Directory.GetFiles(docPath, "*.pdf").Length).ToString()))).DoubleClick().Build().Perform();
                }
                else
                {
                    action.MoveToElement(m.Locate(By.Id(fileNumber.ToString()))).DoubleClick().Build().Perform();
                }
                //m.Click(By.Name("Open"));
            }
            else
            {
                if (fileNumber == 0)
                {
                    action.MoveToElement(m.Locate(By.Id(rand.Next(Directory.GetFiles(docPath, "*.tif").Length).ToString()))).DoubleClick().Build().Perform();
                }
                else
                {
                    action.MoveToElement(m.Locate(By.Id(fileNumber.ToString()))).DoubleClick().Build().Perform();
                }
                //m.Click(By.Name("Open"));
            }
            Thread.Sleep(2000);
        }
        /// <summary><para>Creation of Documents</para>
        /// <para>numOfDocs: specifies how many to create, isPDF: pdf or tif,docPath: allows you to specify the directory of docs, default is set in config, filenumber: which document in a certain directory </para>
        /// </summary>
        public void CreateDocument(int? numOfDocs = 1, bool isPDF = true, string docPath = "", int? fileNumber = 0)
        {
            method = MethodBase.GetCurrentMethod().Name;
            Print(method, "Started");

            //check if maximized
            window = m.Locate(By.Id("frmIntactMain"));
            if (m.IsElementPresent(By.Name("Maximize"), window))
            {
                m.Click(By.Name("Maximize"), window);
            }

            for (int i = 0; i < numOfDocs; i++)
            {
                m.Click(By.Name("Add Document"));

                //add document button (+ icon)

                m.Click(By.Id("lblType"));
                action.MoveByOffset(20, -40).Click().MoveByOffset(20, 60).Click().Build().Perform();

                //find the document to add in file explorer
                //configure docpath in app.config, takes arg of pdf or tif 
                if (docPath.Length < 1)
                {
                    docPath = ConfigurationManager.AppSettings.Get("AddDocumentStorage");
                }
                m.SendKeys(By.Id("1001"), docPath);
                Print(method, "Go to \"" + docPath + "\"");
                m.Click(By.Name("Go to \"" + docPath + "\""));

                var rand = new Random();
                if (isPDF)
                {
                    Winium.Elements.Desktop.ComboBox filesOfType = new Winium.Elements.Desktop.ComboBox(m.Locate(By.Name("Files of type:")));
                    filesOfType.SendKeys("p");
                    filesOfType.SendKeys(OpenQA.Selenium.Keys.Enter);
                    Thread.Sleep(500);
                    if (fileNumber == 0)
                    {
                        action.MoveToElement(m.Locate(By.Id(rand.Next(Directory.GetFiles(docPath, "*.pdf").Length).ToString()))).DoubleClick().Build().Perform();
                    }
                    else
                    {
                        action.MoveToElement(m.Locate(By.Id(fileNumber.ToString()))).DoubleClick().Build().Perform();
                    }
                    m.Click(By.Name("Open"));
                }
                else
                {
                    if (fileNumber == 0)
                    {
                        action.MoveToElement(m.Locate(By.Id(rand.Next(Directory.GetFiles(docPath, "*.tif").Length).ToString()))).DoubleClick().Build().Perform();
                    }
                    else
                    {
                        action.MoveToElement(m.Locate(By.Id(fileNumber.ToString()))).DoubleClick().Build().Perform();
                    }
                    m.Click(By.Name("Open"));
                }
                Thread.Sleep(3000);


                //edit custom fields
                Print(method, "custom fields");
                m.Click(By.Id("lblType"));
                action.MoveByOffset(150, 240).Click().SendKeys("1/1/2000").
                    MoveByOffset(0, 20).Click().SendKeys("7").MoveByOffset(0, 20).Click().SendKeys("string").Build().Perform();

                //save and quit
                Print(method, "save and quit");
                m.Click(By.Id("btnSave"));
                m.Click(By.Id("btnClose"));
                Print(method, "Finished");
            }
        }
        /// <summary><para>Not Implemented</para></summary>
        private void AddAnnotations()
        {
            throw new NotImplementedException();
            // Print(method, "x: " + Cursor.Position.X + " y: " + Cursor.Position.Y);
            // m.Click(By.Id("lblType"));
            // Print(method, "x: " + Cursor.Position.X + " y: " + Cursor.Position.Y);
            // action.MoveByOffset(424, -35).Build().Perform();
            // Thread.Sleep(1000);
            // action.Click().MoveByOffset(0, 100).Build().Perform();
            // Print(method, "x: " + Cursor.Position.X + " y: " + Cursor.Position.Y);
            // action.ClickAndHold().MoveByOffset(0, 50).Build().Perform();
        }
        private void Print(string method, string toPrint)
        {
            debugLog.Info(method + " " + toPrint);
        }
    }
}
