# CSharpThreadDemo

##### Both Thread and Task are used for concurrent programming, but Task were later introduced in .NET FW 4 

What is the limitation of Thread ?

Before .Net Framework 4, the only way to do concurrent programming was to use the Thread class. An instance of this class represents a managed thread
that the OS internally schedules. 

```
double exp1 = 0;
var t = new Thread(()=> { exp1 = Math.Exp(40); });
t.Start();
// ...

t.Join()
```
In the above program, we are spawning a new thread to do a **complex** computation. When we call `Join()` method, the current thread waits until the 
target thread terminates.

There is nothing wrong in this code, but we should be aware that spawing a thread has a cost. **we should never create a new thread every time we need to execute a task. because thread initialization and disposal are expensive activities for the Operating System**. A web Server which creates a new thread for each request would not work
when the load is high.

### From Thread to Thread Pool Threads 

MicroSoft decided to introduce a thread pool implementation inside the CLR. When App starts, the ThreadPool contains no threads. they are created on demand when the app needs them. after the thread completes, it is not destroyed , unless it remains inactive for a long time. Instead, it returned to the thread pool in a suspended state and is awakened whenever necessary. The thread pool implementation also has some configurable parameters , like the minimum and maximum threads that can be spawned. 

### Task

Task Class ( https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task?view=net-7.0 )

A `Task` is a abstraction that allows us to easily use the thread pool or native threads without the much complexity. by default, each `Task`  is executed in the background thread that belongs to a thread pool.

```
var task = Task.Run(() => Math.Exp(40));

// ...

var taskResult = task.Result;

```







