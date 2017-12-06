using BubbleBuster.Helper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QSLib
{
    public class QueueServerInstance
    {
        //The instance variable
        private static QueueServerInstance _instance;

        //Private constructor such that it is a singleton 
        private QueueServerInstance() { }

        /// <summary>
        /// Method to get access to the TweetRetriever.
        /// Is the only static method, because it is not possible to create an instance outside of this class
        /// </summary>
        public static QueueServerInstance Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new QueueServerInstance();
                return _instance;
            }
        }

        private Queue<TwitterAcc> nonAddedRequests = new Queue<TwitterAcc>(); //Requests from the browser goes into here

        /// <summary>
        /// //The main function of the queue server
        /// </summary>
        public async void TaskQueue()
        {
            const int TASKLIMIT = 5;
            Queue<Task> taskQueue = new Queue<Task>(); //Queue of tasks that are not started yet
            List<Task> runningTasksList = new List<Task>(); //List of currently running tasks.
            while (true) //runs for as long as the server is up.
            {
                while (ThereIsNewTask()) // Adds all requests to the queue as tasks.
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

                while (runningTasksList.Count < TASKLIMIT) //Starts tasks from the queue until there are none left or the limit is reached.
                {
                    if (taskQueue.Count > 0)
                    {
                        runningTasksList.Add(taskQueue.Peek());
                        await RunTaskAsync(taskQueue.Dequeue());
                    }
                    else
                        break;
                }
                if (runningTasksList.Count > 0) // If any tasks are running, it removes the completed ones from the list.
                {
                    List<Task> runningTasksListInstance = new List<Task>();
                    runningTasksListInstance.AddRange(runningTasksList);
                    foreach (var task in runningTasksListInstance)
                    {
                        if (task.IsCompleted)
                        {
                            runningTasksList.Remove(task);
                        }
                    }
                }
            }
        }

        //Checks if there are any requests
        private bool ThereIsNewTask()
        {
            return nonAddedRequests.Count > 0;
        }

        //Wrapper
        private Task RunTaskAsync(Task task)
        {
            return Task.Run(() =>
            {
                RunTask(task);
            });
        }

        //Starts the task
        private void RunTask(Task task)
        {
            task.Start();
        }

        //The method used by the server to add requests to the request queue.
        public bool AddTask(TwitterAcc tAcc)
        {
            bool wasSuccesful = true;
            Log.Debug("Added task");

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
