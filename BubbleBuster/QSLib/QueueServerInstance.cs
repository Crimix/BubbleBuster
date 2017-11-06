using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

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

            Queue<Task> taskQueue = new Queue<Task>();
            List<Task> runningTasksList = new List<Task>();
            while (true)
            {
                while (ThereIsNewTask())
                {
                    Task newTask = Task.Factory.StartNew(() =>
                    {
                        ServerTask st = new ServerTask();

                    });
                    taskQueue.Enqueue(newTask);
                    nonAddedRequests.Dequeue();
                }

                while (runningTasksList.Count < taskLimit)
                {
                    if (taskQueue.Count != 0)
                    {
                        runningTasksList.Add(taskQueue.Peek());
                        await RunTaskAsync(taskQueue.Dequeue());

                    }
                }
                runningTasksList.RemoveAt(Task.WaitAny(runningTasksList.ToArray()));
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
