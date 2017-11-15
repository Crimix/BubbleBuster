﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Runtime.InteropServices;
using BubbleBuster.Helper;

namespace QSLib
{
    public class QueueServerInstance
    {
        private static QueueServerInstance _instance;

        private QueueServerInstance() { }

        public static QueueServerInstance Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new QueueServerInstance();
                return _instance;
            }
        }

        Queue<TwitterAcc> nonAddedRequests = new Queue<TwitterAcc>();
        public async void TaskQueue()
        {
            const int taskLimit = 5;
            Log.Info("test");
            Queue<Task> taskQueue = new Queue<Task>();
            List<Task> runningTasksList = new List<Task>();
            while (true)
            {
                while (ThereIsNewTask())
                {
                    if (nonAddedRequests.Count != 0)
                    {
                        TwitterAcc input = nonAddedRequests.Peek();

                        Task newTask = new Task(() =>
                        {
                            ServerTask st = new ServerTask(input);
                            st.Run();
                        });
                        taskQueue.Enqueue(newTask);
                        nonAddedRequests.Dequeue();
                    }
                }

                while (runningTasksList.Count < taskLimit)
                {
                    if (taskQueue.Count > 0)
                    {
                        Log.Info("Start one task");
                        runningTasksList.Add(taskQueue.Peek());
                        await RunTaskAsync(taskQueue.Dequeue());

                    }
                    else
                        break; 
                }
                if(runningTasksList.Count > 0)
                {
                    runningTasksList.RemoveAt(Task.WaitAny(runningTasksList.ToArray()));
                }
            }
        }

        private bool ThereIsNewTask()
        {
            return nonAddedRequests.Count > 0;
        }

        private Task RunTaskAsync(Task task) //Wrapper
        {
            return Task.Run(() =>
            {
                RunTask(task);
            });
        }
        private void RunTask(Task task)
        {
            task.Start();
        }

        public bool AddTask (TwitterAcc tAcc)
        {
            bool wasSuccesful = true;
            Log.Info("Added task");
            try
            {
                nonAddedRequests.Enqueue(tAcc);
            }
            catch (Exception) //It does not matter what the exception is.
            {
                wasSuccesful = false;
            }

            return wasSuccesful;
        }
    }
}
