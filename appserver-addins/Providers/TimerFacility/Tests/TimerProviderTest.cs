using System;
using System.Diagnostics;

using Metreos.Samoa.Core;
using Metreos.Samoa.Interfaces;
using Metreos.Providers.TimerFacility;

namespace Metreos.Providers.TimerFacility.Tests
{
    #region Mock Objects

    sealed class MockPrimaryComponent : PrimaryTaskBase
    {
        public MockPrimaryComponent(string name) : base(name, TraceLevel.Info)
        {}

        public InternalMessage GetMessageFromQueue()
        {
            InternalMessage im;
            this.taskQueue.Receive(out im);
            return im;
        }

        protected override void OnStartup()
        {}
        
        protected override void OnShutdown()
        {}

		protected override void RefreshConfiguration()
		{}


        protected override bool HandleMessage(InternalMessage im)
        {
            return true;
        }

        protected override void Run()
        {}
    }


    #endregion

    public class TimerProviderTest
    {
        TimerProvider timer;
        MockPrimaryComponent pal;

        public TimerProviderTest()
        {
            pal = new MockPrimaryComponent("TimerProviderTest-MockPrimaryComponent");

            timer = new TimerProvider();
            timer.Initialize(pal.Name);

            InternalMessage im = new InternalMessage();

            im.MessageId = ITask.MSG_STARTUP;

            timer.PostMessage(im);

            System.Threading.Thread.Sleep(250);
        }

        [csUnit.FixtureTearDown]
        public void FixtureTearDown()
        {
            InternalMessage im;

            timer.ResetTimerTable();

            if(timer.TaskStatus == PrimaryTaskBase.TaskStatusType.STARTED)
            {
                im = new InternalMessage();
                im.MessageId = ITask.MSG_SHUTDOWN;

                timer.PostMessage(im);

                System.Threading.Thread.Sleep(100);

                im = null;
            }

            if(pal.TaskStatus == PrimaryTaskBase.TaskStatusType.STARTED)
            {
                im = new InternalMessage();
                im.MessageId = ITask.MSG_SHUTDOWN;

                pal.PostMessage(im);

                System.Threading.Thread.Sleep(100);

                im = null;
            }

            pal.Cleanup();
            timer.Cleanup();
        }

        public void testTimerRegisteredItsNamespace()
        {
            InternalMessage im;

            im = this.pal.GetMessageFromQueue();

            csUnit.Assert.NotNull(im);
            csUnit.Assert.Equals("Metreos.Samoa.PAL.RegisterProviderNamespace", im.MessageId);
        }

        public void testAddTimer()
        {
            timer.ResetTimerTable();

            csUnit.Assert.Equals(PrimaryTaskBase.TaskStatusType.STARTED, timer.TaskStatus);
            csUnit.Assert.Equals(0, timer.GetNumberOfTimers());
                
            InternalMessage im = new InternalMessage();

            im.MessageId = TimerProvider.MSG_ADD_TIMER;
            im.Source = "testAddTimer";

            im.AddField(new Field("actionGuid", "testAddTimerActionGuid"));
            im.AddField(new Field(TimerProvider.FIELD_TIMER_DATE_TIME, DateTime.Now.AddMilliseconds(30000).ToString()));

            timer.PostMessage(im);

            System.Threading.Thread.Sleep(100);

            im = null;

            im = this.pal.GetMessageFromQueue();

            string timerIdField;
            string actionGuidField;

            csUnit.Assert.Equals(1, timer.GetNumberOfTimers());
            csUnit.Assert.NotNull(im);
            csUnit.Assert.Equals(TimerProvider.MSG_ADD_TIMER_COMPLETE, im.MessageId);
            csUnit.Assert.True(im.GetFieldByName("actionResult", out timerIdField));
            csUnit.Assert.NotNull(timerIdField);
            csUnit.Assert.NotEquals("", timerIdField);
            csUnit.Assert.True(im.GetFieldByName("actionGuid", out actionGuidField));
            csUnit.Assert.NotNull(actionGuidField);
            csUnit.Assert.Equals("testAddTimerActionGuid", actionGuidField);
            csUnit.Assert.False(im.GetFieldByName(TimerProvider.FIELD_TIMER_USER_DATA, out actionGuidField));

            timerIdField = null;
            actionGuidField = null;

            timer.ResetTimerTable();
        }

        public void testRemoveTimer()
        {
            timer.ResetTimerTable();

            csUnit.Assert.Equals(PrimaryTaskBase.TaskStatusType.STARTED, timer.TaskStatus);
            csUnit.Assert.Equals(0, timer.GetNumberOfTimers());

            InternalMessage im = new InternalMessage();

            im.MessageId = TimerProvider.MSG_ADD_TIMER;
            im.Source = "testRemoveTimer";
            im.AddField(new Field("actionGuid", "testRemoveTimerActionGuid"));
            im.AddField(new Field(TimerProvider.FIELD_TIMER_DATE_TIME, DateTime.Now.AddMilliseconds(10000).ToString()));

            timer.PostMessage(im);

            System.Threading.Thread.Sleep(100);

            im = null;

            im = this.pal.GetMessageFromQueue();

            string timerIdField;

            im.GetFieldByName("actionResult", out timerIdField);

            im = null;

            im = new InternalMessage();

            im.MessageId = TimerProvider.MSG_REMOVE_TIMER;
            im.AddField(new Field(TimerProvider.FIELD_TIMER_ID, timerIdField));

            timer.PostMessage(im);

            System.Threading.Thread.Sleep(100);

            csUnit.Assert.Equals(0, timer.GetNumberOfTimers());

            timer.ResetTimerTable();

            im = null;
        }

        public void testAddTimerFailed()
        {
            timer.ResetTimerTable();

            csUnit.Assert.Equals(PrimaryTaskBase.TaskStatusType.STARTED, timer.TaskStatus);
            csUnit.Assert.Equals(0, timer.GetNumberOfTimers());
                
            InternalMessage im = new InternalMessage();

            im.MessageId = TimerProvider.MSG_ADD_TIMER;
            im.Source = "testAddTimer";

            im.AddField(new Field("actionGuid", "testAddTimerFailedActionGuid"));
            
            // Add the timerDateTime field with the current time, this will guarantee that it will occur
            // in the past by the time it gets to the provider.
            im.AddField(new Field(TimerProvider.FIELD_TIMER_DATE_TIME, DateTime.Now.ToString()));

            timer.PostMessage(im);

            System.Threading.Thread.Sleep(100);

            im = null;

            im = this.pal.GetMessageFromQueue();

            string timerIdField;
            string actionGuidField;

            csUnit.Assert.Equals(0, timer.GetNumberOfTimers());
            csUnit.Assert.NotNull(im);
            csUnit.Assert.Equals(TimerProvider.MSG_ADD_TIMER_FAILED, im.MessageId);
            csUnit.Assert.False(im.GetFieldByName("actionResult", out timerIdField));
            csUnit.Assert.Null(timerIdField);
            csUnit.Assert.True(im.GetFieldByName("actionGuid", out actionGuidField));
            csUnit.Assert.NotNull(actionGuidField);
            csUnit.Assert.Equals("testAddTimerFailedActionGuid", actionGuidField);
            csUnit.Assert.False(im.GetFieldByName(TimerProvider.FIELD_TIMER_USER_DATA, out actionGuidField));

            timerIdField = null;
            actionGuidField = null;

            timer.ResetTimerTable();
        }

        public void testSingleTimerFired()
        {
            timer.ResetTimerTable();

            csUnit.Assert.Equals(PrimaryTaskBase.TaskStatusType.STARTED, timer.TaskStatus);
            csUnit.Assert.Equals(0, timer.GetNumberOfTimers());

            InternalMessage im = new InternalMessage();

            im.MessageId = TimerProvider.MSG_ADD_TIMER;
            im.Source = "testSingleTimerFired";
            im.AddField(new Field("actionGuid", "testSingleTimerFiredActionGuid"));
            im.AddField(new Field(TimerProvider.FIELD_TIMER_DATE_TIME, DateTime.Now.AddMilliseconds(1000).ToString()));

            timer.PostMessage(im);

            System.Threading.Thread.Sleep(1500);

            im = null;

            im = this.pal.GetMessageFromQueue();

            string origTimerId;
            csUnit.Assert.Equals(TimerProvider.MSG_ADD_TIMER_COMPLETE, im.MessageId);
            csUnit.Assert.True(im.GetFieldByName("actionResult", out origTimerId));

            im = null;

            im = this.pal.GetMessageFromQueue(); 

            string temp;

            csUnit.Assert.NotNull(im);
            csUnit.Assert.Equals(TimerProvider.MSG_TIMER_FIRE, im.MessageId);
            csUnit.Assert.True(im.GetFieldByName(TimerProvider.FIELD_TIMER_ID, out temp));
            csUnit.Assert.True(im.GetFieldByName("protocolProviderGuid", out temp));

            origTimerId = null;
            temp = null;

            timer.ResetTimerTable();
        }

        public void testAddMultipleTimersAndWaitForFire()
        {
            timer.ResetTimerTable();

            csUnit.Assert.Equals(PrimaryTaskBase.TaskStatusType.STARTED, timer.TaskStatus);
            csUnit.Assert.Equals(0, timer.GetNumberOfTimers());
            
            int numTimersToAdd = 10;

            InternalMessage im;

            for(int i = 0; i < numTimersToAdd; i++)
            {
                im = new InternalMessage();

                im.MessageId = TimerProvider.MSG_ADD_TIMER;
                im.Source = "testAddTimer";

                im.AddField(new Field("actionGuid", "testAddMultipleTimersAndWaitForFire" + i));
                im.AddField(new Field(TimerProvider.FIELD_TIMER_DATE_TIME, DateTime.Now.AddMilliseconds(2000).ToString()));

                timer.PostMessage(im);

                System.Threading.Thread.Sleep(5);
            }

            System.Threading.Thread.Sleep(250);

            im = null;

            int j = 0;

            csUnit.Assert.Equals(numTimersToAdd, timer.GetNumberOfTimers());

            while(j < numTimersToAdd)
            {
                im = this.pal.GetMessageFromQueue();

                string timerIdField;
                string actionGuidField;

                csUnit.Assert.NotNull(im);
                csUnit.Assert.Equals(TimerProvider.MSG_ADD_TIMER_COMPLETE, im.MessageId);
                csUnit.Assert.True(im.GetFieldByName("actionResult", out timerIdField));
                csUnit.Assert.NotNull(timerIdField);
                csUnit.Assert.NotEquals("", timerIdField);
                csUnit.Assert.True(im.GetFieldByName("actionGuid", out actionGuidField));
                csUnit.Assert.NotNull(actionGuidField);
                csUnit.Assert.True(actionGuidField.StartsWith("testAddMultipleTimersAndWaitForFire"));

                j++;
                im = null;
                timerIdField = null;
                actionGuidField = null;
            }

            System.Threading.Thread.Sleep(2000);

            j = 0;

            while(j < numTimersToAdd)
            {
                im = this.pal.GetMessageFromQueue(); 

                string temp;

                csUnit.Assert.NotNull(im);
                csUnit.Assert.Equals(TimerProvider.MSG_TIMER_FIRE, im.MessageId);
                csUnit.Assert.True(im.GetFieldByName(TimerProvider.FIELD_TIMER_ID, out temp));
                csUnit.Assert.True(im.GetFieldByName("protocolProviderGuid", out temp));

                temp = null;
                im = null;

                j++;
            }

            timer.ResetTimerTable();
        }

        public void testUserSpecificTimerData()
        {
            timer.ResetTimerTable();

            csUnit.Assert.Equals(PrimaryTaskBase.TaskStatusType.STARTED, timer.TaskStatus);
            csUnit.Assert.Equals(0, timer.GetNumberOfTimers());

            InternalMessage im = new InternalMessage();

            im.MessageId = TimerProvider.MSG_ADD_TIMER;
            im.Source = "testUserSpecificTimerInformation";
            im.AddField(new Field("actionGuid", "testUserSpecificTimerInformationActionGuid"));
            im.AddField(new Field(TimerProvider.FIELD_TIMER_DATE_TIME, DateTime.Now.AddMilliseconds(1000).ToString()));
            im.AddField(new Field(TimerProvider.FIELD_TIMER_USER_DATA, "testUserSpecificTimerInformation"));

            timer.PostMessage(im);

            System.Threading.Thread.Sleep(2000);

            im = null;

            im = this.pal.GetMessageFromQueue();

            csUnit.Assert.Equals(TimerProvider.MSG_ADD_TIMER_COMPLETE, im.MessageId);

            im = null;

            im = this.pal.GetMessageFromQueue(); 

            string temp;
            string userData;

            csUnit.Assert.NotNull(im);
            csUnit.Assert.Equals(TimerProvider.MSG_TIMER_FIRE, im.MessageId);
            csUnit.Assert.True(im.GetFieldByName("protocolProviderGuid", out temp));
            csUnit.Assert.True(im.GetFieldByName(TimerProvider.FIELD_TIMER_ID, out temp));
            csUnit.Assert.True(im.GetFieldByName(TimerProvider.FIELD_TIMER_USER_DATA, out userData));
            csUnit.Assert.Equals("testUserSpecificTimerInformation", userData);

            temp = null;
            userData = null;
            im = null;

            timer.ResetTimerTable();
        }

        public void testTimerWithRecurrence()
        {
            timer.ResetTimerTable();

            csUnit.Assert.Equals(PrimaryTaskBase.TaskStatusType.STARTED, timer.TaskStatus);
            csUnit.Assert.Equals(0, timer.GetNumberOfTimers());

            InternalMessage im = new InternalMessage();

            im.MessageId = TimerProvider.MSG_ADD_TIMER;
            im.Source = "testTimerWithRecurrence";
            im.AddField(new Field("actionGuid", "testTimerWithRecurrenceActionGuid"));
            im.AddField(new Field(TimerProvider.FIELD_TIMER_DATE_TIME, DateTime.Now.AddMilliseconds(1000).ToString()));
            im.AddField(new Field(TimerProvider.FIELD_TIMER_USER_DATA, "testUserSpecificTimerInformation"));
            im.AddField(new Field(TimerProvider.FIELD_TIMER_RECURRENCE, new TimeSpan(0, 0, 0, 0, 50).ToString()));

            timer.PostMessage(im);

            System.Threading.Thread.Sleep(2250);

            im = null;

            im = this.pal.GetMessageFromQueue();

            csUnit.Assert.Equals(TimerProvider.MSG_ADD_TIMER_COMPLETE, im.MessageId);

            im = null;

            im = new InternalMessage();

            im.MessageId = TimerProvider.MSG_REMOVE_TIMER;
            im.AddField(new Field(TimerProvider.FIELD_TIMER_ID, "testTimerWithRecurrenceActionGuid"));

            timer.PostMessage(im);

            System.Threading.Thread.Sleep(250);

            im = null;

            im = this.pal.GetMessageFromQueue();

            while(im != null)
            {
                string temp;
                string userData;

                csUnit.Assert.NotNull(im);
                csUnit.Assert.Equals(TimerProvider.MSG_TIMER_FIRE, im.MessageId);
                csUnit.Assert.True(im.GetFieldByName(TimerProvider.FIELD_TIMER_ID, out temp));
                csUnit.Assert.True(im.GetFieldByName("protocolProviderGuid", out temp));
                csUnit.Assert.True(im.GetFieldByName(TimerProvider.FIELD_TIMER_USER_DATA, out userData));
                csUnit.Assert.Equals("testUserSpecificTimerInformation", userData);

                temp = null;
                userData = null;
                im = null;

                im = this.pal.GetMessageFromQueue();
            }

            timer.ResetTimerTable();
        }
    }
}
