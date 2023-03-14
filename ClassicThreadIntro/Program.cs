using System.Diagnostics;

public class Program
{
    //private static void Main(string[] args)
    //{
    //    Console.WriteLine("Threading in C#");
    //    // Inbuilt thread in the main function is called the main thread.
    //    // all other threads we creates in the program is called worker threads
    //    Thread thread = new Thread(WriteAThreadFunction);// This is a worker thread
    //    thread.Name = "Abdul Worker Thread";
    //    thread.Start();
    //    Thread.CurrentThread.Name = "Abdul Main Thread";

    //    for (int i = 0; i < 75; i++)
    //    {
    //        Console.Write($"     <=Main {i}=>    ");
    //    }

    //    Console.Read();
    //}

    //public static void WriteAThreadFunction()
    //{
    //    for (int i = 0; i < 50; i++)
    //    {
    //        Console.Write($"     [Worker{i}]     ");
    //    }
    //}

    //public static bool IsCompleted { get; private set; }//Once it is assigined, then do not modify that the meaning for private set and which shoulde be inside program class
    //public static object checkLocks = new object();
    //public static void Main(string[] args)
    //{
    //    Thread thread = new Thread(HelloWorld);
    //    thread.Name = "Worker";
    //    thread.Start();

    //    Thread.CurrentThread.Name = "Main";
    //    HelloWorld();

    //    Console.Read();
    //}

    //public static void HelloWorld()
    //{
    //    Console.WriteLine($"Current Thread = {Thread.CurrentThread.Name}");
    //    lock (checkLocks)
    //    {
    //        if (IsCompleted == false)
    //        {
    //            IsCompleted = true;
    //            Console.WriteLine($"Hello World from thread {Thread.CurrentThread.Name}");
    //        }
    //    }
    //}

    public static void Main(string[] args)
    {
        Thread t = new Thread(HelloWorld);
        t.Name = "Worker";
        t.Start();
        Console.WriteLine("HelloWorld Main");
       
    }
    public static void HelloWorld()
    {
        Console.WriteLine($"HelloWorld from {Thread.CurrentThread.Name}");
        Thread.Sleep(5000);
    }
}