using System;
using System.Threading;

public class Stopwatch
{
    public delegate void StopwatchEventHandler(string message);
    
    public event StopwatchEventHandler OnStarted;
    public event StopwatchEventHandler OnStopped;
    public event StopwatchEventHandler OnReset;

    private TimeSpan timeElapsed;
    private bool isRunning;
    private Thread timerThread;

    public TimeSpan TimeElapsed => timeElapsed;

    public void Start()
    {
        if (!isRunning)
        {
            isRunning = true;
            timerThread = new Thread(Tick);
            timerThread.Start();
            OnStarted?.Invoke("Stopwatch Started!");
        }
    }

    public void Stop()
    {
        if (isRunning)
        {
            isRunning = false;
            timerThread?.Join();
            OnStopped?.Invoke("Stopwatch Stopped! Time Elapsed: " + timeElapsed);
        }
    }

    public void Reset()
    {
        isRunning = false;
        timeElapsed = TimeSpan.Zero;
        timerThread?.Join();
        OnReset?.Invoke("Stopwatch Reset!");
    }

    private void Tick()
    {
        while (isRunning)
        {
            Thread.Sleep(1000);
            timeElapsed = timeElapsed.Add(TimeSpan.FromSeconds(1));
            Console.WriteLine("Time Elapsed: " + timeElapsed);
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Stopwatch stopwatch = new Stopwatch();

        // Subscribe to events
        stopwatch.OnStarted += (message) => Console.WriteLine(message);
        stopwatch.OnStopped += (message) => Console.WriteLine(message);
        stopwatch.OnReset += (message) => Console.WriteLine(message);

        Console.WriteLine("Press 'S' to start, 'T' to stop, 'R' to reset, and 'Q' to quit.");

        while (true)
        {
            var key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.S)
            {
                stopwatch.Start();
            }
            else if (key == ConsoleKey.T)
            {
                stopwatch.Stop();
            }
            else if (key == ConsoleKey.R)
            {
                stopwatch.Reset();
            }
            else if (key == ConsoleKey.Q)
            {
                break;
            }
        }
    }
}