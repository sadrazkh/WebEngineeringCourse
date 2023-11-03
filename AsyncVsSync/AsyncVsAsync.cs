using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        Console.WriteLine("Synchronous Operation:");
        MeasureTime(SynchronousDownloadWebPages);

        Console.WriteLine("\nAsynchronous Operation:");
        MeasureTime(AsynchronousDownloadWebPages);

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

    static void SynchronousDownloadWebPages()
    {
        for (int i = 1; i <= 5; i++)
        {
            string content = DownloadWebPage("https://soft98.ir/page/" + i);
            Console.WriteLine("Page " + i + " downloaded: " + content.Length + " bytes");
        }
    }

    static async void AsynchronousDownloadWebPages()
    {
        var tasks = new Task<string>[5];
        for (int i = 1; i <= 5; i++)
        {
            tasks[i - 1] = DownloadWebPageAsync("https://soft98.ir/page/" + i);
        }

        await Task.WhenAll(tasks);

        for (int i = 0; i < tasks.Length; i++)
        {
            Console.WriteLine("Page " + (i + 1) + " downloaded: " + tasks[i].Result.Length + " bytes");
        }
    }

    static string DownloadWebPage(string url)
    {
        WebClient webClient = new WebClient();
        string content = webClient.DownloadString(url);
        return content;
    }

    static async Task<string> DownloadWebPageAsync(string url)
    {
        WebClient webClient = new WebClient();
        string content = await webClient.DownloadStringTaskAsync(url);
        return content;
    }
}