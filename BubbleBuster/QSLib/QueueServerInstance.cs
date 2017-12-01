using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Threading.Tasks;
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

        Queue<TwitterAcc> nonAddedRequests = new Queue<TwitterAcc>(); //Requests from the browser goes into here
        public async void TaskQueue() //The main function of the queue server
        {
            const int TASKLIMIT = 5;
            Log.Info("test");
            Queue<Task> taskQueue = new Queue<Task>(); //Queue of tasks that are not started yet
            List<Task> runningTasksList = new List<Task>(); //List of currently running tasks.
            while (true) //runs for as long as the server is up.
            {
                while (ThereIsNewTask())
                {
                    if (nonAddedRequests.Count > 0) // Adds all requests to the queue as tasks.
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

                while (runningTasksList.Count < TASKLIMIT) //Starts tasks from the queue until there are none left or the limit is reached.
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
                if(runningTasksList.Count > 0) // If any tasks are running, it removes the completed ones from the list.
                {
                    List<Task> runningTasksListInstance = new List<Task>();
                    runningTasksListInstance.AddRange(runningTasksList);
                    foreach (var task in runningTasksListInstance) //ToDo Fik en exception her, omkring samling blev ændret
                    {
                        if (task.IsCompleted)
                        {
                            runningTasksList.Remove(task);
                        }
                    }
                }
            }
        }

        private bool ThereIsNewTask() //Checks if there are any requests
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

        public bool AddTask (TwitterAcc tAcc) //The method used by the server to add requests to the request queue.
        {
            bool wasSuccesful = true;
            Log.Info("Added task");

            try
            {
                //If the twitter account does not exist or if any of the two keys do not contain proper information, it can not be added.
                if (tAcc == null || (String.IsNullOrWhiteSpace(tAcc.Name) || String.IsNullOrWhiteSpace(tAcc.Token) || String.IsNullOrWhiteSpace(tAcc.Secret)))
                {
                    return false;
                }
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
