using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

class Program
{
    static void Main()
    {
        int n = 40; // Fibonacci sequence number to calculate

        Console.WriteLine("Synchronous Operation:");
        MeasureTime(() => CalculateFibonacciSync(n));

        Console.WriteLine("\nAsynchronous Operation:");
        MeasureTime(() =>  CalculateFibonacciAsync(n));

        Console.WriteLine("\nMultithreaded Operation:");
        MeasureTime(() => CalculateFibonacciMultithreaded(n));

        Console.ReadLine();
    }

    static void MeasureTime(Action action)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        action();
        stopwatch.Stop();
        Console.WriteLine("Time taken: " + stopwatch.ElapsedMilliseconds + "ms");
    }

    static long Fibonacci(int n)
    {
        if (n <= 1)
            return n;
        return Fibonacci(n - 1) + Fibonacci(n - 2);
    }

    static void CalculateFibonacciSync(int n)
    {
        long result = Fibonacci(n);
        Console.WriteLine("Fibonacci(" + n + ") = " + result);
    }

    static async Task<long> FibonacciAsync(int n)
    {
        return await Task.Run(() => Fibonacci(n));
    }

    static void CalculateFibonacciAsync(int n)
    {
        long result = FibonacciAsync(n).Result;
        Console.WriteLine("Fibonacci(" + n + ") = " + result);
    }

    static void CalculateFibonacciMultithreaded(int n)
    {
        var block = new TransformBlock<int, long>(i => Fibonacci(i));
        long result = 0;

        for (int i = 0; i <= n; i++)
        {
            block.Post(i);
        }

        for (int i = 0; i <= n; i++)
        {
            result = block.Receive();
        }

        Console.WriteLine("Fibonacci(" + n + ") = " + result);
    }
}