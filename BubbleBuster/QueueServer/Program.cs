using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using QueueServer.Classes;

namespace QueueServer
{
    public class Program
    {
        static void Main(string[] args)
        {
            TaskQueue();
        }
        private static async void TaskQueue()
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
                }

                while (runningTasksList.Count < taskLimit)
                {
                    if (taskQueue.Count != 0)
                    {
                        runningTasksList.Add(taskQueue.Peek());
                        await RunTaskAsync(taskQueue.Dequeue());
                    }
                }
                
                Thread.Sleep(420); //so it doesn't check all the time.
                Task.WaitAll(runningTasksList.ToArray());
            }           
        }

        private static bool ThereIsNewTask()
        {
            bool newTaskYes = false;
            /* check for new tasks */
            
            return newTaskYes;
        }

        private static Task RunTaskAsync(Task task)
        {
            return Task.Run(() =>
            {
                RunTask(task);
            });
        }
        private static void RunTask (Task task)
        {
            task.Start();
        }

        private void 
    }
}
