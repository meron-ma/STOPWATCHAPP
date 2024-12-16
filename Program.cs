using System;
using System.Threading;

public class Stopwatch
{
    private TimeSpan timeElapsed;
    private bool isRunning;
    private Thread tickThread;

    public delegate void StopwatchEventHandler(string message);
    public event StopwatchEventHandler OnStarted;
    public event StopwatchEventHandler OnStopped;
    public event StopwatchEventHandler OnReset;

    public TimeSpan TimeElapsed => timeElapsed;
    public bool IsRunning => isRunning;

    public void Start()
    {
        if (!isRunning)
        {
            isRunning = true;
            tickThread = new Thread(Tick);
            tickThread.Start();
            OnStarted?.Invoke("Stopwatch Started!");
        }
    }

    public void Stop()
    {
        if (isRunning)
        {
            isRunning = false;
            
            OnStopped?.Invoke($"Stopwatch Stopped! Time Elapsed: {timeElapsed}");
        }
    }

    public void Reset()
    {
        Stop();
        timeElapsed = TimeSpan.Zero;
       
        OnReset?.Invoke("Stopwatch Reset!");
    }

    private void Tick()
    {
        while (isRunning)
        {
            Thread.Sleep(1000); 
            if (isRunning)  
            {
                timeElapsed = timeElapsed.Add(TimeSpan.FromSeconds(1));
                Console.WriteLine($"Time Elapsed: {timeElapsed}");
            }
        }
    }

    public void StopThread()
    {
       
        if (tickThread != null && tickThread.IsAlive)
        {
            tickThread.Abort();
            tickThread = null;
        }
    }
}

class Program
{
    static void Main()
    {
        var stopwatch = new Stopwatch();
        
        // Subscribe to events
        stopwatch.OnStarted += message => Console.WriteLine(message);
        stopwatch.OnStopped += message => Console.WriteLine(message);
        stopwatch.OnReset += message => Console.WriteLine(message);

        Console.WriteLine("Press S to start, T to stop, R to reset, Any key to exit.");

        while (true)
        {
            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.S:
                    stopwatch.Start();
                    break;
                case ConsoleKey.T:
                    stopwatch.Stop();
                    break;
                case ConsoleKey.R:
                    stopwatch.Reset();
                    break;
                
                default:
                   Console.WriteLine("Exiting...");
                    stopwatch.StopThread(); 
                    return; // Exit the application
            }
        }
    }
}
