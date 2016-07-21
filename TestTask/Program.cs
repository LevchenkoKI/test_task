using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestTask
{
    class Program
    {
        private static Compiler compiler = new Compiler();
        private static Queue<Task<byte[]>> tasks = new Queue<Task<byte[]>>();
        public static async Task<byte[]> BuildExtensial(string projectPath)
        {

            lock (tasks)
            {
                tasks.Enqueue(new Task<byte[]>(() => compiler.BuildProject(projectPath)));
            }
            return await ExecuteTask();
        }

        private static async Task<byte[]> ExecuteTask()
        {
            Task<byte[]> executingTask;
            lock (tasks)
            {

                executingTask = tasks.Dequeue();
                if (!executingTask.IsCompleted)
                    executingTask.Start();
                Console.WriteLine(executingTask.Id);
            }
            return await executingTask;

        }
        static void Main(string[] args)
        {

         
        }
    }
    public class Compiler
    {
        public byte[] BuildProject(string projectPath)
        {
            // Имитируем бурную деятельность.
            Thread.Sleep(5000);

            // В реальности здесь будут байты собранной dll-ки.
            return Encoding.UTF8.GetBytes(projectPath);
        }
    }
    
}
