[Producer Consumer Problem using locks in C#.Net](https://pragmaticdevs.wordpress.com/2015/10/15/producer-consumer-problem-using-locks-in-c-net/)

What is a process? If we Open Task Manager , We can see process. These process are the applications working in our machine. Like Google Chrome like below



Each process has multiple threads in it. The Code in the Main Function handles is the main thread. All other thread we are creating is called a worker thread .

Example Thread program:


class Program
    {
        static void Main(string[] args)
        {
            Thread thread = new Thread(WriteAThreadFunction);
            //This is a worker thread
            thread.Name = "Abdul";
            thread.Start();
            Thread.CurrentThread.Name = "Abdul Main";
            //The below is a main thread
            for(int i = 0; i < 1000; i++)
            {
                Console.Write("A"+i+" ");
            }
            Console.Read();
        }

        private static void WriteAThreadFunction()
        {
            for (int i = 0; i < 1000; i++)
            {
                Console.Write("Z" + i + " ");
            }
        }
    }




How a thread works?

OS has a thread scheduler. Multi threading is managed internally by a thread scheduler
CLR (Common Language Runtime) delegates thread scheduling task to OS

What happens to Shared resources inside the process or the application?
CLR Assigns each thread its own local memory stack to keep local variables separate
This way local variables are completely separate. This means a separate copy of local variables is created on each threads memory stack. 
For example, I am running a FOR loop with values from 1 - 10 , it does not matter how many thread accessing this code, each thread maintains a local copy of the FOR loop variable

When Shared variables case, like static variables , how do you share the value between multiple threads ?

This is the responsibility of the thread scheduler to allocate appropriate execution time for each active thread

How above work for Single - Processor Computers?

In case of S P C, we use time slicing , which means the thread scheduler is going to rapidly switch the execution between each of the active threads
The timeslice typically takes tens of milliseconds in Windows . CPU overhead of switching between threads is a few microseconds

In Case of Multiprocessor Computers?

Different thread run code simultaneously on different processors

Preempted Threads 
If a thread execution is interrupted, usually by an external factor such as time slicing, its called preempted threads. Thread has no control over when and where ity is preempted

Lock is the main use when working with shared resources

/// <summary>
    /// In this case, Worker and Main share the same function HelloWorld(), so HelloWorld function is a shared resource
    /// The Hello world should only be print Once
    /// The issue happens if Worker thread and main thread calls Hello world, So two times the Function executes
    /// To prevent this , we can use lock and example is below
    /// </summary>
    class Program
    {
        public static bool IsCompleted { get; private set; }
        public static object checkLocks = new object();
        static void Main(string[] args)
        {
            Thread thread = new Thread(HellowWorld);
            //Worker Thread
            thread.Name = "Worker";
            thread.Start();
            //Main thread
            Thread.CurrentThread.Name = "Main";
            HellowWorld();
            Console.Read();
        }

        private static void HellowWorld()
        {
            lock (checkLocks)
            {
                if (!IsCompleted)
                {
                    IsCompleted = true;
                    Console.WriteLine("Hellow World");

                }
            }
        }
    }





Threads vs Process
Threads
Run in Parallel within in a single process
Share memory with other threads running in the same application
Thread has only a limited degree of isolation
Thread maintains heap memory with other threads running in the same application
Process
	Processes runs Parallel in a computer
	Processes are fully isolated from each other

Thread Pool 

We can set how many worker thread can be used in our application using ThreadPool.

Ex: Use ThreadPool.GetMaxThreadCount method retrieve the max worker threads supported by the computer….


Join Method:

This is used to join the worker thread to Main thread.

Eg:- main()
{
	Thread t = new Thread(HelloWorld);
	t.Start();
	Console.WriteLine(“Hello World is printed”);
}

Private static HelloWorld()
{
Console.WriteLine(“Hello World”);
t.Sleep(5000);//After Printing, wait for 5 second 
}

When we run this program, mostly main thread “Hello World is printed”” gets executed before Printing Hello World message inside HelloWorld() func. So in these scenario, we can use Join method. This basically Join the Worker thread with Main thread . So once Worker executed, Main will continue its execution. The modified code below

main()
{
	Thread t = new Thread(HelloWorld);
	t.Start();
	t.Join();
	Console.WriteLine(“Hello World is printed”);
}

Private static HelloWorld()
{
Console.WriteLine(“Hello World”);
t.Sleep(5000);//After Printing, wait for 5 second 
}

Exception handling in Threads
Check below Pgm: I am calling HelloWorld() from main() [I mean from main thread ]
& Exception caught in the main its self


If we Call HelloWorld from a Thread ( say worker thread ) , then The Main thread won’t able to catch the exception in main thread and Program crashes. To avoid that Handle exception in HelloWorld() instead of main function.



Threads vs Tasks

Why Tasks ?

Assume you need to return some value from a worker thread to Main function ( I mean main thread) , to do that , we need to Use sleep or Join functions and maintain a shared static variable and output the value to the shared variables, once Worker thread finish execution, Join it to main thread and then Print the value. Its works But lot of complications, this is where Tasks helps

Tasks : 
Higher level of abstractions
Capable of returning values
Very handy
Can be chained
Use thread pool
May be used for I/O bound operations 
There is a CPU Bound & I/O Bound means Out of process calls
Lets say we are calculating a factorial of 5000, this is a pure calculation and it takes some time to finish say 10 sec, this is a CPU intensive operation
In IO boud , lets say Our application is making a DB call or Call a Web Service to get some data like calling Google service for some search query and use the Ouput in our application, so this case, we don’t know how much time we need to block the program execution by the external  call to finish , so in this case we can use with some callback method, so once the call finish , call back executes, so we are not actually blocking other operations in the code to wait till the I/O Bound calls finishes

Task Examples

Simple Task

class Program
    {
        static void Main(string[] args)
        {
            Task task = new Task(SimpleMethod);
            task.Start();
            Console.ReadKey();
        }

        private static void SimpleMethod()
        {
            Console.WriteLine("Hello World");
        }
    }




Lets looks now how do we return a value from a method using Task


class Program
    {
        static void Main(string[] args)
        {
            Task task = new Task(SimpleMethod);
            task.Start();
           //Task<string> myReturnedValue : the <string> in this line is actually the return   value from the Task Thread
            //The <string> in ReturnValue function is the actual return value of the function 
            Task<string> myReturnedValue = new Task<string>(ReturnValue);

            Thread.Sleep(2000);//2 Sec
            myReturnedValue.Start();//Starting task
            myReturnedValue.Wait();//Waiting for Task finishes
            //Printing returned value
            Console.WriteLine(myReturnedValue.Result);
            Console.ReadKey();
        }

        private static string ReturnValue()
        {
            return "Hello";           
        }

        private static void SimpleMethod()
        {
            Console.WriteLine("Hello World");
        }
    }






Synchronization :

Coordinating actions of multiple threds or tasks running concurrently . Necessary to do sync to get predictable outcomes from multiple running threads

Different ways to do Sync

Blocking methods : Blocks execution until a task of thread is completed. Blocked threads do not consume CPU, they do consume memory
Sleep & 
Join 
Task.Wait
Spinning : Spinning consumes CPU, 
1.	Consume CPU as long as the thread is being blocked 
Like we have a while loop while ( i < x ) , as long as the condition is met, the execution takes CPU , this is called Spinning

Locks
Locks limit number of threads
Exclusive lock: Allow only one thread to access certain piece of code
We can also use Monitor.Enter , if we use that we need to use Monitor.Exit
Two Kinds of Exclusive locks, They are Lock and Mutex
		B.	Non Exclusive Locks
			Semaphore,		SemaphoreSlim,		Reader/Writer :Allow Multiple threads to access a resource

Semaphore allow multiple thread to access resource where as a Mutex only allow one thread to access a resource

3.	Signaling Constructs:
	A.	pause threads until they receive a signal from another thread
	Two common ways to do Signaling , they are 
	Event wait handles & monikors Wait/Pulse Methods


4. Non Blocking Syncronization
	1.	There are like Thread.MemoryBarrier, Thread.VolatileRead, and Thread.VolatileWrite , the volatile keyword and the interlocked class

Protect access to a common field

Examples :

Moniker Vs Locks


Account Class:
internal class Account
    {
        int balace;
        Object AbdulLock = new object();
        Random ramdom = new Random();
        public Account(int initialBalance)
        {
            balace = initialBalance;
        }
        int Withdraw(int amount)
        {
            if (balace < 0)
            {
                throw new Exception("Not enough balance");
            }
            Monitor.Enter(AbdulLock);
            try
            {
                if (balace >= amount)
                {
                    Console.WriteLine("Amount Withdrawn :" + amount);
                    balace = balace - amount;
                    return balace;
                }
            }
            finally
            {
                Monitor.Exit(AbdulLock);
            }
            return 0;
        }
        public void WithdrawRandomly()
        {
            for (int i = 0; i < 100; i++)
            {
                var balace = Withdraw(ramdom.Next(2000, 5000));
                if (balace > 0)
                {
                    Console.WriteLine("Balance left " + balace);
                }
            }
        }
    }
Bolded Section in Account class can be replaced with locks as given below

locks(AbdulLock)
{
  if (balace >= amount)
                {
                    Console.WriteLine("Amount Withdrawn :" + amount);
                    balace = balace - amount;
                    return balace;
                }
 return 0;
}



Main Program:

static void Main(string[] args)
        {
            Account account = new Account(20000);
            Task task1 = Task.Factory.StartNew(() => account.WithdrawRandomly());
            Task task2 = Task.Factory.StartNew(() => account.WithdrawRandomly());
            Task task3 = Task.Factory.StartNew(() => account.WithdrawRandomly());
            Task.WaitAll(task1, task2, task3);
            Console.WriteLine("All Tasks completed");
            Console.ReadKey();
        }





DeadLock Example:




class Program
    {

        static void Main(string[] args)
        {
            object caztonLock = new object();
            object chanderLock = new object();
            new Thread(() =>
            {
                lock(caztonLock)
                {
                    Console.WriteLine("caztonLock obtaind..Tid"+Thread.CurrentThread.ManagedThreadId);
                    Thread.Sleep(2000);
                    lock(chanderLock)
                    {
                        Console.WriteLine("chanderLock obtaind..Tid" + Thread.CurrentThread.ManagedThreadId);
                    }
                }
            }).Start();
            lock (chanderLock)
            {
                Console.WriteLine("chanderLock in main thread..Tid" + Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(1000);
                lock (caztonLock)
                {
                    Console.WriteLine("caztonLock in main thread..Tid" + Thread.CurrentThread.ManagedThreadId);
                }
            }
            Console.ReadKey();
        }
    }






Concrete class :  A concrete class means a actual class which we can instantiate the object, like a full fledged employee class

Abstract class : An abstraction is referred to as a general capability associated with a group of species where all properties are not applicable in the category but many specifications are common so we can abstract from the class

Say Animal is an abstract


We can derive Dog , Cat , Snake etc from it 

Like Domestic Animal , Wild Animal etc .. These Dog can be considered as a Concrete class

Factory Pattern :

We have a class : SimpleFactory with a method createProduct() thats create a product concrete Product.




So we have a Factory Class which Operate on products we create , but leaves the decisions to which product to make to its factory sub classes. Factory Super class specifies an abstract method createProduct() which all subclasses must implement

Different concrete product gets created based on which concrete factory the client is using


Observer Design pattern:
https://medium.com/better-programming/observer-vs-pub-sub-pattern-50d3b27f838c

the observer pattern is a software design pattern in which an object, called the subject, maintains a list of its dependents, called observers, and notifies them automatically of any state changes, usually by calling one of their methods.


Pub-Sub (Publisher-Subscriber) Design Pattern
The major difference between the (real) publisher-subscriber pattern and the observer pattern is this:
In the publisher-subscriber pattern, senders of messages, called publishers, do not program the messages to be sent directly to specific receivers, called subscribers.
This means that the publisher and subscriber don’t know about the existence of one another.
There is a third component, called broker, message broker or event bus, which is known by both the publisher and subscriber. It filters all incoming messages and distributes them accordingly.
In other words, pub-sub is a pattern used to communicate messages between different system components, without these components knowing anything about each other’s identity.

A Beginner's Guide to Design Patterns
Factory Pattern Example code



Factory Method Design Pattern - C#

namespace Factory
{
 /// <summary>
 /// The 'Product' interface
 /// </summary>
 public interface IFactory
 {
 void Drive(int miles);
 }

 /// <summary>
 /// A 'ConcreteProduct' class
 /// </summary>
 public class Scooter : IFactory
 {
 public void Drive(int miles)
 {
 Console.WriteLine("Drive the Scooter : " + miles.ToString() + "km");
 }
 }

 /// <summary>
 /// A 'ConcreteProduct' class
 /// </summary>
 public class Bike : IFactory
 {
 public void Drive(int miles)
 {
 Console.WriteLine("Drive the Bike : " + miles.ToString() + "km");
 }
 }

 /// <summary>
 /// The Creator Abstract Class
 /// </summary>
 public abstract class VehicleFactory
 {
 public abstract IFactory GetVehicle(string Vehicle);

 }

 /// <summary>
 /// A 'ConcreteCreator' class
 /// </summary>
 public class ConcreteVehicleFactory : VehicleFactory
 {
 public override IFactory GetVehicle(string Vehicle)
 {
 switch (Vehicle)
 {
 case "Scooter":
 return new Scooter();
 case "Bike":
 return new Bike();
 default:
 throw new ApplicationException(string.Format("Vehicle '{0}' cannot be created", Vehicle));
 }
 }

 }
 
 /// <summary>
 /// Factory Pattern Demo
 /// </summary>
 class Program
 {
 static void Main(string[] args)
 {
 VehicleFactory factory = new ConcreteVehicleFactory();

 IFactory scooter = factory.GetVehicle("Scooter");
 scooter.Drive(10);

 IFactory bike = factory.GetVehicle("Bike");
 bike.Drive(20);

 Console.ReadKey();

 }
 }




