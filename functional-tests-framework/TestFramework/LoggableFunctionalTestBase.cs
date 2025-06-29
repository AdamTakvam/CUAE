//using System;
//using System.Text;
//using System.Collections;
//using System.Collections.Specialized;
//using System.Diagnostics;
//using System.IO;
//
//using Metreos.Samoa.Core;
//using Metreos.Samoa.Interfaces;
//using Metreos.Samoa.FunctionalTestFramework;
//
//namespace Metreos.Samoa.FunctionalTestFramework
//{
//
//    public delegate string FunctionalTestOrderOfOutputDelegate(Hashtable testResults, Hashtable testResultsTime);
//
//    public abstract class LoggableFunctionalTestBase : FunctionalTestBase
//    {
//        protected object testResultsWriteLock;
//        protected object testResultsTimeWriteLock;
//
//        protected Hashtable testResults;
//        protected Hashtable testResultsTime;
//
//        /// The starting and ending times of the test
//        // initial should be declared immediately before the first signal is sent
//        // final should be declared immediately after the last signal is sent
//        protected string initial;
//        protected string final;
//
//        protected string[] loggingGroupHeader;
//
//        public LoggableFunctionalTestBase() : this(TraceLevel.Info, "Unknown functional test")
//        {}
//
//        public LoggableFunctionalTestBase(TraceLevel logLevel, string testName) : base(logLevel, testName)
//        {
//            this.loggingGroupHeader = new string[2] {this.testName, "Test Outcome"};
//
//            testResultsWriteLock = new Object();
//            testResultsTimeWriteLock = new Object();
//
//            testResults = new Hashtable();
//            testResultsTime = new Hashtable();
//
//            testDescription = "No description";
//        }
//        
//        public LoggableFunctionalTestBase(TraceLevel logLevel, string testName, string group) : base(logLevel, testName, group)
//        {
//            this.loggingGroupHeader = new string[2] {this.testName, "Test Outcome"};
//
//            testResultsWriteLock = new Object();
//            testResultsTimeWriteLock = new Object();
//
//            testResults = new Hashtable();
//            testResultsTime = new Hashtable();
//
//            testDescription = "No description";
//        }
//
//        public override bool Execute()
//        {
//            PrepareForBeginningOfTest();
//            
//            bool resultOfTest = Execute();
//
//            PrepareForEndOfTest(resultOfTest);
//
//            return resultOfTest;
//        }
//
//        public abstract bool Execute(Hashtable values, StringCollection messages);
//
//        public override void TearDown()
//        {
//            testResults.Clear();
//            testResultsTime.Clear();
//
//            base.TearDown();
//        }
//
//        public void AddMessage(string key, string message)
//        {
//
//            if(testResults.ContainsKey(key) == true)
//            {
//                lock(testResultsWriteLock)
//                {
//                    ((StringCollection)testResults[key]).Add(message);
//                }
//            }
//            else
//            {
//                StringCollection collection = new StringCollection();
//                collection.Add(message);
//
//                lock(testResultsWriteLock)
//                {
//                    testResults.Add(key, collection);
//                }
//            }
//
//            if(testResultsTime.ContainsKey(key) == true)
//            {
//                lock(testResultsTimeWriteLock)
//                {
//                    // Also log time message was recorded
//                    ((StringCollection)testResultsTime[key]).Add(DateTime.Now.ToLongTimeString());
//                }
//            }
//            else
//            {
//                // Also log time message was recorded
//
//                StringCollection time = new StringCollection();
//                time.Add(DateTime.Now.ToLongTimeString());
//
//                lock(testResultsTimeWriteLock)
//                {
//                    testResultsTime.Add(key, time);
//                }     
//            }               
//        }
//
//        public void OutputMessage(FunctionalTestOrderOfOutputDelegate output)
//        {
//            string toFile = output(testResults, testResultsTime);
//
//            // REFACTOR:  TRY/CATCH for file IO
//            string timeStamp = DateTime.Now.ToString("MM-dd-yy");
//            Directory.CreateDirectory(ITestConfiguration.PATH_TO_TEST_RESULTS + timeStamp);
//            Directory.CreateDirectory(ITestConfiguration.PATH_TO_TEST_RESULTS + timeStamp + "\\" + testName);
//
//            string[] iteration = Directory.GetDirectories(ITestConfiguration.PATH_TO_TEST_RESULTS + timeStamp + "\\" + testName);
//            string newFolderName;
//            
//            // Test for the iteration count for this test run.  If 1, then create a new test run directory.
//            // If more than 1, then use last test run folder
//
//            int numberOfFullTestRuns = iteration.Length + 1;  // Bumping up the new folder name past the most current test run
//            newFolderName = numberOfFullTestRuns.ToString();
//            newFolderName = "TestRun_" + newFolderName;
//            Directory.CreateDirectory(ITestConfiguration.PATH_TO_TEST_RESULTS + timeStamp + "\\" + testName + "\\" + newFolderName);
//           
//            iteration = Directory.GetFiles(ITestConfiguration.PATH_TO_TEST_RESULTS + timeStamp + "\\" + testName + "\\" + newFolderName);
//            int iterationCount = iteration.Length + 1;
//            string newFileName = iterationCount.ToString();
//            newFileName = "Iteration_" + newFileName + ".html";
//
//            FileStream fileWriter = File.Create(ITestConfiguration.PATH_TO_TEST_RESULTS + timeStamp + "\\" + testName + "\\" + newFolderName + "\\" + newFileName);
//            StreamWriter stream = new StreamWriter(fileWriter);
//            stream.Write(toFile);
//
//            stream.Close();
//            fileWriter.Close();
//
//            stream = null;
//            fileWriter = null;
//        }
//
//        /// <summary>
//        /// Override by adding descriptive messages for the test to loggingGroupHeader[0] 
//        /// </summary>
//        public virtual void PrepareForBeginningOfTest()
//        {
//            
//
//            // Fill out test info for XHTML logger.
//            AddMessage(loggingGroupHeader[0], testDescription);
//            
//            // Record test starting time
//            initial = DateTime.Now.ToLongTimeString();
//        } 
//     
//        /// <summary>
//        /// Logs final time and prepares the built of messages of the test
//        /// </summary>
//        /// <param name="messages">Passed on from Execute method</param>
//        /// <param name="success">Success parameter</param>
//        public virtual void PrepareForEndOfTest(bool success)
//        {     
//            final = DateTime.Now.ToLongTimeString();
//
//            //Log test outcome and messages
//            if(success == true)
//            {
//                //messages.Add("Test completed successfully");
//                AddMessage(loggingGroupHeader[1], "Success");
//            }
//            else
//            {
//                //messages.Add("Test exiting abnormally");
//                AddMessage(loggingGroupHeader[1], "Failure");
//            }  
//
////            for (int i = 0; i < messages.Count;  i++)
////            {
////                AddMessage(loggingGroupHeader[1], messages[i]);
////            }
//           
//            // begin outputting to permenant test log
//            OutputMessage(new FunctionalTestOrderOfOutputDelegate(this.GenerateResults));
//        }
//        
//        /// <summary>
//        /// Creates a permanent output file for the test. Override it if you want to output more
//        /// than just a test description, start/stop time, and message log for the test.
//        /// </summary>
//        /// <param name="testResults">Hash with test results</param>
//        /// <param name="testResultsTime">A "parallel" hash that automatically records the times of all entries
//        /// in testResults</param>
//        /// <returns>The XHTML that OutputMessage uses.</returns>
//        public virtual string GenerateResults(Hashtable testResults, Hashtable testResultsTime)
//        {
//       
//            // REFACTOR:  TRY/CATCH for file IO
//            StringBuilder toFile = new StringBuilder();
//    
//            #region HTML HEADER
//            
//            toFile.Append(@"<html>
//<head>
//<title>StressTestResults</title>
//<link href=""style.css"" type=""text/css"" rel=""stylesheet"">
//<meta content=""text/html; charset=ISO-8859-1""
//http-equiv=""Content-Type"">
//<style>
//body{
//position: absolute;
//top: 0px;
//bottom: 0px;
//left: 0px;
//right: 0px;
//margin: 0px;
//padding-left: 0px;
//padding-right: 0px;
//padding-top: 0px;
//padding-bottom: 0px;
//background-color:#99a8c6;
//color:black;
//font-family: verdana,arial,sans-serif;
//font-size:1em;
//height:100%;
//width:100%;
//
//}
//
//
//div{
//position:relative;
//background-color:#99a8c6;
//overflow:auto;
//}
//
//span{
//
//}
//p{
//margin:1px 3px 10px;
//}
//th{
//background-color:#bbbbbb;
//border-color:black;
//border-style:solid;
//border-width:0px 0px 1px 0px;
//text-align:left;
//padding:2px 8px;
//}
//
//tr{
//border-width:0px;
//padding:0px;
//margin:0px;
//
//}
//td{
//border-width:0px;
//margin:0px;
//
//}
//td.even{
//background-color:#d4d4d4;
//border-width:0px;
//padding:2px 5px;
//margin:0px;
//
//}
//td.odd{
//background-color:#bfbfbf;
//border-width:0px;
//padding:2px 5px;
//margin:0px;
//}
//
//
//table{
//border-width:0px;
//padding:0px;
//margin:0px;
//}
//.info{}
//.warning{background-color:yellow;font-size:large}
//.error{background-color:red;font-size:large}
//.important{font-size:larger}
//
//span.title{
//font-size:x-large;
//text-decoration:underline;
//display:block;
//margin:0px 0px 20px 0px;
//}
//div.main{
//margin:5px 30px;
//background-color:#2d2d2d;
//border:1px solid black;
//padding:10px;
//font-size:xx-large;
//color:#eeeeee;
//}
//div.signalInterval{
//margin:5px 30px;
//background-color:#eeeeee;
//border:1px solid #777777;
//padding:10px;
//}
//
//div.allSignalsInterval{
//margin:5px 5px 5px 30px;
//background-color:#dddddd;
//border:1px solid #777777;
//padding:10px;
//}
//div.perfCounter{
//margin:5px 30px;
//background-color:#d4d4d4;
//border:1px solid #777777;
//padding:10px;
//}
//
//div.orderOfSignals{
//margin:5px 30px;
//background-color:#ffffff;
//border:1px solid #777777;
//padding:10px;
//}
//
//div.appInterval{
//margin:5px 30px;
//background-color:#eeeeee;
//border:1px solid #777777;
//padding:10px;
//}
//div.testOutcome{
//margin:5px 30px;
//background-color:black;
//color:white;
//border:1px solid #777777;
//padding:10px;
//}
//div.msmqSizeCreation{
//margin:5px 30px;
//background-color:c9c9c9;
//color:black;
//border:1px solid #777777;
//padding:10px;
//
//}
//div.threeTableRowsGeneric{
//margin:5px 30px;
//background-color:#ffffff;
//border:1px solid #777777;
//padding:10px;
//}
//table.testOutcome{
//background-color:white;
//color:black;
//border:2px solid white;
//
//}
//th.testOutcome{
//background-color:white;
//}
//td.testOutcome{
//border: 0px solid #333333;
//border-width: 0px 0px 1px;
//}
//
//
//
//</style>
//<script type=""text/javascript"">
//</script>
//</head>
//<body>
//<br>
//</body>
//</html>");
//
//            #endregion HTML HEADER
//
//            // Create summary info header for XHTML page
//            TestHeaderCreation firstResult = new TestHeaderCreation((StringCollection)testResults[loggingGroupHeader[0]], loggingGroupHeader[0]);
//            toFile.Append(firstResult.CreateXHTML());
//
//            TestOutcomeCreation secondResult = new TestOutcomeCreation((StringCollection)testResults[loggingGroupHeader[1]], loggingGroupHeader[1], initial, final);
//            toFile.Append(secondResult.CreateXHTML());
//            // Write out results from test 
//
//            // Close out HTML
//            toFile.Append(Constants.HTML_FOOTER);
//
//            return toFile.ToString();        
//        }
//    }
//}